﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public PumpingGauge pumpingGauge;

    //게임 중 사망 시 다시 시작 버튼
    public GameObject UIReStart;

    //플레이어 이동
    public float moveSpeed;
    public float jumpPower;
    public float maxPumping;
    float maxJump = 2;
    int jumpCount = 0;
    int pumpingCount = 0;

    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;

    Vector3 previousPosition = new Vector3();

    public enum LayerName
    {
        BasicLayer = 0,
        DeathLayer = 1
    }

    public void Start()
    {
        gameManager.energyBar.value = 10f;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (this.transform.position.y < this.previousPosition.y)
        {
            anim.SetBool("isJumpUp", false);
        }

        this.previousPosition = this.transform.position;

        if(gameManager.energyBar.value <= 0.5f)
        {
            onDie();
        }
        else
        {
            gameManager.energyBar.value = Mathf.MoveTowards(gameManager.energyBar.value, 10f, Time.deltaTime * 1f);
        }

        if(gameManager.energyBar.value >= 0.5f && IsAlive) 
        {
            Jump();

            Pumping();

            //Stop Speed
            if (Input.GetButtonUp("Horizontal"))
            {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
            }

            if (Input.GetButton("Horizontal"))
            {
                spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
            }
        }

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.3f)
        {
            anim.SetBool("isRun", false);
        }
        else
        {
            anim.SetBool("isRun", true);
        }
    }

    private void FixedUpdate()
    {
        if(gameManager.energyBar.value >= 0.5f)
        {
            //Move Speed
            float h = Input.GetAxisRaw("Horizontal");

            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            rigid.velocity = new Vector2(h * moveSpeed, rigid.velocity.y);
        }

        //Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                {
                    anim.SetBool("isJumpUp", false);
                    anim.SetBool("isJumpDown", false);
                    jumpCount = 0;
                }
            }

            Debug.DrawRay(rigid.position, Vector3.forward, new Color(0, 1, 0));

            RaycastHit2D rayHit2 = Physics2D.Raycast(rigid.position, Vector3.forward, 1, LayerMask.GetMask("Enemy"));

            if (rayHit2.collider != null)
            {
                if (rayHit2.distance < 0.5f)
                {
                    Debug.Log("Ray가 함정에 닿았다!!");
                    onDamaged(capsuleCollider.transform.position, 1);
                }
            }
        }
    }

    public bool IsAlive
    {
        get
        {
            return gameManager.energyBar.value > 0;
        }
    }

    private void Jump()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && jumpCount < 2 && gameManager.energyBar.value >= 1f)
        {
            if (jumpCount < maxJump)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
                anim.SetBool("isJumpUp", true);
                anim.SetBool("isJumpDown", true);
                gameManager.energyBar.value--;
                jumpCount++;
            }
        }
    }

    private void Pumping()
    {
        //Pumping Charging Down
        if (Input.GetKey(KeyCode.UpArrow) && gameManager.pumpingGauge < 1f && gameManager.energyBar.value >= 1f)
        {
            gameManager.pumping.SetActive(true);

            if (pumpingCount <= maxPumping)
            {
                pumpingCount += 3;
                //gameManager.pumpingBar.value = Mathf.MoveTowards(gameManager.pumpingBar.value, 1f, Time.deltaTime);
                gameManager.pumpingGauge = Mathf.MoveTowards(gameManager.pumpingGauge, 1f, Time.deltaTime);
            }
        }

        //Pumping Charging Up
        if (Input.GetKeyUp(KeyCode.UpArrow) && gameManager.energyBar.value >= 1f)
        {
            if (pumpingCount >= 10 && gameManager.pumpingGauge >= 0.2)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower * pumpingCount / 100);
                anim.SetBool("isJumpUp", true);
                anim.SetBool("isJumpDown", true);
                gameManager.energyBar.value -= 3;
            }
            pumpingCount = 0;
            gameManager.pumpingBar.value = 0;
            gameManager.pumpingGauge = 0;

            gameManager.pumping.SetActive(false);
            gameManager.pumping.GetComponent<Image>().sprite = pumpingGauge.emptySprite;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /* if (collision.gameObject.tag == "Enemy")
        {
            //Attack
            /* if ((rigid.velocity.y < 0) && (transform.position.y > collision.transform.position.y))
            {
                //onAttack(collision.transform);
            }
        } */

        if(collision.gameObject.tag == "Platform")
        {
            anim.SetBool("isJumpUp", false);
        }

        if(collision.gameObject.tag == "MineTrap")
        {
            onDamaged(collision.transform.position, 5);
        }

        if(collision.gameObject.tag == "FootTrap")
        {
            this.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y);

            moveSpeed = 0;

            Invoke("MoveOn", 1.1f);

            VelocityZero();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
         if (collision.gameObject.tag == "FenceTrap")
        {
            onDamaged(collision.transform.position, 1);
        }
    }

    private void MoveOn()
    {
        moveSpeed = 10;
    }

    private void onDamaged(Vector2 targetPos, int what)
    {
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;

        if (what == 1)
        {
            //Change Layer (Immortal Active)
            gameObject.layer = 11;

            //View Alpha
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);

            //Reaction Force
            rigid.AddForce(new Vector2(dirc, 0.5f) * 0.5f, ForceMode2D.Impulse);
        }
        else if(what == 5)
        {
            //Reaction Force
            rigid.AddForce(new Vector2(dirc, 3f) * 5f, ForceMode2D.Impulse);
        }

        //Animation
        anim.SetTrigger("doDamaged");

        Invoke("offDamaged", 0.5f);
    }

    void offDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void onDie()
    {
        gameManager.energyBar.value = 0f;

        UIReStart.SetActive(true);

        anim.Play("Died");

        gameObject.layer = 11;

        Invoke("VelocityZero", 1);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            //Next Stage
            gameManager.NextStage();
        }
    }
}