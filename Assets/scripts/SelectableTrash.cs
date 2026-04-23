using UnityEngine;

public class SelectableTrash : MonoBehaviour
{
    [SerializeField] private GameObject arrowVisual;
    [SerializeField] private GameObject correctVisual;
    [SerializeField] private GameObject wrongVisual;
    [SerializeField] private BinType binType;

    public BinType BinType => binType;

    private void Start()
    {
        if (arrowVisual != null) arrowVisual.SetActive(false);
        if (correctVisual != null) correctVisual.SetActive(false);
        if (wrongVisual != null) wrongVisual.SetActive(false);
    }

    public void SetSelected(bool selected)
    {
        if (arrowVisual != null)
        {
            arrowVisual.SetActive(selected);
        }
    }

    public void ShowCorrect()
    {
        if (correctVisual != null) correctVisual.SetActive(true);
        if (wrongVisual != null) wrongVisual.SetActive(false);
    }

    public void ShowWrong()
    {
        if (correctVisual != null) correctVisual.SetActive(false);
        if (wrongVisual != null) wrongVisual.SetActive(true);
    }

    public void ResetVisuals()
    {
        // Each round starts from a neutral can state, then selection/correct/wrong
        // visuals are layered back on top.
        if (arrowVisual != null) arrowVisual.SetActive(false);
        if (correctVisual != null) correctVisual.SetActive(false);
        if (wrongVisual != null) wrongVisual.SetActive(false);
    }
}
