using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Presenter : MonoBehaviour
{
    private void OnEnable()
    {
        GameEventsHandler.ExampleEvent += OnValueChanged;
    }

    private void OnDisable()
    {
        GameEventsHandler.ExampleEvent -= OnValueChanged;
    }

    private void OnValueChanged()
    {
        //changes the view which user see
    }
}
