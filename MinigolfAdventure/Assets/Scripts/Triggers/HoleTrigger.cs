using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BallController ballController = other.GetComponent<BallController>();
        SecondBallController secondBallController = other.GetComponent<SecondBallController>();

        if (ballController != null)
        {
            ballController.OnHoleEntered();
        }

        if (secondBallController != null)
        {
            secondBallController.OnHoleEntered();
        }
    }
}
