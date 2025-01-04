using UnityEngine;

public class GameController : MonoBehaviour
{
    public BallController playerBall;
    public BallController aiBall;

    private bool playerTurn = true;

    void Update()
    {
        if (playerTurn)
        {
            if (playerBall.IsStopped() && !playerBall.isIdle)
            {
                playerBall.StopBall();
                playerTurn = false;
                Invoke(nameof(AI_Turn), 2f);
            }
        }
        else
        {
            if (aiBall.IsStopped() && !aiBall.isIdle)
            {
                aiBall.StopBall();
                playerTurn = true;
            }
        }
    }

    private void AI_Turn()
    {
        Debug.Log("Turno de la IA iniciado");
        aiBall.TakeShotAI();
    }

}
