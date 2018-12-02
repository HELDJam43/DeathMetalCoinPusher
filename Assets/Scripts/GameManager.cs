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
        _currentPoints += points;
        OnPointsAdded.Invoke(points);
    }

    private static GameManager _instance;
    private int _currentPoints;

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

        _currentPoints = 0;
    }

    // Use this for initialization
    void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
