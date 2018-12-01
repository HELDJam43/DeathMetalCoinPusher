using UnityEngine;

public class PatronSpawner : MonoBehaviour
{
    public enum Door
    {
        Left,
        Right
    }

    public GameObject Patron;

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
            GameObject patron = Instantiate(Patron, transform.position, Quaternion.identity) as GameObject;
            Rigidbody2D rigidbody2D = patron.GetComponent<Rigidbody2D>();
            rigidbody2D.AddForce(door == Door.Right ? Vector3.left * force : Vector3.right * force, ForceMode2D.Impulse);
        }
    }
}
