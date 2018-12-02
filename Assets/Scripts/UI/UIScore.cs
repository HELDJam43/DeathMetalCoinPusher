using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour 
{
    [SerializeField]
    private bool _isSacrificeUI;

    [SerializeField]
    private TextMeshProUGUI _text;

    private int _currentValue;
    private string _prefix;

    private const string SACRIFICE_TEXT = "SACRIFICES: ";
    private const string SCORE_TEXT = "SCORE: ";

    public void OnScoreUpdated(int addedScore, int totalScore)
    {
        _currentValue = totalScore;
        _text.text = (_prefix + _currentValue);
    }

    private void Start () 
    {
        if (_isSacrificeUI)
        {
            _prefix = SACRIFICE_TEXT;
            GameManager.Instance.OnSacrificeAdded.AddListener(OnScoreUpdated);
        }
        else
        {
            _prefix = SCORE_TEXT;
            GameManager.Instance.OnPointsAdded.AddListener(OnScoreUpdated);
        }
        OnScoreUpdated(0, 0);
    }
}
