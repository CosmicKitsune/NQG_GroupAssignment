using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunStart : MonoBehaviour
{
    public string levelname;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Ball") {
            int index = Random.Range(1, 4);
            SceneManager.LoadSceneAsync(index);
        }
    }
}
