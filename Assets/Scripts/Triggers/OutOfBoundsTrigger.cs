using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outOfBounds : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BallController ballController = other.GetComponent<BallController>();

        if (ballController != null)
        {
            ballController.OutOfBounds();
        }
    }
}
