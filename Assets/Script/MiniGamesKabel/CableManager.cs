using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CableManager : MonoBehaviour
{
    [Header("Cable Setup")]
    public RectTransform portA;
    public RectTransform cableHeadRT;
    public RectTransform cableSegmentsParent;
    public GameObject cableSegmentPrefab;
    public string cableID;

    [Header("Target Ports")]
    public List<Port> allPorts;
    public float connectThreshold = 50f;

    [Header("Cable Drawing Settings")]
    public float segmentSpacing = 20f;
    public int maxSegmentCount = 200;
    public float drawDistanceThreshold = 5f;

    private List<GameObject> spawnedSegments = new List<GameObject>();
    private Queue<GameObject> segmentPool = new Queue<GameObject>();
    private bool isConnected = false;
    private Vector2 lastDrawPos;

    void Start()
    {
        ResetCable();
    }

    public void SafeDrawCable(Vector2 currentPos)
    {
        if (isConnected) return;

        if (Vector2.Distance(currentPos, lastDrawPos) < drawDistanceThreshold)
            return;

        lastDrawPos = currentPos;
        ClearCable();

        Vector2 start = portA.anchoredPosition;
        Vector2 dir = (currentPos - start).normalized;
        float distance = Vector2.Distance(start, currentPos);
        int count = Mathf.FloorToInt(distance / segmentSpacing);
        count = Mathf.Min(count, maxSegmentCount);

        for (int i = 0; i < count; i++)
        {
            Vector2 pos = start + dir * (i * segmentSpacing);
            GameObject seg = GetSegment();
            RectTransform rt = seg.GetComponent<RectTransform>();
            rt.anchoredPosition = pos;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rt.rotation = Quaternion.Euler(0, 0, angle);
            spawnedSegments.Add(seg);
        }

        cableHeadRT.anchoredPosition = currentPos;
    }

    public void TryConnect()
    {
        if (isConnected) return;

        Port closestPort = null;
        float minDist = Mathf.Infinity;

        foreach (Port p in allPorts)
        {
            float dist = Vector2.Distance(cableHeadRT.anchoredPosition, ((RectTransform)p.transform).anchoredPosition);
            if (dist < minDist && dist <= connectThreshold)
            {
                minDist = dist;
                closestPort = p;
            }
        }

        if (closestPort != null)
        {
            if (closestPort.portID == cableID)
            {
                OnCableConnected(closestPort);
            }
            else
            {
                Debug.Log("❌ Salah port: Kabel " + cableID + " tidak cocok dengan Port " + closestPort.portID);
                ResetCable();
            }
        }
        else
        {
            Debug.Log("⚠️ Tidak dekat dengan port manapun.");
            ResetCable();
        }
    }

    public void OnCableConnected(Port port)
    {
        isConnected = true;
        cableHeadRT.anchoredPosition = ((RectTransform)port.transform).anchoredPosition;
        Debug.Log("✅ Kabel " + cableID + " berhasil disambung ke port " + port.portID);
    }

    public void ResetCable()
    {
        isConnected = false;
        lastDrawPos = portA.anchoredPosition;
        cableHeadRT.anchoredPosition = portA.anchoredPosition;
        ClearCable();
    }

    public void ClearCable()
    {
        foreach (var seg in spawnedSegments)
        {
            ReturnSegment(seg);
        }
        spawnedSegments.Clear();
    }

    private GameObject GetSegment()
    {
        if (segmentPool.Count > 0)
        {
            var seg = segmentPool.Dequeue();
            seg.SetActive(true);
            return seg;
        }
        else
        {
            return Instantiate(cableSegmentPrefab, cableSegmentsParent);
        }
    }

    private void ReturnSegment(GameObject seg)
    {
        seg.SetActive(false);
        segmentPool.Enqueue(seg);
    }
}
