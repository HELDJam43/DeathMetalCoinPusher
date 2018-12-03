using UnityEngine;

public class PatronSpawner : MonoBehaviour
{
    public enum Door
    {
        Left,
        Right
    }

    public GameObject GetSpawnedPatron()
    {
        GameObject patron = Instantiate(GetRandomPatronPrefab(), transform.position, Quaternion.identity) as GameObject;
        return patron;
    }

    [SerializeField]
    private GameObject[] Patrons;

    private GameObject GetRandomPatronPrefab()
    {
        if (Random.value > 0.7) //%30 (1 - 0.7 = 0.3)
        {
            return Patrons[1];
        }

        if (Random.value > 0.8) //%20 (1 - 0.78 = 0.2)
        {
            return Patrons[2];
        }
        return Patrons[0];
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
