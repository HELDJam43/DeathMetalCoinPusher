using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronDrawOrder : MonoBehaviour 
{

    [SerializeField]
    private int NumberOfGroups;

    [SerializeField]
    private SpriteRenderer _head;

    [SerializeField]
    private SpriteRenderer[] _handsAndFeet;

    [SerializeField]
    private SpriteRenderer _torso;

    public void SetIndex(int index)
    {
        int startIndex = index * NumberOfGroups;

        _torso.sortingOrder = startIndex++;
        foreach (SpriteRenderer rend in _handsAndFeet)
        {
            rend.sortingOrder = startIndex;
        }
        _head.sortingOrder = ++startIndex;
    }
}
