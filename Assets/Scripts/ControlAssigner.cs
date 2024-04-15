using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlAssigner : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] Player playerPrefab;
    [SerializeField] Transform[] spawnPoint;

    

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void SpawnPlayer(int numberOfPlayers)
    {
        for (int i = 0; i < numberOfPlayers; i++) {
           Player newPlayer =  Instantiate(playerPrefab, spawnPoint[i].position, Quaternion.identity);
            newPlayer.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player"+i.ToString());
           
           Debug.Log( "Current enabled action map name = " + newPlayer.GetComponent<PlayerInput>().currentActionMap.name);
        }
    }
}
