using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// HU-01 (BELLINGHAM-14): Como jugador de Hielo, quiero moverme y saltar, para avanzar por el nivel.
/// Script de movimiento y salto específico para el personaje de Hielo.
/// 
/// Controles predeterminados para Hielo:
///   - Izquierda: Tecla A
///   - Derecha: Tecla D
///   - Saltar: Tecla W
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoHielo : MonoBehaviour
{
    [Header("Controles de Hielo (Input System)")]
    [Tooltip("Tecla para desplazarse hacia la izquierda")]
    public Key leftKey = Key.A;

    [Tooltip("Tecla para desplazarse hacia la derecha")]
    public Key rightKey = Key.D;

    [Tooltip("Tecla para saltar")]
    public Key jumpKey = Key.W;

    [Header("Parámetros de Movimiento")]
    [Tooltip("Velocidad de movimiento horizontal")]
    public float moveSpeed = 6f;

    [Tooltip("Fuerza aplicada para el salto vertical")]
    public float jumpForce = 12f;

    [Header("Detección de Suelo")]
    [Tooltip("Objeto hijo vacío colocado en los pies del personaje para comprobar el suelo")]
    public Transform groundCheck;

    [Tooltip("Radio de la comprobación de suelo")]
    public float groundCheckRadius = 0.2f;

    [Tooltip("Capa(s) que representan el suelo y plataformas transitables")]
    public LayerMask groundLayer;

    [Header("Visual")]
    [Tooltip("Voltear el sprite automáticamente según la dirección de movimiento")]
    public bool voltearSprite = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private bool isGrounded;

    /// <summary>
    /// Indica si el personaje está actualmente apoyado sobre una superficie de suelo.
    /// </summary>
    public bool EstaEnSuelo => isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        // Evita que el personaje ruede sobre sí mismo al chocar con bordes o paredes
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Si no se asignó un groundCheck en el Inspector, busca un hijo llamado 'GroundCheck'
        if (groundCheck == null)
        {
            Transform hijo = transform.Find("GroundCheck");
            if (hijo != null)
            {
                groundCheck = hijo;
            }
        }
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        // 1. Detección segura de suelo (evita NullReferenceException si no hay groundCheck asignado)
        ActualizarEstadoSuelo();

        // 2. Movimiento horizontal (A / D)
        float dir = 0f;
        if (kb[leftKey].isPressed) dir -= 1f;
        if (kb[rightKey].isPressed) dir += 1f;

        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        // Volteo horizontal del sprite
        if (voltearSprite && spriteRenderer != null)
        {
            if (dir > 0f)
            {
                spriteRenderer.flipX = false;
            }
            else if (dir < 0f)
            {
                spriteRenderer.flipX = true;
            }
        }

        // 3. Salto (W)
        if (kb[jumpKey].wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void ActualizarEstadoSuelo()
    {
        Vector2 checkPos;
        if (groundCheck != null)
        {
            checkPos = groundCheck.position;
        }
        else if (col != null)
        {
            // Fallback: usar el punto inferior del collider del personaje
            checkPos = new Vector2(col.bounds.center.x, col.bounds.min.y);
        }
        else
        {
            // Fallback: usar el Transform con un pequeño desplazamiento hacia abajo
            checkPos = (Vector2)transform.position + Vector2.down * 0.5f;
        }

        LayerMask mascara = groundLayer.value != 0 ? groundLayer : ~LayerMask.GetMask("Ignore Raycast");
        isGrounded = Physics2D.OverlapCircle(checkPos, groundCheckRadius, mascara);
    }

    void OnDrawGizmosSelected()
    {
        Vector2 checkPos;
        if (groundCheck != null)
        {
            checkPos = groundCheck.position;
        }
        else if (GetComponent<Collider2D>() != null)
        {
            var c = GetComponent<Collider2D>();
            checkPos = new Vector2(c.bounds.center.x, c.bounds.min.y);
        }
        else
        {
            checkPos = (Vector2)transform.position + Vector2.down * 0.5f;
        }

        // Color cian característico de Hielo
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(checkPos, groundCheckRadius);
    }
}
