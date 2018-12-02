using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PointsAddedEvent : UnityEvent<int>
{

}

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance { get; private set; }

    public PointsAddedEvent OnPointsAdded;

    public enum GameState
    {
        Title,
        Play,
        Ending
    }

    private bool _isFastMode = false;

    public int CurrentScore
    {
        get;
        private set;
    }

    [SerializeField]
    private int _bonusModeScore;
    private int _nextBonusScore;

    public static GameState CurrentGameState = GameState.Title;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Two GameManagers in the scene!");
        }

        if (OnPointsAdded == null)
        {
            OnPointsAdded = new PointsAddedEvent();
        }

        CurrentScore = 0;
    }

    private void Start()
    {
        _nextBonusScore = _bonusModeScore;
        FindObjectOfType<UIGameTimer>().StartCountdownTimer();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void AddPoints(int points)
    {
        CurrentScore += points;

        if (CurrentScore >= _nextBonusScore)
        {
            StartCoroutine("DoFastMode");
        }
        OnPointsAdded.Invoke(points);
    }

    public IEnumerator DoFastMode()
    {
        if (!_isFastMode)
        {
            _nextBonusScore += _bonusModeScore;
            MusicManager manager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
            manager.PlayFast();
            yield return new WaitForSeconds(9.14f * 2f);
            manager.PlayMain();
        }
    }
}
