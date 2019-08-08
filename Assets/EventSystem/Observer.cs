using System;
using System.Collections.Generic;

public class Observer
{
    private Dictionary<string, EventWatcher> globalWatchers = new Dictionary<string, EventWatcher>(); // (type, watcher)
    private Dictionary<string, List<EventWatcher>> localWatchers = new Dictionary<string, List<EventWatcher>>(); // (name, watchers)

    public Observer()
    {
    }

    public void addWatcherToEvent(string eventType, Action<BaseEvent> callback, string ownerName, string followingItem = default(string))
    {
        EventWatcher watcher = getWatcher(eventType, true, followingItem);
        watcher.addCallBack(ownerName, callback);
    }

    public EventWatcher getWatcher(string eventType, bool addIfNot = false, string followingItem = default(string))
    {
        EventWatcher watcher = null;

        if (followingItem == default(string))
        {
            if (globalWatchers.ContainsKey(eventType))
            {
                globalWatchers.TryGetValue(eventType, out watcher);
            }
            else if (addIfNot)
            {
                watcher = new EventWatcher(eventType);
                globalWatchers.Add(eventType, watcher);
            }
        }
        else
        {
            List<EventWatcher> watchers = null;

            if (localWatchers.ContainsKey(followingItem))
            {
                localWatchers.TryGetValue(followingItem, out watchers);
            }
            else if (addIfNot)
            {
                watchers = new List<EventWatcher>();
                localWatchers.Add(followingItem, watchers);
            }

            if (watchers != null)
            {
                foreach (EventWatcher item in watchers)
                {
                    if (item.type == eventType)
                    {
                        watcher = item;
                    }
                }
            }

            if (addIfNot && watcher == null)
            {
                watcher = new EventWatcher(eventType);
                watchers.Add(watcher);
            }
        }

        return watcher;
    }

    public void removeWatcherToEvent(string eventType, string ownerName, string followingItem = default(string))
    {
        EventWatcher watcher = getWatcher(eventType, false, followingItem);
        watcher.removeCallBack(ownerName);
    }

    public void dispatchEvent(BaseEvent eventValue)
    {
        string sendedOwnerName = eventValue.owner;
        if (eventValue.isGloabal)
        {
            sendedOwnerName = default(string);
        }

        EventWatcher watcher = getWatcher(eventValue.type, false, sendedOwnerName);
        if (watcher != null)
        {
            watcher.dispatch(eventValue);
        }
    }
}
