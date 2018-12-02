using System.Collections;
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
        _diveTimer = 0f;
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
        StartCoroutine(SetMass());
        ResetDiveTimer();
    }

    private Vector3 _swingerLaunchFromPosition = Vector3.zero;

    private Vector3 _swingerLaunchToPosition = Vector3.zero;
    private Vector3 _swingerLaunchToPosition2 = Vector3.zero;
    private Vector3 zGravity = new Vector3(0, 0, -9.8f);

    private void StartSwingerStageDive()
    {
        _activeStageDiver2 = _activeStageDiver;
        _activeStageDiver = null;

        deltaTime = 0.0f;

        _swingerLaunchFromPosition = _activeStageDiver2.transform.localPosition;

        float r = Random.Range(10.0f, 20.0f);
        _swingerLaunchToPosition = _activeStageDiver2.transform.localPosition + (Vector3.forward * r);
        _swingerLaunchToPosition2 = _activeStageDiver2.transform.localPosition + (Vector3.down * r);

        magnitude = (new Vector3(_swingerLaunchToPosition2.x, _swingerLaunchToPosition2.y, 0f) - _activeStageDiver2.transform.localPosition).magnitude;
    }
  
    private float deltaTime = 0.0f;
    private float magnitude = 0.0f;
    private void FixedUpdate()
    {
        if(_activeStageDiver2 != null && _activeStageDiver2.GetComponent<Patron>().patronType == Patron.PatronType.Swinger)
        {
            if (Vector3.Distance(_activeStageDiver2.transform.localPosition, _swingerLaunchToPosition2) > 0.1f)
            {
                _activeStageDiver2.transform.localPosition = Vector3.MoveTowards(_activeStageDiver2.transform.localPosition, _swingerLaunchToPosition2, magnitude * Time.deltaTime);

                deltaTime += Time.deltaTime;
                Vector3 p = SampleParabolaDerivative2D(_swingerLaunchFromPosition, _swingerLaunchToPosition, 2, 2, deltaTime);

                float scale = 4 + (3 * Mathf.Clamp(p.y, -0.5f, 1f));
                _activeStageDiver2.transform.localScale = new Vector3(scale, scale, 1.0f);
            }
            else
            {
   
                StartCoroutine(AddExplosionLanding());
            }
        }
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

    private IEnumerator AddExplosionLanding()
    {
        foreach (GameObject patron in CrowdSpawner.Patrons)
        {
            if(_activeStageDiver2.gameObject != patron)
            {
                if (Vector3.Distance(_activeStageDiver2.transform.localPosition, patron.transform.localPosition) <= 10)
                {
                    patron.GetComponent<Rigidbody2D>().AddExplosionForce(20, _activeStageDiver2.transform.localPosition, 20);
                }
            }
        }
        yield return new WaitForEndOfFrame();
        _activeStageDiver2.transform.localScale = new Vector3(4, 4, 1.0f);
        _activeStageDiver2.GetComponent<PatronAnimatorController>().Idle();
        _activeStageDiver2.simulated = true;
        _activeStageDiver2.mass = 1.0f;
        _activeStageDiver2.GetComponent<PatronDrawOrder>().SetCrowdLayer();
        _activeStageDiver2 = null;
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
            _activeStageDiver2.GetComponent<PatronDrawOrder>().SetCrowdLayer();
            _activeStageDiver2 = null;
        }
    }


}
