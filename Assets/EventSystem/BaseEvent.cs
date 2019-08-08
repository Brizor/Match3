public class BaseEvent
{
    public string type;
    public string owner;
    public bool isGloabal;

    public BaseEvent(string type, string owner, bool isGlobal = false)
    {
        this.type = type;
        this.owner = owner;
        this.isGloabal = isGlobal;
    }
}