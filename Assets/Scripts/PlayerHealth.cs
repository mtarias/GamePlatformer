using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public int health = 100;
    public bool hit = true;

    public GameObject flash;
    public SpriteRenderer playerSpr;
    public Color collideColor, collideColor2;

    public AudioClip axeHit;

    public Slider slider;

    private Animator anim;

    public int potionHealth;

    void Awake()
    {
        anim = GetComponent<Animator>();    
    }

    void Update()
    {
        if (slider.value != health)
            slider.value = health;


        if (health <= 0 || transform.position.y < -4)
        {
            StartCoroutine(PlayerDeath());
        }
    }

    IEnumerator PlayerDeath()
    {
        anim.SetBool("Death", true);
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;
        yield return new WaitForSeconds(2.5f);

        // Restart the level (Stop any scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void takeDamage(int damage)
    {
        if (hit)
        {
            StartCoroutine(PlayerHit());
            health -= damage;
        }
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "FireBall")
        {
            takeDamage(30);
            Destroy(target.gameObject);
        }

        if (target.tag == "Health")
        {
            Destroy(target.gameObject);
            health += potionHealth;
        }
    }

    IEnumerator PlayerFlash()
    {
        for (int i = 0; i < 2; i++)
        {
            playerSpr.color = collideColor;
            yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < 4; i++)
        {
            playerSpr.color = collideColor2;
            yield return new WaitForSeconds(0.1f);

            playerSpr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator PlayerHit()
    {
        // Sound when player hit an Enemy
        SoundManager.instance.PlaySoundFx(axeHit, 0.2f);
        flash.SetActive(true);
        hit = false;
        StartCoroutine(PlayerFlash());
        yield return new WaitForSeconds(1.5f);
        hit = true;
        flash.SetActive(false);
    }
}