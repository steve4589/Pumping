using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int stageIndex;

    public Player playerMv;

    public GameObject menuSet;
    public GameObject player;

    public string gaSc;

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1920, 1080, true);
    }

    public void Start()
    {
        //GameLoad();
    }

    public void NextStage()
    {
        stageIndex++;

        //Calculate Point
        /* totalPoint += stagePoint;
        stagePoint = 0; */

        SceneManager.LoadScene("Stage_2");
    }

    void PlayerReposition()
    {
        playerMv.transform.position = new Vector3(0, 0, -1);
        playerMv.VelocityZero();
    }

    public void Update()
    {
        //Sub Menu
        if (stageIndex!=0&&Input.GetButtonDown("Cancel"))
        {
            if (menuSet.activeSelf)
            {
                menuSet.SetActive(false);
            }
            else
            {
                menuSet.SetActive(true);
            }
        }
    }

    public void GameSave()
    {
        //PlayerPrefs : 간단한 데이터 저장 기능을 지원하는 클래스
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("GameScene", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetString("GaSc", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();

        menuSet.SetActive(false);
    }

    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
        {
            return;
        }

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int gameScene = PlayerPrefs.GetInt("GameScene");
        gaSc = PlayerPrefs.GetString("GaSc");

        SceneManager.LoadScene(gameScene);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
