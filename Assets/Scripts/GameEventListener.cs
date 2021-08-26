using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] GameEvent gameEvent;
    [SerializeField] UnityEvent unityEvent;


    private void Awake()
    {
        gameEvent.Register(this);
    }

    private void OnDestroy()
    {
        gameEvent.Deregister(this);
    }

    public void RaiseEvent()
    {
        unityEvent.Invoke();
    }
}
