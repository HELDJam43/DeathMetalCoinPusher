using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDiveControls : MonoBehaviour 
{
    [SerializeField]
    private float TimeBetweenStageDivers;

    // the amount of time before a new stage diver is spaw
    private float diveTimer;

	// Use this for initialization
	void Start () 
    {
        ResetDiveTimer();
	}
	
	// Update is called once per frame
	void Update () 
    {
        UpdateDiveTimer();

        if (diveTimer < 0f)
        {
            if (WasDiveButtonPressed())
            {
                StartStageDive();
                ResetDiveTimer();
            }
        }
    }

    private bool WasDiveButtonPressed()
    {
        // TODO - if we want to add additional control schemes, do it here.
        bool wasPressed = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            wasPressed = true;
        }
        return wasPressed;
    }

    private void ResetDiveTimer()
    {
        diveTimer = TimeBetweenStageDivers;
    }

    private void UpdateDiveTimer()
    {
        diveTimer -= Time.deltaTime;
    }

    private void StartStageDive()
    {
        // TODO - do things
    }
}
