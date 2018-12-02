using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScore : MonoBehaviour 
{
    private int _currentScore;

    [SerializeField]
    private TextMeshProUGUI _text;

    public void OnScoreUpdated(int addedScore)
    {
        // todo - flash the added points

        _currentScore = GameManager.Instance.CurrentScore;
        _text.text = ("SCORE: " + _currentScore);
    }

    // Use this for initialization
    void Start () 
    {
        GameManager.Instance.OnPointsAdded.AddListener(OnScoreUpdated);
        OnScoreUpdated(0);
	}
}
