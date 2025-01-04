using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BallController : MonoBehaviour
{
    [Header("Tipo de Control")]
    public bool isAI = false;          // ¿Esta bola pertenece a la IA?
    public bool isIdle = true;         // Indica si está en reposo
    public bool isShooting = false;    // Se pone a true cuando va a disparar

    [Header("Coordenadas del hoyo (solo IA)")]
    [Tooltip("Asigna manualmente la posición del hoyo en el espacio. La IA disparará hacia estas coords.")]
    public Vector3 holeCoords;

    [Header("Componentes")]
    public Rigidbody rb;
    private Camera mainCamera;

    [Header("UI (solo para el jugador)")]
    public TMP_Text displayText;
    public TMP_Text hole_message;
    public TMP_Text PB_message;
    public Button nextLevelButton;
    public Button exitButton;
    public Image star_not_collected;
    public Image star_collected_img;

    [Header("Parámetros de fuerza/movimiento")]
    public float stopVelocity = 0.1f;   // Velocidad mínima para considerar la bola parada
    public float shotPower = 10f;       // Multiplicador de fuerza
    public float maxPower = 20f;        // Fuerza máxima
    public float baseAccuracy = 0.9f;   // Precisión base de la IA (0.0 a 1.0)

    // Variables internas
    private bool isAiming = false;      // El jugador está apuntando con el ratón
    private Vector3? worldPoint;        // Punto de impacto en el suelo (jugador)
    private Vector3 pastPosition;       // Posición previa (si sale de límites)
    private int currentPar = 0;

    void Awake()
    {
        rb.maxAngularVelocity = 1000;

        // Si NO es IA, buscamos la cámara principal para apuntar con el ratón
        if (!isAI)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        // --- Lógica solo para el jugador ---
        if (!isAI)
        {
            // Si la bola está prácticamente parada y en reposo...
            if (rb.velocity.magnitude < stopVelocity && isIdle)
            {
                ProcessAim();

                // Empezar a apuntar cuando se pulsa el botón
                if (Input.GetMouseButtonDown(0))
                {
                    isAiming = true;
                }
                // Disparar cuando se suelta el botón
                if (Input.GetMouseButtonUp(0) && isAiming && worldPoint.HasValue)
                {
                    isShooting = true;
                    isAiming = false;
                }
            }
            else if (rb.velocity.magnitude >= stopVelocity)
            {
                isIdle = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isAI)
        {
            // Si la bola está parada pero no en reposo, detenerla
            if (rb.velocity.magnitude < stopVelocity && !isIdle)
            {
                StopBall();
            }

            // Realizar disparo del jugador
            if (isShooting && worldPoint.HasValue)
            {
                PlayerShoot(worldPoint.Value);
                isShooting = false;
            }
        }
    }

    // --------------------------------------------------------------------
    //                    MÉTODOS PÚBLICOS PARA EL GAMECONTROLLER
    // --------------------------------------------------------------------

    public void TakeShotAI()
    {
        Debug.Log("Disparo IA ejecutado");  // Verifica si este mensaje aparece
        pastPosition = transform.position;
        Vector3 direction = (holeCoords - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, holeCoords);
        float force = distance * shotPower;

        float factorError = UnityEngine.Random.Range(baseAccuracy, 1.1f);
        force *= factorError;

        force = Mathf.Min(force, maxPower);
        rb.AddForce(direction * force, ForceMode.Impulse);

        isIdle = false;
        currentPar++;
    }


    public bool IsStopped()
    {
        return rb.velocity.magnitude < stopVelocity;
    }

    public void StopBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isIdle = true;
    }

    public void OutOfBounds()
    {
        rb.MovePosition(pastPosition);
        StopBall();
        isIdle = false;
    }

    public void OnHoleEntered()
    {
        StopBall();
        if (!isAI)
        {
            displayText.enabled = false;
            hole_message.text = GetHoleMessage(currentPar);
            hole_message.enabled = true;
            nextLevelButton.gameObject.SetActive(true);
            exitButton.gameObject.SetActive(true);
        }
        currentPar = 0;
    }

    public void star_collected()
    {
        if (star_collected_img != null)
            star_collected_img.gameObject.SetActive(true);

        if (star_not_collected != null)
            star_not_collected.gameObject.SetActive(false);

        currentPar--;
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
            default: return "¡Better luck next time!";
        }
    }

    private void ProcessAim()
    {
        if (!isAiming) return;
        worldPoint = CastMouseClickRay();
    }

    private Vector3? CastMouseClickRay()
    {
        if (mainCamera == null) return null;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return null;
    }

    private void PlayerShoot(Vector3 point)
    {
        pastPosition = transform.position;
        Vector3 direction = (point - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, point);
        strength = Mathf.Min(strength, maxPower);
        rb.AddForce(direction * strength * shotPower, ForceMode.Impulse);
        isIdle = false;
        currentPar++;
    }
}
