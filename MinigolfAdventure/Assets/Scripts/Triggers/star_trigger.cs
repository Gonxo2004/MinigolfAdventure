using UnityEngine;

public class StarTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Intentar obtener el script BallController del objeto que entra en el Trigger
        BallController ballController = other.GetComponent<BallController>();

        // Verificar si se obtuvo el BallController
        if (ballController != null)
        {
            // Llamar a la función star_collected del BallController
            ballController.star_collected();

            // Destruir este GameObject al que pertenece este script
            Destroy(gameObject);
        }
    }
}
