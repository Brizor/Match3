using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGrid : MonoBehaviour
{
    public C_Proxy proxy;

    public Transform grid;
    public Transform gridBackground;
    public GameObject backgroundPiece;
    private HELPER.ITEMS[,] addedMatrix;

    private float xDisp, yDisp;
    private Dictionary<HELPER.ITEMS, GameObject> _piecePrefabsDict;

    [System.Serializable]
    public struct PiecePrefab
    {
        public HELPER.ITEMS type;
        public Sprite sprite;
        public GameObject prefab;
    }

    public PiecePrefab[] piecePrefabArray;

    protected BasePiece[,] _pieces;

    private void Start()
    {
        _piecePrefabsDict = new Dictionary<HELPER.ITEMS, GameObject>();

        foreach (PiecePrefab prefab in piecePrefabArray)
        {
            if (!_piecePrefabsDict.ContainsKey(prefab.type))
            {
                _piecePrefabsDict.Add(prefab.type, prefab.prefab);
            }
        }
    }

    public void createGrid(HELPER.ITEMS[,] matrix)
    {
        _pieces = new BasePiece[matrix.GetLength(0), matrix.GetLength(1)];

        xDisp = (matrix.GetLength(0) - 1) / 2.0f * HELPER.PIECE_OFFCET;
        yDisp = (matrix.GetLength(1) - 1) / 2.0f * HELPER.PIECE_OFFCET;

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                GameObject pieceBackground = (GameObject)Instantiate(backgroundPiece, getGridPosition(i, j, 0), Quaternion.identity);
                pieceBackground.transform.parent = gridBackground;
                createGamePiece(i, j, matrix[i, j]);
            }
        }
    }



    private BasePiece createGamePiece(int x, int y, HELPER.ITEMS type, int outSide = 0)
    {
        GameObject newPiece = Instantiate(_piecePrefabsDict[type], getGridPosition(x, y + outSide), Quaternion.identity);
        newPiece.GetComponent<SpriteRenderer>().sprite = getPieceView(type);
        newPiece.transform.parent = grid;

        _pieces[x, y] = newPiece.GetComponent<BasePiece>();
        _pieces[x, y].init(x, y, type, this as DinamicGrid);
        return _pieces[x, y];
    }

    public Vector3 getGridPosition(int x, int y, int z = -1)
    {
        float newX = grid.position.x + x * backgroundPiece.transform.localScale.x * HELPER.PIECE_OFFCET - xDisp;
        float newY = grid.position.y - y * backgroundPiece.transform.localScale.y * HELPER.PIECE_OFFCET + yDisp;
        return new Vector3(newX, newY, z);
    }

    private Sprite getPieceView(HELPER.ITEMS type)
    {
        foreach (PiecePrefab item in piecePrefabArray)
        {
            if (item.type == type)
            {
                return item.sprite;
            }
        }
        return null;
    }

    protected bool isAdjacent(BasePiece piece1, BasePiece piece2)
    {
        if (piece1.x == piece2.x && (int)Mathf.Abs(piece1.y - piece2.y) == 1 ||
        piece1.y == piece2.y && (int)Mathf.Abs(piece1.x - piece2.x) == 1)
        {
            return true;
        }

        return false;
    }

    public void destroyPiece(int x, int y)
    {
        Destroy(_pieces[x, y].gameObject);
        _pieces[x, y] = null;
        //  movePiecesToBottom();
    }

  /*  public void reaction(HELPER.ITEMS[,] res)
    {
        for (int i = 0; i < res.GetLength(0); i++)
        {
            for (int j = 0; j < res.GetLength(1); j++)
            {
                if (res[i, j].Equals(HELPER.ITEMS.NULL))
                {
                    destroyPiece(i, j);
                }
            }
        }
        movePiecesToBottom();
    }*/

    public HELPER.ITEMS[,] repotToServer(BasePiece[,] piece)
    {
        HELPER.ITEMS[,] mesMatrix = new HELPER.ITEMS[piece.GetLength(0), piece.GetLength(1)];
        for (int i = 0; i < mesMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < mesMatrix.GetLength(1); j++)
            {
                if (piece[i, j] == null)
                {
                    mesMatrix[i, j] = HELPER.ITEMS.NULL;
                }
                else
                {
                    mesMatrix[i, j] = piece[i, j]._type;
                }
            }
        }
        return mesMatrix;
    }

    public void movePiecesToBottom()
    {
        BasePiece currentItem;
        BasePiece upperItem;
        int upperItemJ;
        for (int j = _pieces.GetLength(1) - 1; j > 0; j--)
        {
            upperItemJ = j - 1;
            for (int i = 0; i < _pieces.GetLength(0); i++)
            {
                if (_pieces[i, upperItemJ] != null && _pieces[i, j] is null)
                {
                    currentItem = _pieces[i, j];
                    upperItem = _pieces[i, upperItemJ];

                    if (upperItem.isMovable())
                    {
                        upperItem.moveComponent.move(i, j, HELPER.FILLD_GREED_STEP);
                        upperItem.x = i;
                        upperItem.y = j;
                        _pieces[i, j] = upperItem;
                        _pieces[i, upperItemJ] = null;
                    }
                }
            }
        }
    }

    public void updateGrid(HELPER.ITEMS[,] matrix)
    {
        addedMatrix = matrix;

        StartCoroutine(updateGridCurotine());
    }

    private IEnumerator updateGridCurotine()
    {
        for (int j = 0; j < addedMatrix.GetLength(1); j++)
        {
            for (int i = 0; i < addedMatrix.GetLength(0); i++)
            {
                if (!addedMatrix[i, j].Equals(HELPER.ITEMS.NULL))
                {
                    createGamePiece(i, j, addedMatrix[i, j], -1).moveComponent.move(i, j, HELPER.FILLD_GREED_STEP);
                }
            }
            movePiecesToBottom();
            yield return new WaitForSeconds(HELPER.PIECE_OFFCET);
        }
    }


}
