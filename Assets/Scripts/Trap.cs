using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class Trap : MonoBehaviour
{
    public GameObject trap;

    public Slider energyBar;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.tag == "MineTrap")
        {
            energyBar.value -= 5;

            anim.SetTrigger("doTouch");

            Destroy(trap, 0.5f);
        }
        else if(collision.gameObject.tag == "Player" && gameObject.tag == "FootTrap")
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            energyBar.value -= 2;

            anim.SetTrigger("doTouch");

            Destroy(trap, 1.1f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && gameObject.tag == "FenceTrap")
        {
            doDamage();
        }
    }

    private void doDamage()
    {
        gameObject.layer = 14;

        //EnergyDown
        energyBar.value -= 1;

        Invoke("offDamage", 0.5f);
    }

    private void offDamage()
    {
        gameObject.layer = 13;
    }
}