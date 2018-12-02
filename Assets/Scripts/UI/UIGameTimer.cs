using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIGameTimer : MonoBehaviour
{
    public int TimeLeft = 3; 
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
        countdown.text = "Time: " + TimeLeft; 
    }

    private IEnumerator LoseTime()
    {
        while (TimeLeft > 0)
        {
            yield return new WaitForSeconds(1.0f);
            TimeLeft--;
        }

        if(TimeLeft == 0)
        {
            //Game Over
        }
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2.0f);
        title.text = "3";
        yield return new WaitForSeconds(1.0f);
        title.text = "2";
        yield return new WaitForSeconds(1.0f);
        title.text = "1";
        yield return new WaitForSeconds(1.0f);
        title.text = "ROCK!";
        yield return new WaitForSeconds(1.0f);
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
    }

    public void OnRestartSelected()
    {
        SceneManager.LoadScene(0);
    }
}

