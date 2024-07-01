using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  /*  void Start()
    {
        int numberOfInstances = FindObjectsOfType<GameManager>().Length;
        if (numberOfInstances > 1)
        {
            Destroy(gameObject);
        }
        else DontDestroyOnLoad(gameObject);
    }*/

    public void StartGame(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void HideCanvas()
    {
        FindObjectOfType<Canvas>().enabled = false;
    }
}
