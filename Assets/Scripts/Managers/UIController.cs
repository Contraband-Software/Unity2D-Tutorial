using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("")]
    [SerializeField] TextMeshProUGUI scoreText;

    private void Start()
    {
        UpdateScore();
        GameController.Instance.coinCollect.AddListener(UpdateScore);
    }

    private void UpdateScore()
    {
        scoreText.text = GameController.Instance.score.ToString();
    }
}
