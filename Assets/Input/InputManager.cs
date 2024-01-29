using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("No input manager instance found.");
            }

            return _instance;
        }
    }

    public GameInputs input { get; private set; }

    private void Awake()
    {
        _instance = this;
        input = new GameInputs();
        input.Player.Enable();
    }

    public void DroneControl(bool active)
    {
        if (active)
        {
            input.Player.Disable();
            input.Drone.Enable();
        }
        else
        {
            input.Player.Enable();
            input.Drone.Disable();
        }
    }

    public void ForkliftControl(bool active)
    {
        if (active)
        {
            input.Player.Disable();
            input.Forklift.Enable();
        }
        else
        {
            input.Player.Enable();
            input.Forklift.Disable();
        }
    }
}
