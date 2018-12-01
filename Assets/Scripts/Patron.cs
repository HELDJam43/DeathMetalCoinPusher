using UnityEngine;

public class Patron : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        InvokeRepeating("RandomForce", 2, Random.Range(3, 10.0f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Rigidbody2D rigidbody2D = collision.otherCollider.gameObject.GetComponent<Rigidbody2D>();
    }

    private void RandomForce()
    {
        float x = Random.Range(-10.0f, 10.0f);
        float y = Random.Range(-0.5f, 10.0f);

        rigidBody.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
    }
}
