using UnityEngine;

public class SelectableTrash : MonoBehaviour
{
    [SerializeField] private GameObject outlineVisual;
    [SerializeField] private GameObject arrowVisual;

    private void Start()
    {
        Debug.Log(name + " Start ran");

        if (outlineVisual != null)
        {
            Debug.Log(name + " outline = " + outlineVisual.name);
            outlineVisual.SetActive(false);
        }
        else
        {
            Debug.Log(name + " outlineVisual is NULL");
        }

        if (arrowVisual != null)
        {
            Debug.Log(name + " arrow = " + arrowVisual.name);
            arrowVisual.SetActive(false);
        }
        else
        {
            Debug.Log(name + " arrowVisual is NULL");
        }
    }

    public void SetHighlighted(bool highlighted)
    {
        Debug.Log(name + " SetHighlighted " + highlighted);

        if (outlineVisual != null)
        {
            outlineVisual.SetActive(highlighted);
        }

        if (arrowVisual != null)
        {
            arrowVisual.SetActive(highlighted);
            Debug.Log(name + " arrow activeSelf = " + arrowVisual.activeSelf);
        }
    }
}