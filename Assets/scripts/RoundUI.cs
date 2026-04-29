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

        // TODO:
        // Change how the trash item name is shown.
        // Example: add stars or extra words around the name.
        // "Detected: " + item.ItemName
        // "*** " + item.ItemName + " ***"
        // "Trash Item: " + item.ItemName
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

        // TODO:
        // Change the words shown when the player is correct or incorrect.
        // Only edit the text in the quotes, don't change: correct ?
        // Example ideas: "Nice job!" or "Try again!"
        titleText.text = correct ? "Correct!" : "Incorrect";
        descriptionText.text = item.Explanation;
    }

    public void ShowGameComplete(int score, int total)
    {
        if (titleText == null || descriptionText == null)
        {
            return;
        }


        // TODO:
        // Change the final message shown when the game ends.
        // Example ideas: "Round Complete!" or "Great Work!"
        titleText.text = "Finished!";

        // TODO:
        // Change how the final score message is written.
        // Example: "You scored: " + score
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
