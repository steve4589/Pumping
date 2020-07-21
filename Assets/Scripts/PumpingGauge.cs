using System.Collections;
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
    public Sprite fullSprite;

    Animator anim;

    void Start()
    {
        pumpingImage = GetComponent<Image>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (gameManager.pumpingGauge <= 0.1f)
        {
            //anim.Play("Empty");
            anim.SetBool("isHalf", false);
            anim.SetBool("isFull", false);
            pumpingImage.sprite = emptySprite;
        }
        else if (gameManager.pumpingGauge >= 0.1f && gameManager.pumpingGauge <= 0.6f)
        {
            //anim.Play("Half");
            anim.SetBool("isHalf", true);
            pumpingImage.sprite = halfSprite;
        }
        else if (gameManager.pumpingGauge >= 0.9f)
        {
            //anim.Play("Full");
            anim.SetBool("isFull", true);
            pumpingImage.sprite = fullSprite;
        }
    }
}
