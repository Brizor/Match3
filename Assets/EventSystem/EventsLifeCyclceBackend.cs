using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsLifeCyclceBackend
{

    private List<BaseEvent> eventsPool = new List<BaseEvent>();
    private List<BaseEvent> eventsPoolBoofer = new List<BaseEvent>();

    private List<UnsobscribeStruct> unsobscribePool = new List<UnsobscribeStruct>();
    private List<UnsobscribeStruct> unsobscribePoolBoofer = new List<UnsobscribeStruct>();

    private List<SobscribeStruct> sobscribePool = new List<SobscribeStruct>();
    private List<SobscribeStruct> sobscribePoolBoofer = new List<SobscribeStruct>();

    private static EventsLifeCyclceBackend _instance;
    public static EventsLifeCyclceBackend instance
    {
        get
        {
            if (default(EventsLifeCyclceBackend) == _instance)
            {
                _instance = new EventsLifeCyclceBackend();
            }

            return _instance;
        }
    }

    private Observer _observer;
    private bool _publishInProgress = false;

    public EventsLifeCyclceBackend()
    {
        _observer = new Observer();
    }

    struct UnsobscribeStruct
    {
        public string name;
        public string eventType;
        public string followingItemName;

        public UnsobscribeStruct(string name, string eventType, string followingItemName)
        {
            this.name = name;
            this.eventType = eventType;
            this.followingItemName = followingItemName;
        }
    }

    struct SobscribeStruct
    {
        public string name;
        public string eventType;
        public string followingItemName;
        public Action<BaseEvent> callback;

        public SobscribeStruct(string name, string eventType, string followingItemName, Action<BaseEvent> callback)
        {
            this.name = name;
            this.eventType = eventType;
            this.followingItemName = followingItemName;
            this.callback = callback;
        }
    }


    public void addEventListener(string eventType, string name, Action<BaseEvent> callback, string followingItemName = default(string))
    {
        if (_publishInProgress)
        {
            sobscribePoolBoofer.Add(new SobscribeStruct(name, eventType, followingItemName, callback));
        }
        else
        {
            sobscribePool.Add(new SobscribeStruct(name, eventType, followingItemName, callback));
        }
    }

    public void removeEventListener(string name, string eventType, string followingItemName = default(string))
    {
        if (_publishInProgress)
        {
            unsobscribePoolBoofer.Add(new UnsobscribeStruct(name, eventType, followingItemName));
        }
        else
        {
            unsobscribePool.Add(new UnsobscribeStruct(name, eventType, followingItemName));
        }
    }

    public void dispatchEvent(BaseEvent eventValue)
    {
        if (_publishInProgress)
        {
            eventsPoolBoofer.Add(eventValue);
        }
        else
        {
            eventsPool.Add(eventValue);
        }
    }

    public void publishBoofers()
    {
        _publishInProgress = true;

        if (unsobscribePool.Count > 0)
        {
            unsobscribe();
        }

        if (sobscribePool.Count > 0)
        {
            sobscribe();
        }

        if (eventsPool.Count > 0)
        {
            fireEvents();
        }

        movePoolToMain<BaseEvent>(ref eventsPool, ref eventsPoolBoofer);
        movePoolToMain<UnsobscribeStruct>(ref unsobscribePool, ref unsobscribePoolBoofer);
        movePoolToMain<SobscribeStruct>(ref sobscribePool, ref sobscribePoolBoofer);

        _publishInProgress = false;
    }

    private void movePoolToMain<T>(ref List<T> main, ref List<T> boofer)
    {
        List<T> temp = main;
        main = boofer;
        boofer = temp;
        boofer.Clear();
    }

    private void sobscribe()
    {
        foreach (SobscribeStruct item in sobscribePool)
        {
            _observer.addWatcherToEvent(item.eventType, item.callback, item.name, item.followingItemName);
        }

        sobscribePool.Clear();
    }

    private void unsobscribe()
    {
        foreach (UnsobscribeStruct item in unsobscribePool)
        {
            _observer.removeWatcherToEvent(item.eventType, item.name, item.followingItemName);
        }

        unsobscribePool.Clear();
    }

    private void fireEvents()
    {
        foreach (BaseEvent item in eventsPool)
        {
            _observer.dispatchEvent(item);
        }

        eventsPool.Clear();
    }
}