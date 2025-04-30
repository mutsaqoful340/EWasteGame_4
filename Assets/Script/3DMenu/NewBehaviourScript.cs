using UnityEngine;

public class ApplyTransformToMesh : MonoBehaviour
{
    [ContextMenu("Apply Transform")]
    void ApplyTransform()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null)
        {
            Debug.LogError("No MeshFilter found!");
            return;
        }

        Mesh mesh = mf.sharedMesh;
        Vector3[] vertices = mesh.vertices;

        // Apply transform to each vertex
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = transform.TransformPoint(vertices[i]);
        }

        // Reset transform
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        // Apply the transformed vertices back (convert from world back to local)
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = transform.InverseTransformPoint(vertices[i]);
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        Debug.Log("Transform applied to mesh!");
    }
}
