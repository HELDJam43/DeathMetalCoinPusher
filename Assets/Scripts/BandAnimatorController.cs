using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandAnimatorController : MonoBehaviour 
{
    [SerializeField]
    private Animation Animator;

    private float _currentBPS = 1;

    // Use this for initialization
    private void Start()
    {
        UpdateBPS();
    }

    // Update is called once per frame
    private void Update()
    {
        if (MusicManager.BeatsPerSecond != _currentBPS)
        {
            UpdateBPS();
        }
    }

    private void UpdateBPS()
    {
        _currentBPS = MusicManager.BeatsPerSecond;

        foreach (AnimationState state in Animator)
        {
            state.speed = _currentBPS;
        }
    }
}
