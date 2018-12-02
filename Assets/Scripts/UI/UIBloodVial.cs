using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIBloodVial : MonoBehaviour
{
    public Image Blood;
    public TMP_Text ScoreText;

    private void Start()
    {
        Blood.fillAmount = 0.0f;
        ScoreText.text = string.Empty;
        GameManager.Instance.OnBonusPointsAdded.AddListener(FillBloodVial);
    }

    private void FillBloodVial(int points, int bonus)
    {
        Blood.fillAmount = bonus / 666.0f;
        ScoreText.text = bonus.ToString();

        if(bonus >= 666)
        {
            StartCoroutine(FlashText());
        }
    }

    private IEnumerator FlashText()
    {
        while(GameManager.Instance.IsBonusMode)
        {
            ScoreText.alpha = Mathf.PingPong(1 / MusicManager.BeatsPerSecond * 30.0f * Time.time, 1.0f);
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        ScoreText.alpha = 1.0f;
        ScoreText.text = string.Empty;
    }
}
