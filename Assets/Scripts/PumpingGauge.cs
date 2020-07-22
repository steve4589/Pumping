﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PumpingGauge : MonoBehaviour
{
    public GameManager gameManager;

    public Image pumpingImage;
    public Sprite emptySprite;
    public Sprite halfSprite;
    public Sprite fullSprite1;
    public Sprite fullSprite2;

    Animator anim;

    private float timer;
    private float waitingTime;

    private int count = 0;

    void Start()
    {
        timer = 0.0f;
        waitingTime = 0.2f;
        pumpingImage = GetComponent<Image>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (gameManager.pumpingGauge < 0.1f)
        {
            //anim.Play("Empty");
            //anim.SetBool("isHalf", false);
            //anim.SetBool("isFull", false);
            //pumpingImage.sprite = emptySprite;
            gameObject.GetComponent<Image>().sprite = emptySprite;
        }
        else if (gameManager.pumpingGauge >= 0.1f && gameManager.pumpingGauge < 0.9f)
        {
            //anim.Play("Half");
            //anim.SetBool("isHalf", true);
            //anim.SetBool("isFull", false);
            //pumpingImage.sprite = halfSprite;
            gameObject.GetComponent<Image>().sprite = halfSprite;
        }
        else if (gameManager.pumpingGauge >= 0.9f)
        {
            timer += Time.deltaTime;

            count++;

            if (timer > waitingTime)
            {
                FullSprite();
                timer = 0;
                count = 0;
            }
        }
    }

    private void FullSprite()
    {
        if (count % 2 != 0)
        {
            gameObject.GetComponent<Image>().sprite = fullSprite1;
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = fullSprite2;
        }
    }
}
