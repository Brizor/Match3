using System;
using UnityEngine;

public class MonoBehaviourEx : MonoBehaviour
{
    public void addEventListener(string eventType, Action<BaseEvent> callback, string followingItemName = default(string))
    {
        EventsLifeCyclceBackend.instance.addEventListener(eventType, name, callback, followingItemName);
    }

    public void removeEventListener(string eventType, string followingItemName = default(string))
    {
        EventsLifeCyclceBackend.instance.removeEventListener(name, eventType, followingItemName);
    }

    public void dispatchEvent(BaseEvent eventValue)
    {
        EventsLifeCyclceBackend.instance.dispatchEvent(eventValue);
    }
}