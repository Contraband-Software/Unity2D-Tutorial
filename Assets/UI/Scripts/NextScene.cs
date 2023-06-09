using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public void LoadNextScene()
    {
        print("Loading Scene 2");
        SceneManager.LoadScene(1);
    }

    public void LoadPreviousScene()
    {
        print("Loading Scene 1");
        SceneManager.LoadScene(0);
    }
}
