using UnityEngine;
using UnityEngine.InputSystem;

public class TrashCanSelector : MonoBehaviour
{
    [SerializeField] private SelectableTrash can1;
    [SerializeField] private SelectableTrash can2;
    [SerializeField] private SelectableTrash can3;

    private void Start()
    {
        Debug.Log("TrashCanSelector Start ran");
        HighlightOnly(null);
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            Debug.Log("Keyboard.current is null");
            return;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Debug.Log("Pressed 1");
            HighlightOnly(can1);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Debug.Log("Pressed 2");
            HighlightOnly(can2);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            Debug.Log("Pressed 3");
            HighlightOnly(can3);
        }
    }

    private void HighlightOnly(SelectableTrash selected)
    {
        Debug.Log("Selected: " + (selected != null ? selected.name : "none"));

        if (can1 != null) can1.SetHighlighted(can1 == selected);
        else Debug.Log("can1 is null");

        if (can2 != null) can2.SetHighlighted(can2 == selected);
        else Debug.Log("can2 is null");

        if (can3 != null) can3.SetHighlighted(can3 == selected);
        else Debug.Log("can3 is null");
    }
}