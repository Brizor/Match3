
public class MatrixEvent : BaseEvent
{
    public HELPER.ITEMS[,] matrix;

    public enum EventsWithMatrix
    {
        INITIALIZATION,
        UPDATE,
        UPDATE_ANSWER,
        UPDATE_REQUEST,
        REACTION_REQUEST,
        REACTION_ANSWER
    }

    public MatrixEvent(string type, string owner, bool isGlobal, HELPER.ITEMS[,] matrix) : base(type, owner, isGlobal)
    {
        this.matrix = matrix;
        this.type = type;
    }
}
