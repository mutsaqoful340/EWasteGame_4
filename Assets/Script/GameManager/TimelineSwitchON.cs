using System.Collections;
using UnityEngine;

public class TimelineSwitchON : MonoBehaviour
{
    [Header("Assign the object to activate")]
    public GameObject objectToActivate;

    [Header("Time in seconds before activation")]
    public float activateTime = 3f;

    void Start()
    {
        StartCoroutine(ActivateAfterTime());
    }

    IEnumerator ActivateAfterTime()
    {
        yield return new WaitForSeconds(activateTime);

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No object assigned to activate!");
        }
    }
}
