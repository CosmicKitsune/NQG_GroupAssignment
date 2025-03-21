using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public void LoadLevel (string levelName) {
        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single); //loads selected scene
    }

    public void ReloadLevel() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex); //reloads current scene
    }

    public void RandomLevel() {
        int index = Random.Range(1, 4);
        SceneManager.LoadSceneAsync(index);
    }
}
