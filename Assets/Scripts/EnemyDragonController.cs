using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDragonController : MonoBehaviour
{
    public float speed, bound;
    private float cooldown;
    private bool up;

    public Animator anim;

    public Transform player;

    public GameObject fireball;
    public Transform fireballPos;

    public AudioClip fireballFX;

    private Rigidbody2D myBody;
    private bool death;

    void Awake()
    {
        anim = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        bound = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (death) return;

        if (Vector2.Distance(player.position, transform.position) < 4 && cooldown == 0)
        {
            cooldown = 2.5f;
            anim.SetTrigger("Attack");
        }
        else if (Vector2.Distance(player.position, transform.position) > 4)
        {
            cooldown = 1.5f;
        }

        Movement();
        cooldownTimer();
    }

    void Attack()
    {
        Instantiate(fireball, fireballPos.position, Quaternion.identity);
        SoundManager.instance.PlaySoundFx(fireballFX, 0.3f);
    }

    void Movement()
    {
        if (up)
        {
            Vector3 pos = transform.position;
            pos.y += speed;
            transform.position = pos;

            if (transform.position.y > bound + 0.13f) up = false;
        }
        else
        {
            Vector3 pos = transform.position;
            pos.y -= speed;
            transform.position = pos;

            if (transform.position.y < bound - 0.13f) up = true;
        }

        if (transform.position.x < player.transform.position.x) transform.localScale = new Vector3(1, 1, 1);
        if (transform.position.x > player.transform.position.x) transform.localScale = new Vector3(-1, 1, 1);
    }

    void cooldownTimer()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        if (cooldown < 0) cooldown = 0;
    }

    void Death()
    {
        myBody.isKinematic = false;
        death = true;
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Ground")
        {
            myBody.isKinematic = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
