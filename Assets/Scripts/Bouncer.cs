using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField]
    private Animation _animator;

    public float BouncerForce = 10.0f;

    private bool _isPushing;
    private const int IDLE_ANIM_IDX = 0;
    private const int PUSH_ANIM_IDX = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Patron patron = collision.GetComponent<Patron>();

        if(patron != null)
        {
            Rigidbody2D rigidBody = patron.GetComponent<Rigidbody2D>();
            rigidBody.AddForce(new Vector2(0, -BouncerForce), ForceMode2D.Impulse);
            StartPush();
        }
    }

    private void StartPush()
    {
        _animator.Play("BouncerPush");
        _isPushing = true;
    }

    private void StopPush()
    {
        _animator.Play("BouncerIdle");
        _isPushing = false;
    }

    private void Update()
    {
        if (_isPushing && !_animator.isPlaying)
        {
            StopPush();
        }
    }
}
