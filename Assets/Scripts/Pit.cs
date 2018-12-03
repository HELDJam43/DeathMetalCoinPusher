using UnityEngine;

public class Pit : MonoBehaviour
{
    [SerializeField]
    private Transform BottomLeft;

    [SerializeField]
    private Transform BottomRight;

    [SerializeField]
    private GameObject PitBlock;

    private void Start()
    {
        GameManager.Instance.OnGameStartingEvent.AddListener(OnGameStartingEvent);
        GameManager.Instance.OnGameOverEvent.AddListener(OnGameOverEvent);
    }

    private void OnGameStartingEvent(int arg0)
    {
        PitBlock.SetActive(false);
    }

    private void OnGameOverEvent(int arg0)
    {
        PitBlock.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Patron patron = collider.GetComponent<Patron>();

        if(patron != null)
        {
            patron.StartPitDeath(FindPitDropPosition(patron.transform));
        }
    }

    private Vector2 FindPitDropPosition(Transform patronPos)
    {
        Vector2 dropPos = BottomLeft.position;

        float patX = patronPos.position.x;

        if (patX > dropPos.x)
        {
            dropPos.x = patX > BottomRight.position.x ? BottomRight.position.x : patX;
        }

        return dropPos;
    }
}
