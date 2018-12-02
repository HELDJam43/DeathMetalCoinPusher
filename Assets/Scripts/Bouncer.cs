using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public float BouncerForce = 10.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Patron patron = collision.GetComponent<Patron>();

        if(patron != null)
        {
            Rigidbody2D rigidBody = patron.GetComponent<Rigidbody2D>();
            rigidBody.AddForce(new Vector2(0, -BouncerForce), ForceMode2D.Impulse);
        }
    }
}
