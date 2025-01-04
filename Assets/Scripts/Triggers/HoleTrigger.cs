using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BallController ballController = other.GetComponent<BallController>();

        if (ballController != null)
        {
            ballController.OnHoleEntered();
        }
    }
}
