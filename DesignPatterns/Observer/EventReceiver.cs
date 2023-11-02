using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReceiver : MonoBehaviour
{
    private void OnEnable()
    {
        GameEventsHandler.ExampleEvent += EmptyFunction;
    }

    private void OnDisable()
    {
        GameEventsHandler.ExampleEvent -= EmptyFunction;
    }

    public void EmptyFunction()
    {
        Debug.Log("Process Completed");
    }
}
