using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIGameTimer : MonoBehaviour
{
    private float TimeLeft = 66.6f; 
    public TMP_Text countdown;
    public TMP_Text title;
    public GameObject restartButton;

    private void Awake()
    {
        restartButton.SetActive(false);
    }

    private void Start()
    {
        countdown = GetComponent<TMP_Text>();
    }

    public void StartCountdownTimer()
    {
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        countdown.text = "Time: " + TimeLeft.ToString("F1"); 
    }

    private IEnumerator LoseTime()
    {
        while (TimeLeft > 0)
        {
            yield return new WaitForEndOfFrame();
            TimeLeft -= Time.deltaTime;
        }

        if(TimeLeft == 0)
        {
            //Game Over
        }
    }

    private IEnumerator StartGame()
    {
     
        MusicManager musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        musicManager.PlayIntro();
        yield return new WaitForSeconds(1.45f);
        title.text = "3";
        yield return new WaitForSeconds(1.45f);
        title.text = "2";
        yield return new WaitForSeconds(1.45f);
        title.text = "1";
        yield return new WaitForSeconds(1.45f);
        title.text = "ROCK!";
        yield return new WaitForSeconds(0.20f);
        GameManager.Instance.OnGameStartingEvent.Invoke(0);
        title.text = string.Empty;
        StartCoroutine(LoseTime());
        GameManager.CurrentGameState = GameManager.GameState.Play;

        while (TimeLeft > 0)
        {
            yield return null;
        }
        GameManager.CurrentGameState = GameManager.GameState.Ending;
        title.text = "GAME OVER";
        restartButton.SetActive(true);
        GameManager.Instance.InvokeGameOver();
    }

    public void OnRestartSelected()
    {
        SceneManager.LoadScene(0);
    }
}

