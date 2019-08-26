using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public float speed = 0.5f;
    public int damage = 50;
    private bool right;

    public AudioClip connectFX;

    // Check direction of arrow (left or right)
    void Start () {
        if (GameObject.FindGameObjectWithTag("Player").transform.localScale.x > 0)
        {
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
            right = true;
        }
        else
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            right = false;
        }

        Destroy(gameObject,6);
	}
	
	// Update movement every frame
	void Update () {
        if (right)
        {
            transform.Translate(Vector2.right * speed);
        }
        else
        {
            transform.Translate(Vector2.left * speed);
        }
	}

    // When collide with an enemy
    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Enemy")
        {
            Destroy(gameObject);
            target.GetComponent<EnemyHealth>().takeDamage(damage);
            SoundManager.instance.PlaySoundFx(connectFX, 0.3f);
        }
    }
}
