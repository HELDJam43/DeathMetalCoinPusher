using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointsAddedEvent : UnityEvent<int>
{

}

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public PointsAddedEvent OnPointsAdded;

    public void AddPoints(int points)
    {
        CurrentScore += points;
        OnPointsAdded.Invoke(points);
    }

    private static GameManager _instance;
    public int CurrentScore
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
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

    // Use this for initialization
    void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
