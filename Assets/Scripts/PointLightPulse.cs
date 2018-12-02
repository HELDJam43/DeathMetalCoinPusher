using UnityEngine;

public class PointLightPulse : MonoBehaviour
{
    public float Range = 4.0f;
    public float Speed
    {
        get
        {
            return MusicManager.BeatsPerSecond * 30f;
        }
    }

    public float Minimum = 1.0f;

    private Light _light;

    private void Start()
    {
        _light = GetComponent<Light>();
    }

    private void Update()
    {
        _light.range = Minimum + Mathf.PingPong(Time.time * Speed, Range - Minimum);
    }
}
