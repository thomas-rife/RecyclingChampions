using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System;

public class TrashCanSelector : MonoBehaviour
{
    [SerializeField] private SelectableTrash can1;
    [SerializeField] private SelectableTrash can2;
    [SerializeField] private SelectableTrash can3;
    [SerializeField] private TrashItem currentTrashItem;
    [SerializeField] private RoundUI roundUI;
    [SerializeField] private ThrowGuide throwGuide;
    [SerializeField] private Transform guideStartPoint;

    public event Action<bool> AnswerSubmitted;

    private SelectableTrash selectedCan;
    private bool answerLocked = false;
    private bool inputEnabled = false;

    private void Start()
    {
        if (roundUI == null)
        {
            roundUI = FindFirstObjectByType<RoundUI>();
        }

        if (throwGuide == null)
        {
            throwGuide = FindFirstObjectByType<ThrowGuide>();
        }

        ResetRound();
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (!inputEnabled || answerLocked)
        {
            return;
        }

        // keyboard input as a  stand-in for controller or gesture input
        if (WasSelectionPressed(Keyboard.current.digit1Key, Keyboard.current.numpad1Key, Keyboard.current.bKey))
        {
            SubmitSelection(can1);
        }
        else if (WasSelectionPressed(Keyboard.current.digit2Key, Keyboard.current.numpad2Key, Keyboard.current.nKey))
        {
            SubmitSelection(can2);
        }
        else if (WasSelectionPressed(Keyboard.current.digit3Key, Keyboard.current.numpad3Key, Keyboard.current.mKey))
        {
            SubmitSelection(can3);
        }
    }

    public void SubmitSelection(BinType binType)
    {
        SubmitSelection(GetSelectableForBin(binType));
    }

    public void SubmitSelection(SelectableTrash selected)
    {
        if (!inputEnabled || answerLocked)
        {
            return;
        }

        if (selected == null)
        {
            return;
        }

        if (currentTrashItem == null)
        {
            return;
        }

        selectedCan = selected;
        answerLocked = true;

        if (can1 != null) can1.SetSelected(can1 == selected);
        if (can2 != null) can2.SetSelected(can2 == selected);
        if (can3 != null) can3.SetSelected(can3 == selected);

        // The trail guide always points to the correct bin so the player gets a visual queue
        // after choosing, even if they selected the wrong can.
        ShowThrowGuide();

        CheckAnswer();
    }

    private void CheckAnswer()
    {
        if (selectedCan == null || currentTrashItem == null)
        {
            return;
        }

        bool correct = selectedCan.BinType == currentTrashItem.CorrectBin;

        if (roundUI != null)
        {
            roundUI.ShowResult(correct, currentTrashItem);
        }

        AnswerSubmitted?.Invoke(correct);

        if (correct)
        {
            selectedCan.ShowCorrect();
        }
        else
        {
            selectedCan.ShowWrong();
        }
    }

    public void ResetRound()
    {
        answerLocked = false;
        selectedCan = null;

        if (throwGuide != null)
        {
            throwGuide.HideGuide();
        }

        if (roundUI != null)
        {
            if (currentTrashItem != null)
            {
                roundUI.ShowItemInfo(currentTrashItem);
            }
            else
            {
                roundUI.ClearUI();
            }
        }

        if (can1 != null) can1.ResetVisuals();
        if (can2 != null) can2.ResetVisuals();
        if (can3 != null) can3.ResetVisuals();
    }

    public void SetCurrentTrashItem(TrashItem item)
    {
        currentTrashItem = item;

        if (roundUI != null)
        {
            if (currentTrashItem != null)
            {
                roundUI.ShowItemInfo(currentTrashItem);
            }
            else
            {
                roundUI.ClearUI();
            }
        }
    }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
    }

    public void ForceResetSelection()
    {
        ResetRound();
    }

    private SelectableTrash GetSelectableForBin(BinType binType)
    {
        if (can1 != null && can1.BinType == binType)
        {
            return can1;
        }

        if (can2 != null && can2.BinType == binType)
        {
            return can2;
        }

        if (can3 != null && can3.BinType == binType)
        {
            return can3;
        }

        return null;
    }

    private static bool WasSelectionPressed(params KeyControl[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] != null && keys[i].wasPressedThisFrame)
            {
                return true;
            }
        }

        return false;
    }

    private void ShowThrowGuide()
    {
        if (throwGuide == null || currentTrashItem == null)
        {
            return;
        }

        // The guide start is a fixed scene anchor near the controller, not the detected
        // real-world object, so the line stays stable during testing, will be changed later 
        SelectableTrash targetCan = GetSelectableForBin(currentTrashItem.CorrectBin);
        if (targetCan == null)
        {
            return;
        }

        Vector3 startPosition = guideStartPoint != null
            ? guideStartPoint.position
            : fallbackGuideStartPosition;

        throwGuide.ShowGuide(startPosition, targetCan.transform.position);
    }
}
