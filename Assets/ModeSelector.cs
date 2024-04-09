using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModeSelector : MonoBehaviour
{
    [SerializeField] int numberOfPlayers;
    [SerializeField] PlayerInput playerInput;

    public PlayerInput SpawnPlayers()
    {
        var instance = PlayerInput.Instantiate(playerInput.gameObject, controlScheme: "Keyboard&Mouse", pairWithDevices: new InputDevice[] { Keyboard.current, Mouse.current });

        instance.transform.parent = playerInput.transform.parent;
        instance.transform.position = playerInput.transform.position;

        Object.Destroy(playerInput.gameObject);
        return instance;
    }
}
