﻿using UnityEngine;

public class PatronSpawner : MonoBehaviour
{
    public enum Door
    {
        Left,
        Right
    }

    public GameObject GetSpawnedPatron()
    {
        GameObject patron = Instantiate(GetRandomPatronPrefab(), transform.position, Quaternion.identity);
        return patron;
    }

    [SerializeField]
    private GameObject[] Patrons;

    [SerializeField]
    private float[] SpawnRates;

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

    public Door door = Door.Left;
    public float force = 10;

    private void Start()
    {
        InvokeRepeating("SpawnPatron", 3, 10);
    }

    private void SpawnPatron()
    {
        if (CrowdSpawner.PatronCount() < CrowdSpawner.MaxCrowdSize)
        {
            GameObject patron = GetSpawnedPatron();
            Rigidbody2D rigidbody2D = patron.GetComponent<Rigidbody2D>();
            rigidbody2D.AddForce(door == Door.Right ? Vector3.left * force : Vector3.right * force, ForceMode2D.Impulse);
        }
    }
}
