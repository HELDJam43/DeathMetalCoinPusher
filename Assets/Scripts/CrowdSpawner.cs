﻿using System.Collections.Generic;
using UnityEngine;

public class CrowdSpawner : MonoBehaviour
{
    public GameObject Patron;
    public static int MaxCrowdSize = 20;

    private static List<GameObject> Patrons = new List<GameObject>();

    private void Start()
    {
        SpawnCrowd();
    }

    private void SpawnCrowd()
    {
        for(int i = 0; i < MaxCrowdSize; i++)
        {
            GameObject patron = RandomSpawnPatron();
           
            Patrons.Add(patron);
        }
    }

    private GameObject RandomSpawnPatron()
    {
        float x = Random.Range(-8.0f, 8.0f);
        float y = Random.Range(-8.0f, 8.0f);

        Vector3 position = transform.position + new Vector3(x, y, 0);
        
        return Instantiate(Patron, position, Quaternion.identity);
    }

    public static void AddPatron(GameObject patron)
    {
        Patrons.Add(patron);
    }

    public static void RemovePatron(GameObject patron)
    {
        Patrons.Remove(patron);
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

    private void MouseSpawnPatron()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0.0f;

            GameObject patron = Instantiate(Patron, position, Quaternion.identity);
            Patrons.Add(patron);

            Rigidbody2D rigidbody2D = patron.GetComponent<Rigidbody2D>();

            patron.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1000.0f));
        }
    }

    private void UpdateDrawPositions()
    {
        List<GameObject> copy = new List<GameObject>();
        foreach (GameObject patron in Patrons)
            copy.Add(patron);
        copy.Sort(CompareDrawPositions);
        for (int i = 0; i < copy.Count; i++)
        {
            copy[i].GetComponent<PatronDrawOrder>().SetIndex(i);
        }
    }

    private int CompareDrawPositions(GameObject x, GameObject y)
    {
        if (x == null)
        {
            if (y == null)
            {
                return 0;
            }
            else
            {
                return 1;
            }

        }
        else
        {
            if (y == null)
            {
                return -1;
            }
            else
            {
                bool xGreater = x.transform.position.y > y.transform.position.y;

                if (xGreater)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }
    }
}
