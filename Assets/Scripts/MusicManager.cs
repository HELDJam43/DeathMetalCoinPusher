using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour 
{
    public static float BeatsPerSecond;

    public AudioSource Source;

    public AudioClip IntroLoop;
    //private float IntroBPS = 8f / 12.8f;
    private float IntroBPS = 16f / 12.3f;

    public AudioClip MainLoop;
    //private float MainBPS = 8f / 5.3f;
    private float MainBPS = 8f / 3.556f;

    public AudioClip FastLoop;
    private float FastBPS = 32f / 9.14f;

	// Use this for initialization
	void Start () 
    {
        PlayIntro();
    }
	
	// Update is called once per frame
	void Update () 
    {
		if (Source.clip == IntroLoop && !Source.isPlaying)
        {
            PlayMain();
        }
    }

    public void PlayIntro()
    {
        Source.clip = IntroLoop;
        Source.loop = false;
        Source.time = 6f;
        Source.Play();

        BeatsPerSecond = IntroBPS;
    }
    public void PlayMain()
    {
        Source.clip = MainLoop;
        Source.time = 0f;
        Source.loop = true;
        Source.Play();

        BeatsPerSecond = MainBPS;
    }
    public void PlayFast()
    {
        Source.clip = FastLoop;
        Source.time = 0f;
        Source.loop = true;
        Source.Play();

        BeatsPerSecond = FastBPS;
    }

}
