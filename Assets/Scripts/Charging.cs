using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charging : MonoBehaviour
{
    public Slider pumpingBar;
    public float jumpPower;
    public float maxJump;
    public float maxPumping;
    public int jumpCount = 0;
    public int pumpingCount = 0;

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
        if (pumpingBar.value < 1f)
        {
            pumpingBar.value = Mathf.MoveTowards(pumpingBar.value, 1f, Time.deltaTime);
        }

        if (pumpingBar.value >= 1f)
        {
            //loadText.text = "스페이스바를 눌러봐!!";
        }
    }
}
