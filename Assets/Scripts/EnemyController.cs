using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Rigidbody2D myBody;

    public Transform playerPos;
    private Animator anim;

    [Header("Movement")]
    public float moveSpeed;
    private float maxX;
    private float minX;
    public float distance;
    public int direction = 1;

    private bool patrol, detect = false;

    [Header("Attack")]
    public Transform attackPos;
    public float attackRange;
    public LayerMask playerLayer;
    public int damage;

    public AudioClip axeSwing;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerPos = GameObject.Find("Assasin").transform;
        myBody = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
        maxX = transform.position.x + distance / 2;
        minX = maxX - distance;
	}
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(transform.position, playerPos.position) <= 3.5f)
        {
            patrol = false;
        }
        else {
            patrol = true;
        }
	}

    private void FixedUpdate()
    {
        if (anim.GetBool("Death"))
        {
            myBody.velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
            myBody.isKinematic = true;
            return;
        }

        if (transform.position.y <= -4)
        {
            gameObject.tag = "Untagged";
            anim.SetBool("Death", true);
            return;
        }

        // Check flip skeleton
        if (myBody.velocity.x > 0)
        {
            transform.localScale = new Vector2(1.3f, transform.localScale.y);
            anim.SetBool("Attack", false);
        }
        else if (myBody.velocity.x < 0)
        {
            transform.localScale = new Vector2(-1.3f, transform.localScale.y);
        }


        if (patrol)
        {
            detect = false;
            switch (direction)
            {
                case -1:
                    // If my position is inside the range, update position, else change direction
                    if (transform.position.x > minX)
                    {
                        myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
                    }
                    else
                    {
                        direction = 1;
                    }
                    break;
                case 1:
                    if (transform.position.x <= maxX)
                    {
                        myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
                    }
                    else
                    {
                        direction = -1;
                    }
                    break;
            }
        }
        else {
            if (Vector2.Distance(playerPos.position, transform.position) >= 0.25)
            {
                if (!detect)
                {
                    detect = true;
                    anim.SetTrigger("Detect");
                    myBody.velocity = new Vector2(0, myBody.velocity.y);
                }

                if (anim.GetCurrentAnimatorStateInfo(0).IsName("SkeletonDetect")) {
                    return;
                }

                Vector3 playerDir = (playerPos.position - transform.position).normalized;

                if (playerDir.x > 0)
                {
                    myBody.velocity = new Vector2(moveSpeed + 0.3f, myBody.velocity.y);
                }
                else
                {
                    myBody.velocity = new Vector2(-(moveSpeed + 0.3f), myBody.velocity.y);
                }
            }
            else {
                if (Vector2.Distance(playerPos.position, transform.position) <= 0.2f) {
                    myBody.velocity = new Vector2(0, myBody.velocity.y);
                    anim.SetBool("Attack", true);
                }
            }
        }

    }

    public void Attack()
    {
        myBody.velocity = new Vector2(0, myBody.velocity.y);
        SoundManager.instance.PlaySoundFx(axeSwing, 0.3f);

        Collider2D attackPlayer = Physics2D.OverlapCircle(myBody.position, attackRange, playerLayer);

        if (attackPlayer != null) {
            if (attackPlayer.tag == "Player") {
                attackPlayer.GetComponent<PlayerHealth>().takeDamage(damage);
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
