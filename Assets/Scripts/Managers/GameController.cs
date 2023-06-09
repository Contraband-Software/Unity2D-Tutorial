using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public int score { get; private set; }
    public UnityEvent coinCollect;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            coinCollect.AddListener(IncrementScore);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void IncrementScore()
    {
        score++;
    }

}
