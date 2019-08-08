

public class PieceIvents : BaseEvent
{
    public int x;
    public int y;
    public enum EventsWithPiece
    {
        DESTROY
    }

    public PieceIvents(string type, string owner, bool isGlobal, int x,int y) : base(type, owner, isGlobal)
    {
        this.type = type;
        this.x = x;
        this.y = y;
    }
}
