using UnityEngine;
using System.Collections;

public class SpawnComponents : MonoBehaviour
{
    public GameObject[] componentPrefabs;
    public float radius = 0.5f;
    public float moveDuration = 0.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    SpawnComponentsAround();
                }
            }
        }
    }

    void SpawnComponentsAround()
    {
        int total = componentPrefabs.Length;

        for (int i = 0; i < total; i++)
        {
            if (componentPrefabs[i] != null)
            {
                float angle = i * Mathf.PI * 2f / total;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

                Vector3 startPos = transform.position;
                Vector3 targetPos = transform.position + offset;

                // Rotasi untuk membaringkan objek (misalnya 90 derajat di X)
                Quaternion rotasiBaring = Quaternion.Euler(90f, 0f, 0f);

                // Spawn dan rotasi sesuai orientasi baring
                GameObject obj = Instantiate(componentPrefabs[i], startPos, rotasiBaring);
                StartCoroutine(MoveToPosition(obj.transform, targetPos, moveDuration));
            }
        }
    }

    IEnumerator MoveToPosition(Transform obj, Vector3 target, float duration)
    {
        Vector3 start = obj.position;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            obj.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        obj.position = target;
    }
}
