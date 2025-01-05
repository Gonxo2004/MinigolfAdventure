using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BallController : MonoBehaviour
{
    // ------------------------------------------
    public GameController gameManager;
    public int playerNumber = 1;
    // ------------------------------------------

    // Referencias de Unity
    public Rigidbody rb;
    private Camera mainCamera;

    [Header("UI")]
    public TMP_Text displayText;
    public TMP_Text hole_message;
    public TMP_Text PB_message;
    public Button nextLevelButton;
    public Button exitButton;
    public Image star_not_collected;
    public Image star_collected_img;

    [Header("Gameplay")]
    public float stopVelocity = 0.3f;
    public float shotPower = 10f;
    public float maxPower = 20f;
    public int currentHole = 0;
    public Vector3 pastPosition;

    // Servicios (inyectados o asignados en tiempo de ejecución)
    private IScoreService scoreService;

    // Variables internas
    private string personalBest = "0";
    private int currentPar = 0;
    private bool isAiming = false;
    private bool isIdle = true;
    private bool isShooting = false;
    private Vector3? worldPoint;

    void Awake()
    {
        mainCamera = Camera.main;
        rb.maxAngularVelocity = 1000;

        // Configuración inicial de UI
        if (exitButton != null) exitButton.gameObject.SetActive(false);
        if (nextLevelButton != null) nextLevelButton.gameObject.SetActive(false);
        if (displayText != null) displayText.gameObject.SetActive(true);
        if (PB_message != null) PB_message.gameObject.SetActive(true);
        if (star_not_collected != null) star_not_collected.gameObject.SetActive(true);
        if (star_collected_img != null) star_collected_img.gameObject.SetActive(false);

        // Localizar la factoría en la escena para obtener servicios
        ServicesFactory factory = FindObjectOfType<ServicesFactory>();
        if (factory != null)
        {
            scoreService = factory.GetScoreService();
        }

        // Cargar "Personal Best" 
        if (scoreService != null)
        {
            personalBest = scoreService.GetPersonalBest(currentHole);
        }
        else
        {
            personalBest = "None";
            Debug.LogWarning("ScoreService not found. Setting personalBest to 'None'.");
        }

        if (PB_message != null)
        {
            PB_message.text = (personalBest == "None")
                ? "PB: None"
                : $"personal best: {personalBest}";
        }
    }

    void Update()
    {
        // Si la velocidad es muy baja, podemos apuntar o disparar
        if (rb.velocity.magnitude < stopVelocity)
        {
            if (isIdle)
            {
                ProcessAim();

                if (Input.GetMouseButtonDown(0))
                {
                    isAiming = true;
                }

                if (Input.GetMouseButtonUp(0) && isAiming && worldPoint.HasValue)
                {
                    isShooting = true;
                    isAiming = false;
                }
            }

            // Actualizar texto de PAR
            if (displayText != null)
            {
                displayText.text = $"PAR {currentPar}";
                displayText.enabled = true;
            }
        }
        else
        {
            // Bola en movimiento
            isIdle = false;
        }
    }

    void FixedUpdate()
    {
        // Si se detuvo el movimiento y no está en idle, forzar Stop
        if (rb.velocity.magnitude < stopVelocity && !isIdle)
        {
            Stop();
        }

        if (isShooting && worldPoint.HasValue)
        {
            Shoot(worldPoint.Value);
            isShooting = false;
        }
    }

    private void ProcessAim()
    {
        if (!isAiming) return;
        worldPoint = CastMouseClickRay();
    }

    private Vector3? CastMouseClickRay()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return null;
    }

    public void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isIdle = true;

        // ------------------------------------------------
        // NUEVO: Al detenerse, si este jugador estaba en turno,
        //        se llama a NextTurn() para pasarlo al siguiente jugador.
        if (gameManager != null && gameManager.currentPlayer == playerNumber)
        {
            gameManager.NextTurn();
        }
        // ------------------------------------------------
    }

    private void Shoot(Vector3 point)
    {
        Vector3 horizontalWorldPoint = new Vector3(point.x, transform.position.y, point.z);
        pastPosition = transform.position;

        Vector3 direction = (horizontalWorldPoint - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, horizontalWorldPoint);

        // Limitar fuerza
        strength = Mathf.Min(strength, maxPower);

        rb.AddForce(direction * strength * shotPower, ForceMode.Impulse);
        isIdle = false;
        currentPar++;
    }

    // Cuando la bola sale de límites
    public void OutOfBounds()
    {
        rb.MovePosition(pastPosition);
        Stop();
        isIdle = false;
    }

    // Cuando la bola cae en el hoyo
    public void OnHoleEntered()
    {
        Debug.Log("Player ha terminado...");
        if (displayText != null)
            displayText.enabled = false; // Ocultar PAR

        if (hole_message != null)
        {
            hole_message.enabled = true;
            hole_message.text = GetHoleMessage(currentPar);
        }

        // Actualizar récord personal si es mejor
        if (scoreService != null)
        {
            string newBest = scoreService.UpdatePersonalBest(currentHole, currentPar);
            if (newBest != personalBest)
            {
                personalBest = newBest;
                if (PB_message != null)
                    PB_message.text = $"personal best: {personalBest}";
            }
        }

        // --- AVISAR AL GAME MANAGER QUE ESTE JUGADOR YA ACABÓ ---
        if (gameManager != null)
        {
            gameManager.PlayerHoled(playerNumber, currentPar);
        }
        // --------------------------------------------------------

        currentPar = 0;
        //if (nextLevelButton != null) nextLevelButton.gameObject.SetActive(true);
        //if (exitButton != null) exitButton.gameObject.SetActive(true);
        if (displayText != null) displayText.gameObject.SetActive(false);

        //ResetBallPosition();

        if (gameManager != null && gameManager.currentPlayer == playerNumber)
             gameManager.NextTurn();
    }


    private string GetHoleMessage(int strokes)
    {
        switch (strokes)
        {
            case 1: return "¡Hole in One!";
            case 2: return "¡Eagle!";
            case 3: return "¡Birdie!";
            case 4: return "¡Par!";
            case 5: return "¡Bogey!";
            case 6: return "¡Double Bogey!";
            default: return "¡Better luck next time!";
        }
    }

    private void ResetBallPosition()
    {
        // Desactivar la bola 
        gameObject.SetActive(false);

        if (displayText != null)
        {
            displayText.enabled = true;
            displayText.text = $"Par: {currentPar}";
        }
    }

    // Lógica de estrella (si quieres se puede extraer a otro script)
    public void star_collected()
    {
        if (star_collected_img != null)
            star_collected_img.gameObject.SetActive(true);
        if (star_not_collected != null)
            star_not_collected.gameObject.SetActive(false);

        // Lógica particular de "restar" un hoyo (si así lo deseas)
        currentPar -= 1;
    }
}
