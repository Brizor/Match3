using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePiece : MonoBehaviourEx
{
    private int _x;
    private int _y;
    public HELPER.ITEMS _type;
    private DinamicGrid _gridRef;
    private MovablePiece _moveComponent;

    public int x
    {
        get
        {
            return _x;
        }

        set
        {
            _x = value;
        }
    }

    public int y
    {
        get
        {
            return _y;
        }

        set
        {
            _y = value;
        }
    }

    public DinamicGrid gridRef
    {
        get
        {
            return _gridRef;
        }
    }

    public bool isMovable()
    {
        return _moveComponent != null;
    }

    public MovablePiece moveComponent
    {
        get
        {
            return _moveComponent;
        }
    }

  /*  private void OnMouseDown()
    {
        dispatchEvent(new PiecesIvents(PiecesIvents.EventsWithPieces.DESTROY.ToString(), this.name, true, x, y));
    }*/

    void OnMouseDown()
    {
        _gridRef.onPieceDown(this);
    }

    void OnMouseEnter()
    {
        _gridRef.onPieceEnter(this);
        _gridRef.onPieceUp();
    }

    public void init(int initX, int initY, HELPER.ITEMS type, DinamicGrid newGridRef)
    {
        _x = initX;
        _y = initY;
        _type = type;
        _gridRef = newGridRef;

        _moveComponent = GetComponent<MovablePiece>();
    }

}
