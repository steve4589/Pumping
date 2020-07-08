using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class Player : MonoBehaviour
{
    public GameManager gameManager;

    //플레이어 에너지
    public Slider energyBar;

    //플레이어 펌핑(차징)
    public GameObject pumping;
    public Slider pumpingBar;

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

    public void Start()
    {
        energyBar.value = 10;
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
        energyBar.value = Mathf.MoveTowards(energyBar.value, 10f, Time.deltaTime * 1f);

        //Jump
        if (Input.GetButtonDown("Jump") && jumpCount < 2 && energyBar.value >= 1f)
        {
            if(jumpCount < maxJump)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
                anim.SetBool("isJump", true);
                jumpCount++;
                energyBar.value--;
            }
        }

        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Direction Sprite
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.3f)
        {
            anim.SetBool("isWalk", false);
        }
        else
        {
            anim.SetBool("isWalk", true);
        }

        //Pumping Charging Down
        if (Input.GetKey(KeyCode.UpArrow) && pumpingBar.value < 1f && energyBar.value >= 1f)
        {
            pumping.SetActive(true);

            if (pumpingCount <= maxPumping)
            {
                pumpingCount+=3;
                pumpingBar.value = Mathf.MoveTowards(pumpingBar.value, 1f, Time.deltaTime);
            }
        }

        //Pumping Charging Up
        if (Input.GetKeyUp(KeyCode.UpArrow) && energyBar.value >= 1f)
        {
            if (pumpingCount >= 10 && pumpingBar.value >= 0.2)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower * pumpingCount / 100);
                anim.SetBool("isJump", true);
                energyBar.value -= 3;
            }
            pumpingCount = 0;
            pumpingBar.value = 0;

            pumping.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        rigid.velocity = new Vector2(h * moveSpeed, rigid.velocity.y);

        //MaxSpeed
        /* if(rigid.velocity.x > moveSpeed) // Right
        {
            rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < moveSpeed * (-1)) // Left
        {
            rigid.velocity = new Vector2(moveSpeed * (-1), rigid.velocity.y);
        } */

        //Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                {
                    anim.SetBool("isJump", false);
                    jumpCount = 0;
                }
            }

            Debug.DrawRay(rigid.position, Vector3.forward, new Color(0, 1, 0));

            RaycastHit2D rayHit2 = Physics2D.Raycast(rigid.position, Vector3.forward * 50f, 1, LayerMask.GetMask("Enemy"));

            if (rayHit2.collider != null)
            {
                if (rayHit2.distance < 0.5f)
                {
                    Debug.Log("Ray가 몬스터에게 닿았다!!");
                }
            }
        }
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
