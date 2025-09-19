using UnityEngine;
using System.Collections;

public class MatrixCubeRotation : MonoBehaviour
{
    public float rotationSpeed = 30f; // Degrees/second
    private bool isRotating = true;

    private Mesh originalMesh;
    private Vector3[] originalVertices;
    private Vector3[] currentVertices;
    public MeshFilter meshFilter;

    void Start()
    {
        // Store original mesh data
        originalMesh = meshFilter.mesh;
        originalVertices = originalMesh.vertices;
        currentVertices = new Vector3[originalVertices.Length];
        originalVertices.CopyTo(currentVertices, 0);
    }

    void Update()
    {
        // Toggle rotation with space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRotating = !isRotating;
        }

        // Apply rotation if enabled
        if (isRotating)
        {
            // Calculate rotation angle in radians
            float angle = rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;

            // Create rotation matrix around Y axis
            Matrix4x4 rotationMatrix = CreateRotationMatrixY(angle);

            // Apply rotation matrix to each vertex
            for (int i = 0; i < originalVertices.Length; i++)
            {
                currentVertices[i] = MultiplyPoint3x4(rotationMatrix, originalVertices[i]);
            }

            // Update mesh vertices
            meshFilter.mesh.vertices = currentVertices;

            // Recalculate normals and bounds
            meshFilter.mesh.RecalculateNormals();
            meshFilter.mesh.RecalculateBounds();

            // Copy transformed vertices back to original for next rotation
            currentVertices.CopyTo(originalVertices, 0);
        }
    }

    // Create a rotation matrix around the Y axis
    Matrix4x4 CreateRotationMatrixY(float angle)
    {
        Matrix4x4 matrix = new Matrix4x4();

        matrix.m00 = Mathf.Cos(angle);  // cos(θ)
        matrix.m01 = 0f;
        matrix.m02 = Mathf.Sin(angle);  // sin(θ)
        matrix.m03 = 0f;

        matrix.m10 = 0f;
        matrix.m11 = 1f;                // No change on Y axis
        matrix.m12 = 0f;
        matrix.m13 = 0f;

        matrix.m20 = -Mathf.Sin(angle); // -sin(θ)
        matrix.m21 = 0f;
        matrix.m22 = Mathf.Cos(angle);  // cos(θ)
        matrix.m23 = 0f;

        matrix.m30 = 0f;
        matrix.m31 = 0f;
        matrix.m32 = 0f;
        matrix.m33 = 1f;

        return matrix;
    }

    // Multiply a 3D point by a 4x4 matrix
    Vector3 MultiplyPoint3x4(Matrix4x4 matrix, Vector3 point)
    {
        Vector3 result;

        result.x = matrix.m00 * point.x + matrix.m01 * point.y + matrix.m02 * point.z + matrix.m03;
        result.y = matrix.m10 * point.x + matrix.m11 * point.y + matrix.m12 * point.z + matrix.m13;
        result.z = matrix.m20 * point.x + matrix.m21 * point.y + matrix.m22 * point.z + matrix.m23;

        return result;
    }

    // Display instructions in the GUI
    /*void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 30), "Press SPACE to toggle rotation");
        GUI.Label(new Rect(10, 30, 300, 30), "Rotation: " + (isRotating ? "ON" : "OFF"));
    }*/
}