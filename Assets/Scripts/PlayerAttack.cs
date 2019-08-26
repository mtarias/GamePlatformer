using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private Animator anim;
    private bool activeTimeToReset;
    private float defaultComboTimer = 0.2f, currentComboTimer;

    private int combo; // 1, 2 or 3 depending of the combo
    private int arrowCount;
    public float cooldown;

    public Transform attackPos;
    public LayerMask enemyLayer;

    public float attackRange;
    public int damage;

    public GameObject arrow;
    public Transform arrowPos;
    private bool canShoot;

    public AudioClip pullShoot, swordHit;
    public AudioClip[] swordClips;

	// Use this for initialization
	void Awake () {
        anim = GetComponent<Animator>();
        arrowCount = 20;
	}
	
	// Update is called once per frame
	void Update () {
        coolDownTimer();
        SwordAttack();
        resetComboState();
        BowAttack();
    }

    void BowAttack() {
        if (Input.GetKeyDown(KeyCode.C) && canShoot)
        {

            if (arrowCount > 0)
                anim.SetTrigger("Shoot");

            canShoot = false;
            arrowCount--;
        }
    }

    void ArrowSpawn() {
        Instantiate(arrow, arrowPos.position, Quaternion.identity);
        SoundManager.instance.PlaySoundFx(pullShoot, 0.3f);
    }

    void SwordAttack() {

        if (Input.GetKeyUp(KeyCode.X) && !anim.GetBool("Jump"))
        {
            if (combo < 3)
            {
                anim.SetBool("SwordAttack", true);
                currentComboTimer = defaultComboTimer;
                activeTimeToReset = true;

                attack(0, combo);
            }
            else
            {
                anim.SetBool("SwordAttack", false);
            }
        }
        else if (Input.GetKeyUp(KeyCode.X) && anim.GetBool("Jump"))
        {
            if (cooldown == 0)
            {
                anim.SetBool("AirAttack", true);
                cooldown = 1;

                attack(10, 0);
            }
        }
    }

    // Add to combo any time that call SwordAttack
    void increaseComboNumber() {
        combo++;
    }

    // Reset combo when use another action (Jump, Run, etc)
    void resetCombo() {
        combo = 0;
        canShoot = true;
    }

    // Check time while combo is activated
    void resetComboState() {
        if (activeTimeToReset) {
            // Decrease time while animation is activated
            currentComboTimer -= Time.deltaTime;
            //print(currentComboTimer);
            // If attack yime end, reset attack
            if (currentComboTimer < 0) {
                activeTimeToReset = false;
                anim.SetBool("SwordAttack", false);
                currentComboTimer = defaultComboTimer;
            }
        }
    }

    void coolDownTimer() {
        if (cooldown > 0)
        {
            anim.SetBool("AirAttack", false);
            cooldown -= Time.deltaTime;

            if (cooldown < 0) cooldown = 0;
        }
    }

    private void attack(int aditionalDamage, int combo)
    {
        //Attack
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemyLayer);

        // Attack all enemies
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<EnemyHealth>().health > 0)
            {
                enemies[i].GetComponent<EnemyHealth>().takeDamage(damage + aditionalDamage);
                SoundManager.instance.PlaySoundFx(swordHit, Random.Range(0.2f, 0.4f));
            }
        }

        SoundManager.instance.PlaySoundFx(swordClips[combo], Random.Range(0.2f, 0.4f));
    }
}