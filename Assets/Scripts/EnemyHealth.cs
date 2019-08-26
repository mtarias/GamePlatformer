using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float health = 300;
    private Animator anim;

	// Use this for initialization
	void Awake () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public void takeDamage (int damage) {

        health -= damage;

        if (health <= 0)
        {
            gameObject.tag = "Untagged";
            anim.SetBool("Death", true);
        }

        anim.SetTrigger("Hit");
	}
}
