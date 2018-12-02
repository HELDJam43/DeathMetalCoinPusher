using UnityEngine;
using UnityEngine.UI;

public class UIBloodVial : MonoBehaviour
{
    public Image Blood;

    private void Start()
    {
        Blood.fillAmount = 0.0f;
        GameManager.Instance.OnBonusPointsAdded.AddListener(FillBloodVial);
    }

    private void FillBloodVial(int points, int bonus)
    {
        Blood.fillAmount = bonus / 666.0f;
    }
}
