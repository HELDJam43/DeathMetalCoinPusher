using System.Collections;
using UnityEngine;

public class Patron : MonoBehaviour
{
    public enum PatronType
    {
        Banger,
        Swinger,
        BigGuy
    }

    public PatronType patronType = PatronType.Banger;

    [Range(1, 10)]
    public float IdleMass = 0.5f;

    [Range(1, 10)]
    public float LaunchMass = 5.0f;

    public float StageDiveForce = 250.0f;

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

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.mass = IdleMass;
        InvokeRepeating("RandomForce", 2, Random.Range(0.25f, 1.0f));
    }

    public void StartPitDeath(Vector2 dropPos)
    {
        if (!_isDying)
        {
            StartCoroutine("PitDeath", dropPos);
        }
    }

    private IEnumerator PitDeath(Vector2 dropPos)
    {
        CrowdSpawner.RemovePatron(gameObject);
        
        _isDying = true;
        _rigidBody.simulated = false;
        AnimController.FallingInPit();
        float pitDeathDeltaScale = transform.localScale.x / PitDeathTime;
        float distToDropPos = (new Vector3(dropPos.x, dropPos.y, 0f) - transform.position).magnitude;
        float moveDelta = distToDropPos / PitDeathTime;

        while (transform.localScale.x >= 0)
        {
            float scaleComp = transform.localScale.x - (pitDeathDeltaScale * Time.deltaTime);
            Vector3 newScale = new Vector3(scaleComp, scaleComp, 1.0f);

            transform.localScale = newScale;
            transform.position = Vector3.MoveTowards(transform.position, dropPos, moveDelta * Time.deltaTime);
            yield return null;
        }

        GameManager.Instance.AddPoints(PointValue);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (patronType == PatronType.BigGuy)
        {
            if (collision.gameObject.GetComponent<Patron>() != null)
            {
                Rigidbody2D rigidBody2D = collision.gameObject.GetComponent<Rigidbody2D>();
                rigidBody2D.AddExplosionForce(10, transform.localPosition, 10);
            }
        }
    }

    private void RandomForce()
    {
        float x = Random.Range(-5.0f, 5.0f);
        float y = Random.Range(-0.5f, 5.0f);

        _rigidBody.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
    }
}
