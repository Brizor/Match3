using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsWithPieces : BaseEvent
{
    public int x1, x2, y1, y2;

    public enum PiecesEvents
    {
        REQUEST_TO_SWAP,
        ANSWER_TO_SWAP
    }

    public EventsWithPieces(string type, string owner, bool isGlobal,int x1,int y1, int x2, int y2) : base(type, owner, isGlobal)
    {
        this.x1 = x1;
        this.x2 = x2;
        this.y1 = y1;
        this.y2 = y2;
        this.type = type;
    }
}
