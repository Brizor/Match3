using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Proxy : MonoBehaviourEx
{
    public DinamicGrid client;

    void Awake()
    {
        addEventListener(MatrixEvent.EventsWithMatrix.INITIALIZATION.ToString(), getStartMatrixFromServer);
        addEventListener(MatrixEvent.EventsWithMatrix.ANSWER_UPDATE.ToString(), getMatrixFromServer);
        addEventListener(MatrixEvent.EventsWithMatrix.DESTROY.ToString(), destroyPieces);
        //addEventListener(PieceIvents.EventsWithPiece.DESTROY.ToString(), destroyPieces);
        addEventListener(EventsWithPieces.PiecesEvents.ANSWER_TO_SWAP.ToString(), getAnswerToSwap);
    }

    private void destroyPieces(BaseEvent cutchEvent)
    {
        /*MatrixEvent custEvent = cutchEvent as MatrixEvent;
        print("asdsadd");
        client.reaction(custEvent.matrix);*/
    }

    private void getStartMatrixFromServer(BaseEvent cutchEvent)
    {
        MatrixEvent custEvent = cutchEvent as MatrixEvent;
        client.createGrid(custEvent.matrix);
    }

    private void getMatrixFromServer(BaseEvent cutchEvent)
    {
        MatrixEvent custEvent = cutchEvent as MatrixEvent;
        client.updateGrid(custEvent.matrix);
    }

    public void updateMatrix(HELPER.ITEMS[,] matrix)
    {
        dispatchEvent(new MatrixEvent(MatrixEvent.EventsWithMatrix.REQUEST_UPADTE.ToString(), this.name, true, matrix));
    }

    private void getAnswerToSwap(BaseEvent cutchEvent)
    {
        EventsWithPieces custEvent = cutchEvent as EventsWithPieces;
        client.startSwapPieces(custEvent.x1, custEvent.y1, custEvent.x2, custEvent.y2);
    }

    public void sendRequestToSwap(int x1, int y1, int x2, int y2)
    {
        dispatchEvent(new EventsWithPieces(EventsWithPieces.PiecesEvents.REQUEST_TO_SWAP.ToString(), this.name, true, x1, y1, x2, y2));
    }

    public void OnDestroy()
    {
        removeEventListener(MatrixEvent.EventsWithMatrix.INITIALIZATION.ToString());
        removeEventListener(PieceIvents.EventsWithPiece.DESTROY.ToString());
        removeEventListener(MatrixEvent.EventsWithMatrix.ANSWER_UPDATE.ToString());
        removeEventListener(EventsWithPieces.PiecesEvents.ANSWER_TO_SWAP.ToString());
    }
}
