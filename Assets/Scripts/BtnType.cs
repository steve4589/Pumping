using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum BTNType
{
    New,
    Load,
    Option,
    Sound,
    Bgm,
    Back,
    Main,
    Exit
}

public class BtnType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BTNType currentType;

    public Transform buttonScale;

    Vector3 defaultScale;

    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;

    public Text SoundText;
    public Text BgmText;

    private void Start()
    {
        defaultScale = buttonScale.localScale;
    }

    bool isSound;
    bool isBgm;

    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BTNType.New:
                SceneLoad.LoadSceneHandle("Stage_1", 1);
                break;
            case BTNType.Load:
                SceneLoad.LoadSceneHandle("Stage_1", 2);
                break;
            case BTNType.Option:
                CanvasGroupOn(optionGroup);
                CanvasGroupOff(mainGroup);
                break;
            case BTNType.Sound:
                if (isSound)
                {
                    isSound = false;
                    SoundText.text = "Sound Off";
                }
                else
                {
                    isSound = true;
                    SoundText.text = "Sound On";
                }
                break;
            case BTNType.Bgm:
                if (isBgm)
                {
                    isBgm = false;
                    BgmText.text = "Bgm Off";
                }
                else
                {
                    isBgm = true;
                    BgmText.text = "Bgm On";
                }
                break;
            case BTNType.Back:
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(optionGroup);
                break;
            case BTNType.Main:
                SceneLoad.LoadSceneHandle("Main", 0);
                break;
            case BTNType.Exit:
                Application.Quit();
                break;
        }
    }

    public void CanvasGroupOn(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }
}