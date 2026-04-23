using UnityEngine;
using TMPro;
using System.Collections;

public class RoundManager : MonoBehaviour
{
    // The demo only has one detected item per run, but the explicit states keep
    // detections, answer input, and the delayed finish screen from overlapping.
    private enum RoundState
    {
        WaitingForDetection,
        AwaitingAnswer,
        ShowingResult,
        Complete
    }

    [SerializeField] private TrashItem[] trashItems;
    [SerializeField] private TrashCanSelector trashCanSelector;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float nextRoundDelay = 20f;
    [SerializeField] private DetectionSimulator detectionSimulator;

    private int currentIndex = -1;
    private int score = 0;
    private RoundState roundState = RoundState.WaitingForDetection;

    private void Awake()
    {
        if (trashCanSelector == null)
        {
            trashCanSelector = GetComponent<TrashCanSelector>();
        }

        if (detectionSimulator == null)
        {
            detectionSimulator = GetComponent<DetectionSimulator>();
        }

        if (trashCanSelector != null)
        {
            trashCanSelector.AnswerSubmitted += HandleAnswerSubmitted;
        }
    }

    private void Start()
    {
        roundState = RoundState.WaitingForDetection;
        currentIndex = -1;
        score = 0;
        UpdateScoreUI();
        ClearActiveRound(showWaitingUI: true);
    }

    private void OnDestroy()
    {
        if (trashCanSelector != null)
        {
            trashCanSelector.AnswerSubmitted -= HandleAnswerSubmitted;
        }
    }

    private void HandleAnswerSubmitted(bool correct)
    {
        if (roundState != RoundState.AwaitingAnswer || currentIndex < 0)
            return;

        roundState = RoundState.ShowingResult;

        if (correct)
        {
            score++;
            UpdateScoreUI();
        }

        StartCoroutine(AdvanceAfterDelay());
    }

    private IEnumerator AdvanceAfterDelay()
    {
        yield return new WaitForSeconds(nextRoundDelay);
        ShowGameComplete();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public bool SetDetectedLabel(string label)
    {
        if (roundState != RoundState.WaitingForDetection || string.IsNullOrWhiteSpace(label) || trashItems == null)
        {
            return false;
        }

        for (int i = 0; i < trashItems.Length; i++)
        {
            TrashItem item = trashItems[i];
            if (item == null)
                continue;

            if (item.MatchesDetectionLabel(label))
            {
                BeginDetectedRound(i);
                return true;
            }
        }

        return false;
    }

    private void BeginDetectedRound(int index)
    {
        if (index < 0 || index >= trashItems.Length)
        {
            return;
        }

        TrashItem item = trashItems[index];
        if (item == null)
        {
            return;
        }

        for (int i = 0; i < trashItems.Length; i++)
        {
            if (trashItems[i] != null)
            {
                trashItems[i].gameObject.SetActive(i == index);
            }
        }

        currentIndex = index;
        roundState = RoundState.AwaitingAnswer;

        if (trashCanSelector != null)
        {
            // Reset selection visuals before binding the newly detected item.
            trashCanSelector.ForceResetSelection();
            trashCanSelector.SetCurrentTrashItem(item);
            trashCanSelector.SetInputEnabled(true);
        }
    }

    private void ClearActiveRound(bool showWaitingUI)
    {
        currentIndex = -1;

        if (trashItems != null)
        {
            for (int i = 0; i < trashItems.Length; i++)
            {
                if (trashItems[i] != null)
                {
                    trashItems[i].gameObject.SetActive(false);
                }
            }
        }

        if (trashCanSelector != null)
        {
            trashCanSelector.SetInputEnabled(false);
            trashCanSelector.ForceResetSelection();
            trashCanSelector.SetCurrentTrashItem(null);
        }

        if (showWaitingUI)
        {
            RoundUI roundUI = FindFirstObjectByType<RoundUI>();
            if (roundUI != null)
            {
                roundUI.ClearUI();
            }
        }

        if (detectionSimulator != null)
        {
            detectionSimulator.ClearDetectionLock();
        }
    }

    private void ShowGameComplete()
    {
        roundState = RoundState.Complete;
        ClearActiveRound(showWaitingUI: false);

        RoundUI roundUI = FindFirstObjectByType<RoundUI>();
        if (roundUI != null)
        {
            roundUI.ShowGameComplete(score, 1);
        }
    }
}
