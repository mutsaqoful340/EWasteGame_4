using System.Collections;
using UnityEngine;

public class TimelineSwitch : MonoBehaviour
{
    [Header("Assign the object to deactivate")]
    public GameObject objectToDeactivate;

    [Header("Time in seconds before deactivation")]
    public float deactivateTime = 3f;

    void Start()
    {
        StartCoroutine(DeactivateAfterTime());
    }

    IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(deactivateTime);

        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No object assigned to deactivate!");
        }
    }
}
