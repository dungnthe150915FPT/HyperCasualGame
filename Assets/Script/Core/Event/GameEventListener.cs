using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CustomGameEvent : UnityEvent<Component, object>
{

}
public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;

    //public delegate void OnEventRaised();
    //public OnEventRaised onEventRaised;

    //public void OnEventRaisedMethod()
    //{
    //    onEventRaised?.Invoke();
    //}

    public CustomGameEvent response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(Component sender, object data)
    {
        response.Invoke(sender, data);
    }
}
