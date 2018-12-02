using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour 
{
    public AudioSource Source;
    public AudioClip IntroLoop;
    public AudioClip MainLoop;

	// Use this for initialization
	void Start () 
    {
        Source.clip = IntroLoop;
        Source.loop = false;
        Source.time = 6f;
        Source.Play();
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (Source.clip == IntroLoop && Source.time >= Source.clip.length)
        {
            Source.clip = MainLoop;
            Source.time = 0f;
            Source.loop = true;
            Source.Play();
        }
    }
}
