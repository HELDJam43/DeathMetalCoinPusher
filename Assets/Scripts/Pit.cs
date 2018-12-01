using UnityEngine;

public class Pit : MonoBehaviour
{
    [SerializeField]
    private Transform BottomLeft;
    [SerializeField]
    private Transform BottomRight;

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
            if (patX > BottomRight.position.x)
            {
                dropPos.x = BottomRight.position.x;
            }
            else
            {
                dropPos.x = patX;
            }
        }

        return dropPos;
    }
}
