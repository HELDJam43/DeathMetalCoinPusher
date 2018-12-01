using System.Collections;
using UnityEngine;

public class Patron : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 2)]
    private float PitDeathTime;

    //private float _pitDeathDeltaScale;
    private bool _isDying = false;

    private Rigidbody2D _rigidBody;

    public void StartPitDeath(Vector2 dropPos)
    {
        if (!_isDying)
        {
            StartCoroutine("PitDeath", dropPos);
        }
    }

    private IEnumerator PitDeath(Vector2 dropPos)
    {
        _isDying = true;
        _rigidBody.simulated = false;
        float _pitDeathDeltaScale = this.transform.localScale.x / PitDeathTime;
        float distToDropPos = (new Vector3(dropPos.x, dropPos.y, 0f) - transform.position).magnitude;
        float moveDelta = distToDropPos / PitDeathTime;

        while (this.transform.localScale.x >= 0)
        {
            float scaleComp = transform.localScale.x - (_pitDeathDeltaScale * Time.deltaTime);
            Vector3 newScale = new Vector3(scaleComp, scaleComp);
            transform.localScale = newScale;
            transform.position = Vector3.MoveTowards(transform.position, dropPos, moveDelta * Time.deltaTime);
            yield return null;
        }

    }

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Rigidbody2D rigidbody2D = collision.otherCollider.gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(_rigidBody.IsSleeping())
        {
            _rigidBody.mass = 0.0f;
        }
    }
}
