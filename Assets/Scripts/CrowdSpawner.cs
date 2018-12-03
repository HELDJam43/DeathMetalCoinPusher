using System.Collections.Generic;
using UnityEngine;

public class CrowdSpawner : MonoBehaviour
{
    [SerializeField]
    public PatronSpawner _spawner;

    public static int MaxCrowdSize = 20;
    public static int MaxBonusModeCrowdSize = 30;

    public static List<GameObject> Patrons = new List<GameObject>();
    public static List<GameObject> DrawOrderPatrons = new List<GameObject>();

    private BoxCollider2D spawnArea;

    private void Awake()
    {
        if (Patrons == null)
            Patrons = new List<GameObject>();

        if (DrawOrderPatrons == null)
            DrawOrderPatrons = new List<GameObject>();
    }

    private void Start()
    {
        spawnArea = GetComponent<BoxCollider2D>();
        SpawnCrowd();
    }

    private void SpawnCrowd()
    {
        while (Patrons.Count <= MaxCrowdSize)
        {
            Vector3 position = GetRandomPosition();

            if (spawnArea.OverlapPoint(position))
            {
                GameObject patron = SpawnPatron(position);
                patron.GetComponent<PatronAnimatorController>().Idle();
         
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * 2;
    }

    private GameObject SpawnPatron(Vector3 position)
    {
        GameObject patron = _spawner.GetSpawnedPatron();
        patron.transform.position = position;
        return patron;
    }

    public static void AddPatron(GameObject patron)
    {
        Patrons.Add(patron);
        DrawOrderPatrons.Add(patron);
    }

    public static void RemovePatron(GameObject patron)
    {
        Patrons.Remove(patron);
        DrawOrderPatrons.Remove(patron);
    }

    public static int PatronCount()
    {
        return Patrons.Count;
    }

    private void Update()
    {
        //MouseSpawnPatron()
        UpdateDrawPositions();
    }

    private void OnDestroy()
    {
        Patrons = null;
    }

    private void MouseSpawnPatron()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0.0f;

            GameObject patron = _spawner.GetSpawnedPatron();
            patron.transform.position = position;
            patron.GetComponent<PatronAnimatorController>().Idle();

            Rigidbody2D rigidBody2D = patron.GetComponent<Rigidbody2D>();

            rigidBody2D.AddForce(new Vector2(0, -1000.0f));
        }
    }

    // CBO - this is terribly inefficient and I'm sorry
    private static void UpdateDrawPositions()
    {
        DrawOrderPatrons.Sort(CompareDrawPositions);
        for (int i = DrawOrderPatrons.Count - 1; i >= 0; i--)
        {
            // CBO - scene restart issue
            if (DrawOrderPatrons[i] == null)
                break;

            DrawOrderPatrons[i].GetComponent<PatronDrawOrder>().SetIndex(DrawOrderPatrons.Count - 1 - i);
        }
    }

    private static int CompareDrawPositions(GameObject x, GameObject y)
    {
        if (x == null)
        {
            return y == null ? 0 : 1;
        }

        if (y == null)
        {
            return -1;
        }

        bool xGreater = x.transform.position.y > y.transform.position.y;

        return xGreater ? -1 : 1;
    }
}
