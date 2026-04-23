using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

[Serializable]
public class DetectionPoint
{
    public float x;
    public float y;
}

[Serializable]
public class DetectionSize
{
    public float w;
    public float h;
}

[Serializable]
public class DetectionItem
{
    public string label;
    public string bin;
    public float score;
    public DetectionPoint center;
    public DetectionSize size;
}

[Serializable]
public class DetectionPayload
{
    public DetectionItem[] detections;
}

public class WebhookReceiver : MonoBehaviour
{
    public static WebhookReceiver Instance;

    [SerializeField] private string listenPrefix = "http://127.0.0.1:8080/";
    private HttpListener listener;
    private Thread listenerThread;

    private readonly object lockObject = new object();
    private string pendingJson = null;

    [SerializeField] private DetectionSimulator detectionSimulator;

    private void Awake()
    {
        Instance = this;

        if (detectionSimulator == null)
        {
            detectionSimulator = GetComponent<DetectionSimulator>();
        }
    }

    private void Start()
    {
        try
        {
            listener = new HttpListener();
            listener.Prefixes.Add(listenPrefix);
            listener.Start();

            listenerThread = new Thread(ListenLoop);
            listenerThread.IsBackground = true;
            listenerThread.Start();

            Debug.Log("WebhookReceiver listening on " + listenPrefix);
        }
        catch (Exception e)
        {
            Debug.LogError("WebhookReceiver failed to start: " + e.Message);
        }
    }

    private void Update()
    {
        string jsonToProcess = null;

        lock (lockObject)
        {
            if (pendingJson != null)
            {
                jsonToProcess = pendingJson;
                pendingJson = null;
            }
        }

        if (!string.IsNullOrEmpty(jsonToProcess))
        {
            if (detectionSimulator == null)
            {
                detectionSimulator = GetComponent<DetectionSimulator>();
            }

            try
            {
                // Only the top scoring item is forwarded into the game loop.
                DetectionPayload payload = JsonUtility.FromJson<DetectionPayload>(jsonToProcess);
                DetectionItem top = GetTopDetection(payload);

                if (top != null && detectionSimulator != null)
                {
                    float centerX = top.center != null ? top.center.x : 0.5f;
                    float centerY = top.center != null ? top.center.y : 0.5f;
                    float width = top.size != null ? top.size.w : 0f;
                    float height = top.size != null ? top.size.h : 0f;

                    detectionSimulator.HandleDetection(top.label, centerX, centerY, width, height);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("WebhookReceiver failed to parse payload: " + e.Message);
            }
        }
    }

    private void ListenLoop()
    {
        while (listener != null && listener.IsListening)
        {
            try
            {
                var context = listener.GetContext();
                string body = "";

                using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                {
                    body = reader.ReadToEnd();
                }

                // Queue the most recent payload for processing
                lock (lockObject)
                {
                    pendingJson = body;
                }

                byte[] responseBytes = Encoding.UTF8.GetBytes("OK");
                context.Response.StatusCode = 200;
                context.Response.ContentLength64 = responseBytes.Length;
                context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                context.Response.OutputStream.Close();
            }
            catch (Exception e)
            {
                if (listener != null && listener.IsListening)
                {
                    Debug.LogWarning("Webhook listener stopped or errored: " + e.Message);
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (listener != null && listener.IsListening)
        {
            listener.Stop();
            listener.Close();
        }

        if (listenerThread != null && listenerThread.IsAlive)
        {
            listenerThread.Join(250);
        }
    }

    private static DetectionItem GetTopDetection(DetectionPayload payload)
    {
        if (payload == null || payload.detections == null || payload.detections.Length == 0)
        {
            return null;
        }

        DetectionItem best = null;
        for (int i = 0; i < payload.detections.Length; i++)
        {
            DetectionItem candidate = payload.detections[i];
            if (candidate == null || string.IsNullOrWhiteSpace(candidate.label))
            {
                continue;
            }

            if (best == null || candidate.score > best.score)
            {
                best = candidate;
            }
        }

        return best;
    }
}
