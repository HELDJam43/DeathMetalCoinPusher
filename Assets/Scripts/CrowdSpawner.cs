using System.Collections.Generic;
using UnityEngine;

public class CrowdSpawner : MonoBehaviour
{
    public GameObject Patron;
    private const int crowdSize = 30;

    public static List<GameObject> Patrons = new List<GameObject>();

    private void Start()
    {
        SpawnCrowd();
    }

    private void SpawnCrowd()
    {
        for(int i = 0; i < crowdSize; i++)
        {
            Patrons.Add(RandomSpawnPatron());
        }
    }

    private GameObject RandomSpawnPatron()
    {
        float x = Random.Range(0.0f, 2.0f);
        float y = Random.Range(0.0f, 2.0f);

        Vector3 position = transform.position + new Vector3(x, y, 0);
        
        return Instantiate(Patron, position, Quaternion.identity);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0.0f;

            GameObject patron = Instantiate(Patron, position, Quaternion.identity);
            Patrons.Add(patron);

            patron.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -100.0f));
        }
    }
}
