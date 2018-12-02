using UnityEngine;
using UnityEngine.UI;

public class UIBloodVial : MonoBehaviour
{
    public Image Blood;

    private void Start()
    {
        Blood.fillAmount = 0.0f;
        GameManager.Instance.OnSacrificeAdded.AddListener(FillBloodVial);
    }

    private void FillBloodVial(int points, int CurrentSacrifices)
    {
        Blood.fillAmount = CurrentSacrifices / 66.6f;
    }
}
