
public class MatrixEvent : BaseEvent
{
    public HELPER.ITEMS[,] matrix;

    public enum EventsWithMatrix
    {
        INITIALIZATION,
        UPDATE,
        ANSWER_UPDATE,
        REQUEST_UPADTE,
        DESTROY
    }

    public MatrixEvent(string type, string owner, bool isGlobal, HELPER.ITEMS[,] matrix) : base(type, owner, isGlobal)
    {
        this.matrix = matrix;
        this.type = type;
    }
}
