using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charging : MonoBehaviour
{
    public Slider progressBar;
    public float jumpPower;
    public float maxJump;
    public float maxPumping;
    int jumpCount = 0;
    int pumpingCount = 0;

    Rigidbody2D rigid;

    Animator anim;

    void Start()
    {
        
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (progressBar.value < 0.9f)
        {
            progressBar.value = Mathf.MoveTowards(progressBar.value, 0.9f, Time.deltaTime);
        }

        if (progressBar.value >= 1f)
        {
            //loadText.text = "스페이스바를 눌러봐!!";
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) && (progressBar.value >= 1f))
        {
            if (pumpingCount >= 100)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower * pumpingCount / 100);
                anim.SetBool("isJump", true);
            }
            pumpingCount = 0;
        }
    }
}
