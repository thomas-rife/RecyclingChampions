using UnityEngine;

public class DetectionSimulator : MonoBehaviour
{
    [SerializeField] private RoundManager roundManager;
    [SerializeField] private float detectionCooldown = 2f;

    private string lastAcceptedLabel = "";
    private float lastAcceptedTime = -999f;

    public void HandleDetection(string label, float centerX, float centerY, float width, float height)
    {
        if (roundManager == null)
        {
            roundManager = GetComponent<RoundManager>();
        }

        if (string.IsNullOrWhiteSpace(label))
        {
            return;
        }

        string normalizedLabel = label.Trim().ToLowerInvariant();

        if (roundManager == null)
        {
            return;
        }

        // Repeated detections of the same label are ignored for a short window so
        // one held object does not reopen the round over and over.
        bool sameLabel = normalizedLabel == lastAcceptedLabel;
        bool stillCoolingDown = Time.time - lastAcceptedTime < detectionCooldown;

        if (sameLabel && stillCoolingDown)
        {
            return;
        }

        if (roundManager.SetDetectedLabel(normalizedLabel))
        {
            lastAcceptedLabel = normalizedLabel;
            lastAcceptedTime = Time.time;
        }
    }

    public void ClearDetectionLock()
    {
        lastAcceptedLabel = "";
        lastAcceptedTime = -999f;
    }
}
