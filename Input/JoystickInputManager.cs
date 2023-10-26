using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickInputManager : MonoBehaviour
{
    [SerializeField] Joystick joystick;

    private bool inputAvailable;

    private void Awake()
    {
        inputAvailable = true;
    }

    public bool IsTouching()
    {
        if (inputAvailable)
        {
            return joystick.isTouching;
        }
        else
        {
            return false;
        }
    }

    public bool InputAvailable()
    {
        return inputAvailable;
    }

    public Vector2 GetInput()
    {
        if (inputAvailable)
        {
            return joystick.Direction;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public void SetInputAvailabe(bool available)
    {
        inputAvailable = available;
    }
}
