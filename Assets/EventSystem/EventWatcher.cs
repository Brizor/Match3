using System;
using System.Collections.Generic;

public class EventWatcher
{
    public string type;
    private Dictionary<string, Action<BaseEvent>> watchers = new Dictionary<string, Action<BaseEvent>>();

    public EventWatcher(string type)
    {
        this.type = type;
    }

    public void addCallBack(string ownerName, Action<BaseEvent> callback)
    {
        if (!watchers.ContainsKey(ownerName))
        {
            watchers.Add(ownerName, callback);
        }
    }

    public void removeCallBack(string ownerName)
    {
        if (watchers.ContainsKey(ownerName))
        {
            watchers.Remove(ownerName);
        }
    }

    public void dispatch(BaseEvent eventValue)
    {
        foreach (var item in watchers)
        {
            (item.Value as Action<BaseEvent>)(eventValue);
        }

    }
}