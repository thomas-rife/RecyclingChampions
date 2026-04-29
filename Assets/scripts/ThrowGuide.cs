using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ThrowGuide : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    // TODO: Change the smoothness of the line!
    // Change this number to make the guide line smoother or blockier
    // Example values to try: 5, 10, 30
    [SerializeField] private int pointCount = 24;

    // TODO: Change the height of the throwing arc!
    // Change this number to make the guide arc flatter or taller.
    // Example values to try: 0.2f, 0.6f, 1.0f
    [SerializeField] private float arcHeight = 0.6f;

    // ---------------------------------------------------------------
    [SerializeField] private float autoHideDelay = 0f;
    [SerializeField] private bool hideOnStart = true;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        if (lineRenderer != null)
        {
            lineRenderer.useWorldSpace = true;
        }
    }

    private void Start()
    {
        if (hideOnStart)
        {
            HideGuide();
        }
    }

    public void ShowGuide(Vector3 start, Vector3 end)
    {
        if (lineRenderer == null)
        {
            return;
        }

        int clampedPointCount = Mathf.Max(2, pointCount);
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = clampedPointCount;

        // simple parabola between the start anchor and target can.
        for (int i = 0; i < clampedPointCount; i++)
        {
            float t = i / (float)(clampedPointCount - 1);
            Vector3 point = Vector3.Lerp(start, end, t);
            point.y += arcHeight * t * (1f - t);
            lineRenderer.SetPosition(i, point);
        }

        lineRenderer.enabled = true;

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        if (autoHideDelay > 0f)
        {
            hideCoroutine = StartCoroutine(HideAfterDelay());
        }
    }

    public void ShowGuide(Transform startTransform, Transform endTransform)
    {
        if (startTransform == null || endTransform == null)
        {
            return;
        }

        ShowGuide(startTransform.position, endTransform.position);
    }

    public void HideGuide()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(autoHideDelay);
        HideGuide();
    }
}
