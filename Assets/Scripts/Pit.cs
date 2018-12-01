using UnityEngine;

public class Pit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Patron patron = collider.GetComponent<Patron>();
        if(patron != null)
        {
            Destroy(patron.gameObject);
        }
    }
}
