using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    private float currentValue;

    public float CurrentValue
    {
        get => currentValue;
        set
        {
            currentValue = value;
            OnValueChanged();
        }
    }

    private void OnValueChanged()
    {
        GameEventsHandler.ExampleEventHandler();
        //sends event to presenter
    } 
}
