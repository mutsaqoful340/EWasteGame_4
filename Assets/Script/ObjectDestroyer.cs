using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [Header("Object to Destroy")]
    public GameObject objectToDestroy;

    [Header("Delay Settings")]
    public float destroyDelay = 2f; // Time in seconds before the object is destroyed

    public void DestroyObject(GameObject obj)
    {
        objectToDestroy = obj;
        if (objectToDestroy != null)
        {
            StartCoroutine(DestroyObjectAfterDelay());
        }
        else
        {
            Debug.LogWarning("No object assigned to destroy!");
        }
    }

    private IEnumerator DestroyObjectAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(objectToDestroy);
        Debug.Log("Object destroyed: " + objectToDestroy.name);
    }
}
