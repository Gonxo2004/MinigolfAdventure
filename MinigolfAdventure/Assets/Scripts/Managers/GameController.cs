using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Referencias a las bolas")]
    public BallController playerBall;         // Bola del primer jugador (humano)
    public SecondBallController secondBall;   // Bola del segundo jugador (IA)

    [Header("Turnos")]
    public int currentPlayer = 1;

    private void Awake()
    {
        // Asegurarnos de que la bola de la IA conozca este GameManager
        if (secondBall != null)
        {
            secondBall.gameController = this;
        }
    }

    /// Cambia el turno entre el jugador 1 y la IA (2).
    public void NextTurn()
    {
        if (currentPlayer == 1)
            currentPlayer = 2;
        else
            currentPlayer = 1;

        Debug.Log("Ahora es el turno del jugador: " + currentPlayer);
    }
}
