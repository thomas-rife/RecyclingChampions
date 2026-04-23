using UnityEngine;

// does not completely work yet in unity simulator, need to test on headset first
// the idea is to box the trash object the user is holding

public class DetectionOverlay : MonoBehaviour
{
    [SerializeField] private RectTransform boxRect;
    [SerializeField] private RectTransform canvasRect;

    public void UpdateBox(float centerX, float centerY, float width, float height)
    {
        if (boxRect == null || canvasRect == null) return;

        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // MediaPipe/webhook coordinates arrive normalized, so convert them into the
        // overlay canvas' local space before sizing the box.
        float x = centerX * canvasWidth - canvasWidth * 0.5f;
        float y = (1f - centerY) * canvasHeight - canvasHeight * 0.5f;

        boxRect.anchoredPosition = new Vector2(x, y);
        boxRect.sizeDelta = new Vector2(width * canvasWidth, height * canvasHeight);
    }
}
