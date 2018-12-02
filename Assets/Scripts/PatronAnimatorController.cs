using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronAnimatorController : MonoBehaviour
{
    [SerializeField]
    private Animation Animator;

    [SerializeField]
    private AnimationClip _onStageClip;

    [SerializeField]
    private AnimationClip _stageDiveClip;

    [SerializeField]
    private AnimationClip[] _idleClips;

    [SerializeField]
    private AnimationClip _fallingClip;

    private bool _isIdle = false;

    public void OnStage()
    {
        _isIdle = false;
        Animator.clip = _onStageClip;
        Animator.Play();

    }

    public void StageDive()
    {
        _isIdle = false;
        Animator.clip = _stageDiveClip;
        
        Animator.Play();

    }

    public void Idle()
    {
        _isIdle = true;
        int rand = Random.Range(0, _idleClips.Length - 1);
        Animator.clip = _idleClips[rand];
        Animator.Play();
    }

    public void FallingInPit()
    {
        _isIdle = false;
        Animator.clip = _fallingClip;
        Animator.Play();

    }


    // Use this for initialization
    void Start()
    {
        //Idle();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO - Change up the idle occasionally
        if (_isIdle)
        {
            // CBO - warning supression!
            _isIdle = true;
        }
    }
}
