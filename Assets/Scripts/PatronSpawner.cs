using UnityEngine;

public class PatronSpawner : MonoBehaviour
{
    public enum EDoor
    {
        Left,
        Right
    }

    public EDoor Door = EDoor.Left;

    public float Force = 10;

    public float PatronSpawnRateIntervalNormal = 3.0f;
    public float PatronSpawnRateIntervalBonusMode = 1.0f;

    private readonly Vector3 RightDoorDirection = new Vector3(-1, -1, 0);
    private readonly Vector3 LeftDoorDirection = new Vector3(1, -1, 0);


    [SerializeField]
    private GameObject[] Patrons;

    [SerializeField]
    private float[] SpawnRates;

    private void Start()
    {
        InvokeRepeating("SpawnPatronNormal", 3, PatronSpawnRateIntervalNormal);
        InvokeRepeating("SpawnPatronBonusMode", 3, PatronSpawnRateIntervalBonusMode);
    }

    public GameObject GetSpawnedPatron()
    {
        GameObject patron = Instantiate(GetRandomPatronPrefab(), transform.position, Quaternion.identity);
        CrowdSpawner.AddPatron(patron);
        return patron;
    }

    private GameObject GetRandomPatronPrefab()
    {
        int idx = 0;
        float rand = Random.Range(0f, 100f);
        float curCheck = 0f;
        for (int i = 0; i < SpawnRates.Length; i++)
        {
            if (rand < SpawnRates[i] + curCheck)
            {
                idx = i;
                break;
            }
            curCheck += SpawnRates[i];
        }
        return Patrons[idx];
    }

    private void SpawnPatronNormal()
    {
        if (CrowdSpawner.PatronCount() < CrowdSpawner.MaxCrowdSize)
        {
            SpawnPatron();
        }
    }

    private void SpawnPatronBonusMode()
    {
        if (GameManager.Instance.IsBonusMode && CrowdSpawner.PatronCount() < CrowdSpawner.MaxBonusModeCrowdSize)
        {
            SpawnPatron();
        }
    }

    private void SpawnPatron()
    {
        GameObject patron = GetSpawnedPatron();

        Rigidbody2D rigidBody2D = patron.GetComponent<Rigidbody2D>();
        rigidBody2D.AddForce(Door == EDoor.Right ? RightDoorDirection * Force : LeftDoorDirection * Force, ForceMode2D.Impulse);
    }
}
