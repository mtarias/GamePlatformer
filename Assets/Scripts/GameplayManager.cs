using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    private GameObject[] enemies;

    private bool levelUp;

    public string loadScene;

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length < 1 && !levelUp)
        {
            levelUp = true;
            print("Level Passed");
            SceneManager.LoadScene(loadScene);
        }
    }
}
