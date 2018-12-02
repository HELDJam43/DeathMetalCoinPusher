﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDiveControls : MonoBehaviour 
{
    [SerializeField]
    private float TimeBetweenStageDivers;

    [SerializeField]
    private float StageMovementSpeed;

    [SerializeField]
    private float StageDiveForce;

    [SerializeField]
    private PatronSpawner _spawner;

    [SerializeField]
    private Transform LeftStageBoundary;

    [SerializeField]
    private Transform RightStageBoundary;

    private float _diveTimer;
    private Rigidbody2D _activeStageDiver;
    private float _stageDiverMovementDirection = -1;

    private bool _isActiveStageDiverAvailable
    {
        get
        {
            return _activeStageDiver != null;
        }
    }

    private void Start () 
    {
        ResetDiveTimer();
	}
	
	private void Update () 
    {
        if (GameManager.CurrentGameState != GameManager.GameState.Play) return;

        UpdateDiveTimer();

        if (_isActiveStageDiverAvailable)
        {
            MoveStageDiver();
            if (WasDiveButtonPressed())
            {
                StartStageDive();
                ResetDiveTimer();
            }
        }
        else
        {
            if (_diveTimer <= 0f)
            {
                SpawnStageDiver();
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
        else if (Input.GetMouseButtonDown(0))
        {
            wasPressed = true;
        }
        return wasPressed;
    }

    private void ResetDiveTimer()
    {
        _diveTimer = TimeBetweenStageDivers;
    }

    private void UpdateDiveTimer()
    {
        if (_diveTimer > 0)
        {
            _diveTimer -= Time.deltaTime;
        }
    }

    private void MoveStageDiver()
    {
        Vector3 newPos = _activeStageDiver.transform.position;
        newPos.x = newPos.x + ((StageMovementSpeed * Time.deltaTime) * _stageDiverMovementDirection);

        if (_stageDiverMovementDirection < 0 && newPos.x <= LeftStageBoundary.position.x)
        {
            newPos.x = LeftStageBoundary.position.x;
            _stageDiverMovementDirection *= -1f;
        }
        else if (_stageDiverMovementDirection > 0 && newPos.x >= RightStageBoundary.position.x)
        {
            newPos.x = RightStageBoundary.position.x;
            _stageDiverMovementDirection *= -1f;
        }

        _activeStageDiver.transform.position = newPos;
    }

    private void SpawnStageDiver()
    {
        // Find a random spot in between the two boundaries
        float randX = Random.Range(LeftStageBoundary.position.x + 1f, RightStageBoundary.position.x - 1f);
        Vector3 stageDiverPos = new Vector3(randX, LeftStageBoundary.position.y);

        GameObject temp = _spawner.GetSpawnedPatron();
        CrowdSpawner.AddPatron(temp);

        _activeStageDiver = temp.GetComponent<Rigidbody2D>();
        _activeStageDiver.simulated = false;
        _activeStageDiver.mass = 5.0f;
        _activeStageDiver.transform.position = stageDiverPos;
        _activeStageDiver.GetComponent<PatronAnimatorController>().OnStage();
    }

    private void StartStageDive()
    {
        // Send the stage dive
        Patron patron = _activeStageDiver.GetComponent<Patron>();

        switch (patron.patronType)
        {
            case Patron.PatronType.Banger:
                StartBangerStageDive();
                break;
            case Patron.PatronType.Swinger:
                break;
        }

        StartCoroutine(SetMass());
        ResetDiveTimer();
    }

    private void StartBangerStageDive()
    {
        _activeStageDiver.simulated = true;
        _activeStageDiver.AddForce(new Vector2(0f, -StageDiveForce), ForceMode2D.Impulse);
        _activeStageDiver.GetComponent<PatronAnimatorController>().StageDive();
    }

    private void StartSwingStageDive()
    {
        Vector3 position = transform.localPosition + (Vector3.down * 5);
        StartCoroutine(JumpHeight(position));
    }

    private IEnumerator JumpHeight(Vector2 dropPos)
    {
        Rigidbody2D rigidbody2D = _activeStageDiver.GetComponent<Rigidbody2D>();
        rigidbody2D.simulated = false;

        Debug.Log(Vector3.Distance(_activeStageDiver.transform.localPosition, dropPos));

        while(Vector3.Distance(_activeStageDiver.transform.localPosition, dropPos) > 1)
        {
            float scale = 4 + Mathf.PingPong(Time.time * 5, 4);

            _activeStageDiver.transform.localScale = new Vector3(scale, scale);

            float distToDropPos = (new Vector3(dropPos.x, dropPos.y, 0f) - transform.position).magnitude;

            transform.position = Vector3.MoveTowards(transform.position, dropPos, distToDropPos * Time.deltaTime);
            yield return null;
        }
    }

    private Rigidbody2D _activeStageDiver2;
    private IEnumerator SetMass()
    {
        _activeStageDiver2 = _activeStageDiver;
        _activeStageDiver = null;

        yield return new WaitForSeconds(2.0f);
        if(_activeStageDiver2 != null)
        {
            _activeStageDiver2.mass = 0.5f;
            _activeStageDiver2.GetComponent<PatronAnimatorController>().Idle();
        }
    }
}
