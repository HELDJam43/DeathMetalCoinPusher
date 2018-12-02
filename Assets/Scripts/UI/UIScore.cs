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

    public void OnScoreUpdated(int addedScore, int totalScore)
    {
        // todo - flash the added points

        _currentScore = totalScore;
        _text.text = ("SACRIFICES: " + _currentScore);
    }

    // Use this for initialization
    void Start () 
    {
        GameManager.Instance.OnPointsAdded.AddListener(OnScoreUpdated);
        OnScoreUpdated(0, 0);
	}
}
