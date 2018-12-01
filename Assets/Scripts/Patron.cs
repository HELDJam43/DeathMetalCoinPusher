using UnityEngine;

public class Patron : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Rigidbody2D rigidbody2D = collision.otherCollider.gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(rigidBody.IsSleeping())
        {
            rigidBody.mass = 0.0f;
        }
    }
}
