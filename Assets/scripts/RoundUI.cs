using UnityEngine;
using TMPro;

public class RoundUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void ShowItemInfo(TrashItem item)
    {
        if (item == null)
        {
            return;
        }

        if (titleText == null || descriptionText == null)
        {
            return;
        }

        // Before selection, the UI shows the detected item's name and learning prompt.
        titleText.text = item.ItemName;
        descriptionText.text = item.Description;
    }

    public void ShowResult(bool correct, TrashItem item)
    {
        if (item == null)
        {
            return;
        }

        if (titleText == null || descriptionText == null)
        {
            return;
        }

        // After selection, the same panel is reused for the result and explanation text.
        titleText.text = correct ? "Correct!" : "Incorrect";
        descriptionText.text = item.Explanation;
    }

    public void ShowGameComplete(int score, int total)
    {
        if (titleText == null || descriptionText == null)
        {
            return;
        }

        titleText.text = "Finished!";
        descriptionText.text = "Final Score: " + score + " / " + total;
    }
    public void ClearUI()
    {
        if (titleText != null)
        {
            titleText.text = "";
        }

        if (descriptionText != null)
        {
            descriptionText.text = "";
        }
    }
}
