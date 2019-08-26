using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float runSpeed, jumpForce;
    [Range(0, 1)]
    public float jumpHeight;

    private float moveInput;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator anim;

    private bool faceRight = true;

    private Rigidbody2D myBody;

    public Vector3 range;

    public AudioClip[] footSteps;
    public AudioClip jumpSound;

    // Use this for initialization
    void Awake () {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}

    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
        if (anim.GetBool("SwordAttack"))
        {
            moveInput = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        Movement();
        CheckCollitionForJump();
	}

    void CheckCollitionForJump() {
        Collider2D bottomhit = Physics2D.OverlapBox(groundCheck.position, range, 0, groundLayer);

        if (bottomhit != null) {
            if (bottomhit.gameObject.tag == "Ground" && Input.GetKeyDown(KeyCode.Z))
            {
                myBody.velocity = new Vector2(myBody.velocity.x, jumpForce);
                SoundManager.instance.PlaySoundFx(jumpSound, Random.Range(0.2f, 0.4f));
                anim.SetBool("Jump", true);
            }
            else {
                anim.SetBool("Jump", false);
            }
        }
    }

    void Movement() {
        moveInput = Input.GetAxisRaw("Horizontal") * runSpeed;

        myBody.velocity = new Vector2(moveInput, myBody.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        if (Input.GetKeyUp(KeyCode.Z))
        {
            if (myBody.velocity.y > 0)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, myBody.velocity.y * jumpHeight);
                
            }
        }

        if (moveInput > 0 && !faceRight || moveInput < 0 && faceRight)
        {
            Flip();
        }
    }

    void Flip() {
        faceRight = !faceRight;

        Vector3 transformScale = transform.localScale;
        transformScale.x *= -1;
        transform.localScale = transformScale;
    }

    void RunningSound()
    {
        SoundManager.instance.PlayRandomSoundFX(footSteps);
    }
}
