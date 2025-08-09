using System;
using System.Collections.Generic;

public static class EVMLight
{
    private static Dictionary<GameEvent, Delegate> eventTable = new ();

    public static void Subscribe<T>(GameEvent eventType, Action<T> listener)
    {
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = Delegate.Combine(eventTable[eventType], listener);
        }
        else
        {
            eventTable[eventType] = listener;
        }
    }

    public static void Unsubscribe<T>(GameEvent eventType, Action<T> listener)
    {
        if (eventTable.ContainsKey(eventType))
        {
            var currentDel = Delegate.Remove(eventTable[eventType], listener);

            if (currentDel == null)
                eventTable.Remove(eventType);
            else
                eventTable[eventType] = currentDel;
        }
    }

    public static void Trigger<T>(GameEvent eventType, T param)
    {
        if (eventTable.ContainsKey(eventType))
        {
            (eventTable[eventType] as Action<T>)?.Invoke(param);
        }
    }
}