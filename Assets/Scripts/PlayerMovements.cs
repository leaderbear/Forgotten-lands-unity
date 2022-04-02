using UnityEngine;

public class PlayerMovements : MonoBehaviour
{

    #region edit in unity 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float velPower;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float jumpForce;
    #endregion

    #region states
    private bool isJumping;
    private bool isGrounded;
    #endregion

    #region attached in unity 
    public Rigidbody2D rb;
    public Animator animator; 
    public Transform groundCheckLeft;
    public Transform groundCheckRight;
    public SpriteRenderer spriteRenderer;
    #endregion

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter; 


    void FixedUpdate()
    {
        
        float horizontalSpeed = Input.GetAxis("Horizontal");

        #region checks
        isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position);
        if (isGrounded) { 
            coyoteTimeCounter = coyoteTime;
        }
        else { 
            coyoteTimeCounter -= Time.deltaTime;
        }

        #endregion

        #region movements 

        if (coyoteTimeCounter > 0f && Input.GetButton("Jump"))// && isGrounded
        {
            isJumping = true;
            Jump();
        }

        MovePlayer(horizontalSpeed * moveSpeed);

        Flip(horizontalSpeed);
        #endregion

        #region animator 
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        #endregion


    }

    void MovePlayer(float _horizontalMovement)
    {
        float targetSpeed = _horizontalMovement - rb.velocity.x;
        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        rb.AddForce(movement * Vector2.right);
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isJumping = false;
        coyoteTimeCounter = 0f;
    }

    void Flip(float _xSpeed)
    {
        if (_xSpeed > 0.1f && spriteRenderer.flipX != false)
        {
            spriteRenderer.flipX = false;
        }
        else if (_xSpeed < -0.1f && spriteRenderer.flipX != true)
        {
            spriteRenderer.flipX = true;
        }
    }
}
