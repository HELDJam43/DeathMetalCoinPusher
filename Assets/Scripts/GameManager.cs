using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

// First parameter is the number of points added, second is total points
public class PointsAddedEvent : UnityEvent<int, int>
{

}

public class GameStateEvent : UnityEvent<int>
{

}

public class GameManager : MonoBehaviour 
{
    public enum GameState
    {
        Title,
        Play,
        Ending
    }

    public static GameManager Instance { get; private set; }

    public static GameState CurrentGameState = GameState.Title;

    public PointsAddedEvent OnSacrificeAdded;
    public PointsAddedEvent OnPointsAdded;
    public PointsAddedEvent OnBonusPointsAdded;

    public GameStateEvent OnGameStartingEvent;
    public GameStateEvent OnGameOverEvent;

    public int CurrentSacrifices
    {
        get;
        private set;
    }

    public int CurrentScore
    {
        get;
        private set;
    }

    public int CurrentBonusModeScore
    {
        get;
        private set;
    }

    public bool IsBonusMode
    {
        get;
        private set;
    }

    private const int BONUS_MODE_TARGET = 666;
    private const int BONUS_MODE_MULTIPLIER = 2;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Two GameManagers in the scene!");
        }

        // Init Events
        if (OnPointsAdded == null)
        {
            OnSacrificeAdded = new PointsAddedEvent();
            OnPointsAdded = new PointsAddedEvent();
            OnBonusPointsAdded = new PointsAddedEvent();

            OnGameStartingEvent = new GameStateEvent();
            OnGameOverEvent = new GameStateEvent();
        }

        // Init Scores
        CurrentSacrifices = 0;
        CurrentScore = 0;
        CurrentBonusModeScore = 0;
        IsBonusMode = false;
    }

    private void Start()
    {
        FindObjectOfType<UIGameTimer>().StartCountdownTimer();
    }

    private void OnDestroy()
    {
        // Clear current instance for game reset
        Instance = null;
    }

    public void AddPoints(int points)
    {
        CurrentSacrifices += 1;
        CurrentScore += !IsBonusMode ? points : points * BONUS_MODE_MULTIPLIER;

        if (!IsBonusMode)
        {
            CurrentBonusModeScore += points;
            CheckForBonusMode();
            OnBonusPointsAdded.Invoke(points, CurrentBonusModeScore);
        }

        OnSacrificeAdded.Invoke(1, CurrentSacrifices);
        OnPointsAdded.Invoke(points, CurrentScore);
    }

    private void CheckForBonusMode()
    {
        if (CurrentBonusModeScore >= BONUS_MODE_TARGET)
        {
            // Don't allow going over the target
            CurrentBonusModeScore = BONUS_MODE_TARGET;

            // Start Bonus Mode
            StartCoroutine(DoBonusMode());
        }
    }

    public IEnumerator DoBonusMode()
    {
        IsBonusMode = true;
        MusicManager musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        musicManager.PlayFast();

        // Do bonus mode for as long as it takes to finish two rounds of the fast song
        const float lengthOfSong = 9.14f;
        const int numberOfRounds = 1;
        yield return new WaitForSeconds(lengthOfSong * numberOfRounds);

        // Finish bonus mode
        IsBonusMode = false;
        CurrentBonusModeScore = 0;
        musicManager.PlayMain();
    }

    public void InvokeGameOver()
    {
        OnGameOverEvent.Invoke(CurrentScore);
    }
}
