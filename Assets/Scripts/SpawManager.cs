using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawManager : MonoBehaviour
{
    Player[] players;
    [SerializeField] Transform[] positionTransforms;
    void Start()
    {
        players = FindObjectsOfType<Player>();
        for (int i = 0; i<players.Length; i++)
        {
            players[i].transform.position = positionTransforms[i].position;
        }
    }

}
