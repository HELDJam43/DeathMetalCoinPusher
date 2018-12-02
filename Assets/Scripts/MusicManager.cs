using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour 
{
    public static float BeatsPerSecond;

    public AudioSource Source;

    public AudioClip IntroLoop;
    private float IntroBPS = 8f / 12.8f;

    public AudioClip MainLoop;
    private float MainBPS = 8f / 5.3f;

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
		if (Source.clip == IntroLoop && Source.time >= Source.clip.length)
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
