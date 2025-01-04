using UnityEngine;

public class RotateBlades : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 100f; // Velocidad de rotación en grados por segundo

    void Update()
    {
        // Rotar localmente alrededor del eje Z (ajusta según necesites)
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
