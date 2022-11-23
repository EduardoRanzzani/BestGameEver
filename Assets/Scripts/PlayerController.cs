using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [SerializeField] private float moveSpeed = 7f;
  [SerializeField] private float jumpForce = 14f;
  [SerializeField] private LayerMask jumpableGround;

  private new Rigidbody2D rigidbody2D;

  private new BoxCollider2D collider2D;

  private SpriteRenderer spriteRenderer;

  private Animator animator;

  private float directionX;

  private enum MovementState { idle, walking, jumping, falling }

  public static PlayerController instance;

  [SerializeField] private AudioSource jumpSoundEffect;
  [SerializeField] private AudioSource deathSoundEffect;

  // Start is called before the first frame update
  void Start()
  {
    rigidbody2D = GetComponent<Rigidbody2D>();
    collider2D = GetComponent<BoxCollider2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    animator = GetComponent<Animator>();
    instance = this;
  }

  // Update is called once per frame
  void Update()
  {
    Move();
    Jump();
    UpdateAnimationState();
  }

  void Move()
  {
    directionX = Input.GetAxis("Horizontal");
    rigidbody2D.velocity = new Vector2(directionX * moveSpeed, rigidbody2D.velocity.y);
  }

  void Jump()
  {
    if (Input.GetButtonDown("Jump") && IsGrounded())
    {
      jumpSoundEffect.Play();
      rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
    }
  }

  private void UpdateAnimationState()
  {
    MovementState state;

    if (directionX > 0f)
    {
      state = MovementState.walking;
      spriteRenderer.flipX = false;
    }
    else if (directionX < 0f)
    {
      state = MovementState.walking;
      spriteRenderer.flipX = true;
    }
    else
    {
      state = MovementState.idle;
    }

    if (rigidbody2D.velocity.y > .1f)
    {
      state = MovementState.jumping;
    }
    else if (rigidbody2D.velocity.y < -.1f)
    {
      state = MovementState.falling;
    }

    animator.SetInteger("state", (int)state);
  }

  private bool IsGrounded()
  {
    return Physics2D.BoxCast(collider2D.bounds.center, collider2D.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Trap"))
    {
      PlayerDie();
    }
  }

  private void PlayerDie()
  {
    deathSoundEffect.Play();
    rigidbody2D.bodyType = RigidbodyType2D.Static;
    animator.SetTrigger("death");
  }

  private void RestartLevel()
  {
    GameController.instance.RestartLevel();
  }
}
