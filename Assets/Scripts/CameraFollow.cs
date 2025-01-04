using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(10, 10, 0); 
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null) return;

        // Detecta la entrada de las teclas WASD para rotar la cámara
        if (Input.GetKey(KeyCode.W))
        {
            transform.RotateAround(target.position, Vector3.right, -90 * Time.deltaTime); // Rota hacia arriba
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.RotateAround(target.position, Vector3.right, 90 * Time.deltaTime); // Rota hacia abajo
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(target.position, Vector3.up, -90 * Time.deltaTime); // Rota hacia la izquierda
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(target.position, Vector3.up, 90 * Time.deltaTime); // Rota hacia la derecha
        }

        // Calcula la nueva posición de la cámara basada en el offset
        Vector3 direction = (transform.position - target.position).normalized; // Direccion desde el target
        transform.position = target.position + direction * offset.magnitude; // Reaplica la distancia del offset

        // Mantén la cámara mirando al objetivo
        transform.LookAt(target);
    }
}
