using UnityEngine;
using UnityEngine.Events;

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

    public int CurrentScore
    {
        get;
        private set;
    }

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
        FindObjectOfType<UIGameTimer>().StartCountdownTimer();
    }

    public void AddPoints(int points)
    {
        CurrentScore += points;
        OnPointsAdded.Invoke(points);
    }
}
