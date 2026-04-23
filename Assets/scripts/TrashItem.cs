using UnityEngine;

public class TrashItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private BinType correctBin;
    [SerializeField] private string description;
    [SerializeField] private string explanation;
    [SerializeField] private string[] detectionLabels;

    public string ItemName => itemName;
    public BinType CorrectBin => correctBin;
    public string Description => description;
    public string Explanation => explanation;

    public bool MatchesDetectionLabel(string label)
    {
        string normalizedLabel = NormalizeLabel(label);
        string normalizedItemName = NormalizeLabel(itemName);
        if (string.IsNullOrEmpty(normalizedLabel))
        {
            return false;
        }

        if (normalizedLabel == normalizedItemName)
        {
            return true;
        }

        if (normalizedLabel == NormalizeLabel((itemName ?? string.Empty).Replace(" ", string.Empty)))
        {
            return true;
        }

        string[] itemWords = string.IsNullOrWhiteSpace(itemName)
            ? System.Array.Empty<string>()
            : itemName.Split(' ');
        if (itemWords.Length > 0 && normalizedLabel == NormalizeLabel(itemWords[itemWords.Length - 1]))
        {
            return true;
        }

        if (detectionLabels == null)
        {
            return false;
        }

        // Extra aliases let the detector vocabulary stay flexible without hardcoding
        // label checks back in the round manager.
        for (int i = 0; i < detectionLabels.Length; i++)
        {
            if (normalizedLabel == NormalizeLabel(detectionLabels[i]))
            {
                return true;
            }
        }

        return false;
    }

    private static string NormalizeLabel(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : value.Trim().ToLowerInvariant();
    }
}
