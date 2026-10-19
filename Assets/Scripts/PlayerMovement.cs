using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// BELLINGHAM-15 (HU-02): movimiento independiente de Lava.
/// Mismo script que usa Hielo (HU-01): las teclas se configuran en el Inspector.
/// Valores por defecto = controles de Lava (flechas).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Controles (configurables)")]
    public Key leftKey = Key.LeftArrow;
    public Key rightKey = Key.RightArrow;
    public Key jumpKey = Key.UpArrow;

    [Header("Movimiento")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;

    [Header("Deteccion de suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float dir = 0f;
        if (kb[leftKey].isPressed) dir -= 1f;
        if (kb[rightKey].isPressed) dir += 1f;
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        if (kb[jumpKey].wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
