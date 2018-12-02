using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualController : MonoBehaviour 
{
    public GameObject SingerObject;
    public GameObject DemonObject;

    private bool _isSingerActive;

	// Use this for initialization
	void Start () 
    {
        _isSingerActive = true;
        SetSingerActive();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (_isSingerActive != !GameManager.Instance.IsBonusMode)
        {
            _isSingerActive = !GameManager.Instance.IsBonusMode;
            if (_isSingerActive)
            {
                SetSingerActive();
            }
            else
            {
                SetDemonActive();
            }
        }
    }

    private void SetSingerActive()
    {
        SingerObject.SetActive(true);
        DemonObject.SetActive(false);
    }

    private void SetDemonActive()
    {
        SingerObject.SetActive(false);
        DemonObject.SetActive(true);
    }
}
