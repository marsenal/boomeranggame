using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlAssigner : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] Player playerPrefab;
    [SerializeField] Transform spawnPoint;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void SpawnPlayer()
    {
        Instantiate(playerPrefab.gameObject, spawnPoint.position, Quaternion.identity);
    }
}
