using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class Server : MonoBehaviour
{
    HELPER.ITEMS[,] matrixServer = new HELPER.ITEMS[HELPER.MATRIX_LENGHT, HELPER.MATRIX_WIDTH];

    public S_Proxy proxy;

    private void Awake()
    {
        initMatrix();
    }

    public void initMatrix()
    {
        Array values = Enum.GetValues(typeof(HELPER.ITEMS));
        System.Random random = new System.Random();

        for (int i = 0; i < matrixServer.GetLength(0); i++)
        {
            for (int j = 0; j < matrixServer.GetLength(1); j++)
            {
                HELPER.ITEMS randomItem = (HELPER.ITEMS)values.GetValue(random.Next(values.Length - 1));
                matrixServer[i, j] = randomItem;
            }
        }

        proxy.sendStartMatrixToClient(matrixServer);
    }

    public void updateMatrix(HELPER.ITEMS[,] matrix)
    {
        HELPER.ITEMS[,] addedMatrix = new HELPER.ITEMS[matrix.GetLength(0), matrix.GetLength(1)];

        Array values = Enum.GetValues(typeof(HELPER.ITEMS));
        System.Random random = new System.Random();


        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j].Equals(HELPER.ITEMS.NULL))
                {

                    HELPER.ITEMS randomItem = (HELPER.ITEMS)values.GetValue(random.Next(values.Length - 1));
                    addedMatrix[i, j] = randomItem;
                    matrixServer[i, j] = randomItem;
                }
                else
                {
                    addedMatrix[i, j] = HELPER.ITEMS.NULL;
                }
            }
        }
        proxy.sendMatrixToClient(addedMatrix);
    }

    private HELPER.ITEMS[,] movePiecesToBottom(HELPER.ITEMS[,] matrix)
    {
        HELPER.ITEMS currentItem;
        HELPER.ITEMS upperItem;
        int upperItemJ;
        for (int j = matrix.GetLength(1) - 1; j > 0; j--)
        {
            upperItemJ = j - 1;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (!matrix[i, upperItemJ].Equals(HELPER.ITEMS.NULL) && matrix[i, j].Equals(HELPER.ITEMS.NULL))
                {
                    currentItem = matrix[i, j];
                    upperItem = matrix[i, upperItemJ];

                    if (true /*на будущее, если будут недвжик камни*/)
                    {
                        matrix[i, j] = upperItem;
                        matrix[i, upperItemJ] = HELPER.ITEMS.NULL;
                    }
                }
            }
        }
        return matrix;
    }

    protected bool isAdjacent(int x1, int y1, int x2, int y2)
    {
        if (x1 == x2 && (int)Mathf.Abs(y1 - y2) == 1 ||
        y1 == y2 && (int)Mathf.Abs(x1 - x2) == 1)
        {
            return true;
        }

        return false;
    }

    public void swap2Pieces(int x1, int y1, int x2, int y2)
    {
        if (isAdjacent(x1, y1, x2, y2))
        {
            matrixServer[x1, y1] = matrixServer[x2, y2];
            matrixServer[x2, y2] = matrixServer[x1, y1];
            proxy.sentAnswerToSwap(x1, y1, x2, y2);
        }
       // checkAllPieces();
    }
    /*public void checkAllPieces()
    {
        HELPER.ITEMS[,] itemsNeedToDestroy= new HELPER.ITEMS[matrixServer.GetLength(0), matrixServer.GetLength(1)];
        for (int i = 0; i < matrixServer.GetLength(0); i++)
            for (int j = 0; j < matrixServer.GetLength(1); j++)
            {
                if (!matrixServer[i, j].Equals(HELPER.ITEMS.NULL))
                {
                    List<int[,]> result = checkMuch(i, j, matrixServer[i, j]);

                    if (result.Count != 0)
                    {
                        foreach (int[,] item in result)
                        {
                            int[,] pos = item;
                            matrixServer[pos[0, 0], pos[1, 1]] = HELPER.ITEMS.NULL;
                            itemsNeedToDestroy[pos[0, 0], pos[1, 1]] = HELPER.ITEMS.NULL;
                            print(pos[0, 0] + " " + pos[1, 1]);
                        }
                        proxy.destroyPiece(itemsNeedToDestroy);
                    }
                }
            }
           //updateMatrix(matrixServer);
    }
    private List<int[,]> checkMuch(int x, int y, HELPER.ITEMS type)
    {
        List<int[,]> results = new List<int[,]>();
        List<int[,]> horisontalResults = new List<int[,]>();
        List<int[,]> verticalResults = new List<int[,]>();
        int[,] pos = new int[2, 2];

        horisontalResults = checkHorisontal(x, y, type);
        verticalResults = checkVertical(x, y, type);

        if (horisontalResults.Count > 1)
        {
            results.AddRange(horisontalResults);
        }

        if (verticalResults.Count > 1)
        {
            results.AddRange(verticalResults);
        }

        if (results.Count > 1)
        {
            pos[0, 0] = x;
            pos[1, 1] = y;
            results.Add(pos);
        }

        return results;
    }

    private List<int[,]> checkVertical(int x, int y, HELPER.ITEMS type)
    {
        List<int[,]> result = new List<int[,]>();
        int[,] pos = new int[2, 2];

        for (int j = (y + 1); j < matrixServer.GetLength(1); j++)
        {
            if (!matrixServer[x, j].Equals(HELPER.ITEMS.NULL) && matrixServer[x, j].Equals(type))
            {
                pos[0, 0] = x;
                pos[1, 1] = j;
                result.Add(pos);
            }
            else
            {
                break;
            }
        }

        for (int j = (y - 1); j >= 0; j--)
        {
            if (!matrixServer[x, j].Equals(HELPER.ITEMS.NULL) && matrixServer[x, j].Equals(type))
            {
                pos[0, 0] = x;
                pos[1, 1] = j;
                result.Add(pos);
            }
            else
            {
                break;
            }
        }

        return result;
    }


    private List<int[,]> checkHorisontal(int x, int y, HELPER.ITEMS type)
    {
        List<int[,]> result = new List<int[,]>();
        int[,] pos = new int[2, 2];

        for (int i = x + 1; i < matrixServer.GetLength(0); i++)
        {
            if (!matrixServer[i, y].Equals(HELPER.ITEMS.NULL) && matrixServer[i, y].Equals(type))
            {
                pos[0, 0] = i;
                pos[1, 1] = y;
                result.Add(pos);
            }
            else
            {
                break;
            }
        }

        for (int i = x - 1; i >= 0; i--)
        {
            if (!matrixServer[i, y].Equals(HELPER.ITEMS.NULL) && matrixServer[i, y].Equals(type))
            {
                pos[0, 0] = i;
                pos[1, 1] = y;
                result.Add(pos);
            }
            else
            {
                break;
            }
        }

        return result;
    }*/
}
