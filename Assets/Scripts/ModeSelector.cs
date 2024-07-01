using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class ModeSelector
{
    [SerializeField] static int numberOfPlayers;
    [SerializeField] static PlayerInput playerInput;
    static string[] playerStrings;

    public static PlayerInput SpawnPlayers()
    {
        var instance = PlayerInput.Instantiate(playerInput.gameObject, controlScheme: "Keyboard&Mouse", pairWithDevices: new InputDevice[] { Keyboard.current, Mouse.current });

        instance.transform.parent = playerInput.transform.parent;
        instance.transform.position = playerInput.transform.position;

        Object.Destroy(playerInput.gameObject);
        return instance;
    }

    public static string ReturnControl(int playerNumber)
    {
        return playerStrings[playerNumber];
    }
}
