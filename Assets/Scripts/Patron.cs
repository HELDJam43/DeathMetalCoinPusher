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

    [Range(1, 1000)]
    public int PointValue;

    public PatronDrawOrder _drawOrder;
    public PatronAnimatorController AnimController;

    public void StartPitDeath(Vector2 dropPos)
    {
        if (!_isDying)
        {
            StartCoroutine("PitDeath", dropPos);
        }
    }

    private IEnumerator PitDeath(Vector2 dropPos)
    {
        CrowdSpawner.RemovePatron(this.gameObject);
        
        _isDying = true;
        _rigidBody.simulated = false;
        AnimController.FallingInPit();
        float _pitDeathDeltaScale = this.transform.localScale.x / PitDeathTime;
        float distToDropPos = (new Vector3(dropPos.x, dropPos.y, 0f) - transform.position).magnitude;
        float moveDelta = distToDropPos / PitDeathTime;

        while (this.transform.localScale.x >= 0)
        {
            float scaleComp = transform.localScale.x - (_pitDeathDeltaScale * Time.deltaTime);
            Vector3 newScale = new Vector3(scaleComp, scaleComp, 1.0f);

            transform.localScale = newScale;
            transform.position = Vector3.MoveTowards(transform.position, dropPos, moveDelta * Time.deltaTime);
            yield return null;
        }

        GameManager.Instance.AddPoints(this.PointValue);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        InvokeRepeating("RandomForce", 1, Random.Range(0.25f, 1.0f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rigidbody2D = collision.otherCollider.gameObject.GetComponent<Rigidbody2D>();

    }

    private void RandomForce()
    {
        float x = Random.Range(-5.0f, 5.0f);
        float y = Random.Range(-0.5f, 5.0f);

        _rigidBody.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
    }
}
