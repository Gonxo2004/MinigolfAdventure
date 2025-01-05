using UnityEngine;
using UnityEngine.SceneManagement; // Para cambiar de escena

public class GameController : MonoBehaviour
{
    [Header("Turnos")]
    public int currentPlayer = 1;

    // Referencias a las bolas (ya las tienes)
    public BallController playerBall;
    public SecondBallController secondBall;

    // -------------- NUEVO: Para guardar los Par Totales --------------
    private int playerTotalPar = 0;
    private int aiTotalPar = 0;

    // Banderas para saber si cada uno ya terminó el hoyo
    private bool playerDoneThisHole = false;
    private bool aiDoneThisHole = false;
    // -----------------------------------------------------------------

    private void Awake()
    {
        // Asegurarnos de que la IA conozca este GameController
        if (secondBall != null)
        {
            secondBall.gameController = this;
        }

        // (Opcional) evitar que se destruya al cambiar de escena
        DontDestroyOnLoad(gameObject);
    }

    public void NextTurn()
    {
        if (currentPlayer == 1)
            currentPlayer = 2;
        else
            currentPlayer = 1;

        Debug.Log("Ahora es el turno del jugador: " + currentPlayer);
    }

    // Llamado desde OnHoleEntered() en BallController / SecondBallController
    public void PlayerHoled(int playerNumber, int currentPar)
    {
        if (playerNumber == 1)
        {
            // Sumo el par del hoyo al total
            playerTotalPar += currentPar;
            playerDoneThisHole = true;
        }
        else // playerNumber == 2 (IA)
        {
            aiTotalPar += currentPar;
            aiDoneThisHole = true;
        }

        // Si ambos terminaron este hoyo, cambiamos a la escena de resultados
        if (playerDoneThisHole && aiDoneThisHole)
        {
            Debug.Log("Ambos jugadores han terminado el hoyo. Cambiando a ResultsScene...");

            // Reset de flags para el próximo hoyo
            playerDoneThisHole = false;
            aiDoneThisHole = false;

            // Cargar la escena de resultados
            SceneManager.LoadScene("ScoreboardScene");
        }
    }

    // Método público para que la escena de resultados pida los totales
    public int GetPlayerTotalPar()
    {
        return playerTotalPar;
    }

    public int GetAiTotalPar()
    {
        return aiTotalPar;
    }

    // (Opcional) Si quieres resetear al empezar un nuevo hoyo
    public void ResetForNewHole()
    {
        // Por ejemplo, setea currentPlayer a 1, reasigna posiciones de las bolas, etc.
        currentPlayer = 1;
        Debug.Log("Preparando siguiente hoyo...");
    }
}
