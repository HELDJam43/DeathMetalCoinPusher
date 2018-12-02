using UnityEngine;
using TMPro;
using System.Collections;

public class UITitle : MonoBehaviour
{
    public TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

}
