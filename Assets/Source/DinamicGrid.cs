using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinamicGrid : BaseGrid
{
    protected BasePiece pieceUP;
    protected BasePiece pieceDOWN;

    public void onPieceDown(BasePiece value)
    {
        pieceDOWN = value;
    }

    public void onPieceEnter(BasePiece value)
    {
        pieceUP = value;
    }

    public void onPieceUp()
    {
        if (pieceDOWN != null && pieceUP != null)
        {
            proxy.swap_request( pieceUP.x, pieceUP.y, pieceDOWN.x, pieceDOWN.y);
        }
    }

    public void startSwapPieces(int x1, int y1, int x2, int y2)
    {
        BasePiece piece1 = _pieces[x1, y1];
        BasePiece piece2 = _pieces[x2, y2];
        StartCoroutine(swapPieces(piece1, piece2));
    }


    private IEnumerator swapPieces(BasePiece piece1, BasePiece piece2)
    {
        pieceUP = null;
        pieceDOWN = null;

        if (piece1.isMovable() && piece2.isMovable())
        {
            if (!piece1.Equals(HELPER.ITEMS.NULL) && !piece2.Equals(HELPER.ITEMS.NULL))
            {
                swap2Pieces(piece1, piece2, true);

                yield return new WaitForSeconds(HELPER.FILLD_GREED_STEP);
            }
        }

         //test

    }
    private void swap2Pieces(BasePiece piece1, BasePiece piece2, bool isAnimated = false)
    {
        int piece1X = piece1.x;
        int piece1Y = piece1.y;
        int piece2X = piece2.x;
        int piece2Y = piece2.y;

        _pieces[piece1X, piece1Y] = piece2;
        _pieces[piece2X, piece2Y] = piece1;

        if (isAnimated)
        {
            piece1.moveComponent.move(piece2X, piece2Y, HELPER.FILLD_GREED_STEP);
            piece2.moveComponent.move(piece1X, piece1Y, HELPER.FILLD_GREED_STEP);
        }
        else
        {
            piece1.x = piece2X;
            piece1.y = piece2Y;
            piece2.x = piece1X;
            piece2.y = piece1Y;
        }

        checkMatrix(_pieces, server.matrixServer);
        proxy.reaction_request(repotToServer(_pieces));
    }

}
