using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class ControlAssigner : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] Player playerPrefab;
    [SerializeField] Transform[] spawnPoint;

    private string[] playerStrings;
    private static string keyboardString = "Keyboard";
    private static string gamepadString = "Gamepad";

      void Start()
      {
          int numberOfInstances = FindObjectsOfType<ControlAssigner>().Length;
          if (numberOfInstances > 1)
          {
              Destroy(gameObject);
          }
          else DontDestroyOnLoad(gameObject);
      }

    public void SpawnPlayer(int numberOfPlayers)
    {
        for (int i = 0; i < numberOfPlayers; i++) {
            Player newPlayer =  Instantiate(playerPrefab, spawnPoint[i].position, Quaternion.identity);

           //newPlayer.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player"+i.ToString());
           
           Debug.Log( "Current enabled action map name = " + newPlayer.GetComponent<PlayerInput>().currentActionMap.name);
        }
    }

    public void AssignGamepadToPlayer(int value)
    {
      // playerPrefab.GetComponent<PlayerInput>().SwitchCurrentControlScheme(InputSystem.GetDevice("Gamepad"));
       // Debug.Log(playerPrefab.GetComponent<PlayerInput>().);

       // InputUser newUser = InputUser.CreateUserWithoutPairedDevices();
       // newUser = InputUser.PerformPairingWithDevice(Gamepad.all[0], newUser, InputUserPairingOptions.None);

    }

    public void AssignKeyboardToPlayer(int value)
    {
        //Debug.Log(InputSystem.GetDevice<Keyboard>().ToString());
       // playerPrefab.GetComponent<PlayerInput>().SwitchCurrentControlScheme(InputSystem.GetDevice("Keyboard"), InputSystem.GetDevice("Mouse"));

       // InputUser newUser = InputUser.CreateUserWithoutPairedDevices();
      //  newUser = InputUser.PerformPairingWithDevice(Keyboard.current, newUser, InputUserPairingOptions.None);
       // newUser = InputUser.PerformPairingWithDevice(Mouse.current, newUser, InputUserPairingOptions.None);
        // playerPrefab.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player" + value.ToString());
    }
    public void JoinPlayer()
    {
        playerPrefab.gameObject.SetActive(true);
    }
}
