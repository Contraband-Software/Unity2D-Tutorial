using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public UnityEvent coinCollect;

    private void Start()
    {
        coinCollect.AddListener(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().IncrementScore);
        coinCollect.AddListener(GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().UpdateScore);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            coinCollect.Invoke();
            Destroy(gameObject);
        }
    }

    public void TestMethod()
    {
        print("FIRE COIN COLLECT");
    }
}
