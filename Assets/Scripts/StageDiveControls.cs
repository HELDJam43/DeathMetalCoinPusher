﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDiveControls : MonoBehaviour
{
    [SerializeField]
    private float TimeBetweenStageDivers;

    [SerializeField]
    private float StageMovementSpeed;

    private float ModifiedStageMovementSpeed
    {
        get
        {
            return StageMovementSpeed * MusicManager.BeatsPerSecond;
        }
    }

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
        _diveTimer = 0f;
        _divingSwingers = new List<Rigidbody2D>();
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
        newPos.x = newPos.x + ((ModifiedStageMovementSpeed * Time.deltaTime) * _stageDiverMovementDirection);

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
        _activeStageDiver.GetComponent<PatronDrawOrder>().SetStageDiverLayer();
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
                StartSwingerStageDive();
                break;
        }
    }

    private void StartBangerStageDive()
    {
        _activeStageDiver.simulated = true;
        _activeStageDiver.AddForce(new Vector2(0f, -StageDiveForce), ForceMode2D.Impulse);
        _activeStageDiver.GetComponent<PatronAnimatorController>().StageDive();
        StartCoroutine(SetMass(_activeStageDiver));
        _activeStageDiver = null;
        ResetDiveTimer();
    }

    private Vector3 _swingerLaunchFromPosition = Vector3.zero;

    private Vector3 _swingerLaunchToPosition = Vector3.zero;
    private Vector3 _swingerLaunchToPosition2 = Vector3.zero;
    private Vector3 zGravity = new Vector3(0, 0, -9.8f);

    private void StartSwingerStageDive()
    {
        Rigidbody2D diver = _activeStageDiver;
        _activeStageDiver = null;

        deltaTime = 0.0f;

        _swingerLaunchFromPosition = diver.transform.localPosition;

        float r = Random.Range(10.0f, 20.0f);
        _swingerLaunchToPosition = diver.transform.localPosition + (Vector3.forward * r);
        _swingerLaunchToPosition2 = diver.transform.localPosition + (Vector3.down * r);

        magnitude = (new Vector3(_swingerLaunchToPosition2.x, _swingerLaunchToPosition2.y, 0f) - diver.transform.localPosition).magnitude;

        _divingSwingers.Add(diver);
    }

    private float deltaTime = 0.0f;
    private float magnitude = 0.0f;
    private void FixedUpdate()
    {
        List<Rigidbody2D> diversToRemove = new List<Rigidbody2D>();
        for(int i = 0; i < _divingSwingers.Count; i++)
        {
            Rigidbody2D diver = _divingSwingers[i];

            if (diver != null && diver.GetComponent<Patron>().patronType == Patron.PatronType.Swinger)
            {
                if (Vector3.Distance(diver.transform.localPosition, _swingerLaunchToPosition2) > 0.1f)
                {
                    diver.transform.localPosition = Vector3.MoveTowards(diver.transform.localPosition, _swingerLaunchToPosition2, magnitude * Time.deltaTime);

                    deltaTime += Time.deltaTime;
                    Vector3 p = SampleParabolaDerivative2D(_swingerLaunchFromPosition, _swingerLaunchToPosition, 2, 2, deltaTime);

                    float scale = 4 + (3 * Mathf.Clamp(p.y, -0.5f, 1f));
                    diver.transform.localScale = new Vector3(scale, scale, 1.0f);
                }
                else
                {

                    StartCoroutine(AddExplosionLanding(diver));
                    diversToRemove.Add(diver);
                }
            }
        }

        // Remove any divers who have started the explosion coroutine
        foreach (Rigidbody2D toRemove in diversToRemove)
        {
            _divingSwingers.Remove(toRemove);
        }
        diversToRemove = null;
    }

    public static Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
    {
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result.y += Mathf.Sin(t * Mathf.PI) * height;
            return result;
        }
        else
        {
            //start and end are not level, gets more complicated
            Vector3 travelDirection = end - start;
            Vector3 levelDirecteion = end - new Vector3(start.x, end.y, start.z);
            Vector3 right = Vector3.Cross(travelDirection, levelDirecteion);
            Vector3 up = Vector3.Cross(right, travelDirection);
            if (end.y > start.y) up = -up;
            Vector3 result = start + t * travelDirection;
            result += (Mathf.Sin(t * Mathf.PI) * height) * up.normalized;
            return result;
        }
    }

    public static Vector2 SampleParabolaDerivative2D(Vector3 start, Vector3 end, float height, float t, float delta)
    {
        if (t - delta < 0)
            return Vector2.zero;
        Vector3 v2 = SampleParabola(start, end, height, t);
        Vector3 v1 = SampleParabola(start, end, height, t - delta);
        return v2 - v1;
    }

    private IEnumerator AddExplosionLanding(Rigidbody2D stageDiver)
    {
        Rigidbody2D diver = stageDiver;
        foreach (GameObject patron in CrowdSpawner.Patrons)
        {
            if(diver.gameObject != patron)
            {
                if (Vector3.Distance(diver.transform.localPosition, patron.transform.localPosition) <= 10)
                {
                    patron.GetComponent<Rigidbody2D>().AddExplosionForce(20, diver.transform.localPosition, 20);
                }
            }
        }
        yield return new WaitForEndOfFrame();
        diver.transform.localScale = new Vector3(4, 4, 1.0f);
        diver.GetComponent<PatronAnimatorController>().Idle();
        diver.simulated = true;
        diver.mass = 1.0f;
        diver.GetComponent<PatronDrawOrder>().SetCrowdLayer();
        diver = null;
    }

    private List<Rigidbody2D> _divingSwingers;

    private const float BANGER_DIVE_TIME = 1.0f;
    private IEnumerator SetMass(Rigidbody2D stageDiver)
    {
        Rigidbody2D diver = stageDiver;

        yield return new WaitForSeconds(BANGER_DIVE_TIME);

        if(diver != null)
        {
            diver.mass = 0.5f;
            diver.GetComponent<PatronAnimatorController>().Idle();
            diver.GetComponent<PatronDrawOrder>().SetCrowdLayer();
            diver = null;
        }
    }


}
