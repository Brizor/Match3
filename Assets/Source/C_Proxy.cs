using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Proxy : MonoBehaviourEx
{
    public DinamicGrid client;

    void Awake()
    {
        addEventListener(MatrixEvent.EventsWithMatrix.INITIALIZATION.ToString(), getStartMatrixFromServer);
        addEventListener(MatrixEvent.EventsWithMatrix.UPDATE_ANSWER.ToString(), update_answer);
        addEventListener(MatrixEvent.EventsWithMatrix.REACTION_ANSWER.ToString(), reaction_answer);
        addEventListener(EventsWithPieces.PiecesEvents.SWAP_ANSWER.ToString(), swap_answer);
    }

    private void reaction_answer(BaseEvent cutchEvent)
    {
        MatrixEvent custEvent = cutchEvent as MatrixEvent;
        client.reaction(custEvent.matrix);
    }

    public void reaction_request(HELPER.ITEMS[,] matrix)
    {
        dispatchEvent(new MatrixEvent(MatrixEvent.EventsWithMatrix.REACTION_REQUEST.ToString(), this.name, true, matrix));
    }


    private void getStartMatrixFromServer(BaseEvent cutchEvent)
    {
        MatrixEvent custEvent = cutchEvent as MatrixEvent;
        client.createGrid(custEvent.matrix);
    }

    private void update_answer(BaseEvent cutchEvent)
    {
        MatrixEvent custEvent = cutchEvent as MatrixEvent;
        client.updateGrid(custEvent.matrix);
    }

    public void update_request(HELPER.ITEMS[,] matrix)
    {
        dispatchEvent(new MatrixEvent(MatrixEvent.EventsWithMatrix.UPDATE_REQUEST.ToString(), this.name, true, matrix));
    }

    private void swap_answer(BaseEvent cutchEvent)
    {
        EventsWithPieces custEvent = cutchEvent as EventsWithPieces;
        client.startSwapPieces(custEvent.x1, custEvent.y1, custEvent.x2, custEvent.y2);
    }

    public void swap_request(int x1, int y1, int x2, int y2)
    {
        dispatchEvent(new EventsWithPieces(EventsWithPieces.PiecesEvents.SWAP_REQUEST.ToString(), this.name, true, x1, y1, x2, y2));
    }

    public void OnDestroy()
    {
        removeEventListener(MatrixEvent.EventsWithMatrix.INITIALIZATION.ToString());
        removeEventListener(PieceIvents.EventsWithPiece.DESTROY.ToString());
        removeEventListener(MatrixEvent.EventsWithMatrix.UPDATE_ANSWER.ToString());
        removeEventListener(EventsWithPieces.PiecesEvents.SWAP_ANSWER.ToString());
    }
}
