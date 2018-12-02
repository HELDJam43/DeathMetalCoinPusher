using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScore : MonoBehaviour 
{
    private int _currentScore;

    private TextMeshProUGUI _text;

    public void OnScoreUpdated(int currentScore)
    {
        _currentScore = currentScore;
        _text.text = ("SCORE: " + _currentScore);
    }

    // Use this for initialization
    void Start () 
    {
        OnScoreUpdated(0);
	}
}
