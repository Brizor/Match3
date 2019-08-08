using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Proxy : MonoBehaviourEx
{
    public Server server;

    void Awake()
    {
        addEventListener(MatrixEvent.EventsWithMatrix.UPDATE_REQUEST.ToString(), getMatrixFromClient);
        addEventListener(EventsWithPieces.PiecesEvents.SWAP_REQUEST.ToString(), swapPieces);
        addEventListener(MatrixEvent.EventsWithMatrix.REACTION_REQUEST.ToString(), reaction_request);
    }

    private void getMatrixFromClient(BaseEvent cutchEvent)
    {
        print("getMatrixFromClient");
        MatrixEvent custEvent = cutchEvent as MatrixEvent;
        server.updateMatrix(custEvent.matrix);
    }

    private void swapPieces(BaseEvent cutchEvent)
    {
        EventsWithPieces custEvent = cutchEvent as EventsWithPieces;
        server.swap2Pieces(custEvent.x1, custEvent.y1, custEvent.x2, custEvent.y2);
    }

    public void reaction_answer(HELPER.ITEMS[,] itemsNeedToDestroy)
    {
        dispatchEvent(new MatrixEvent(MatrixEvent.EventsWithMatrix.REACTION_ANSWER.ToString(), this.name, true, itemsNeedToDestroy));
    }

    private void reaction_request(BaseEvent cutchEvent)
    {
        print("getMatrixFromClient");
        MatrixEvent custEvent = cutchEvent as MatrixEvent;
        server.checkAllPieces();
    }

    public void sentAnswerToSwap(int x1, int y1, int x2, int y2)
    {
        dispatchEvent(new EventsWithPieces(EventsWithPieces.PiecesEvents.SWAP_ANSWER.ToString(), this.name, true, x1, y1, x2, y2));
    }

    public void sendStartMatrixToClient(HELPER.ITEMS [,] matrix)
    {
        dispatchEvent(new MatrixEvent(MatrixEvent.EventsWithMatrix.INITIALIZATION.ToString(), this.name, true, matrix));
    }

    public void sendMatrixToClient(HELPER.ITEMS[,] matrix)
    {
        dispatchEvent(new MatrixEvent(MatrixEvent.EventsWithMatrix.UPDATE_ANSWER.ToString(), this.name, true, matrix));
    }

    public void OnDestroy()
    {
        removeEventListener(MatrixEvent.EventsWithMatrix.UPDATE_REQUEST.ToString());
        removeEventListener(EventsWithPieces.PiecesEvents.SWAP_ANSWER.ToString());
    }

}
