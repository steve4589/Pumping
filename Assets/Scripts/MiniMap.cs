using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject miniMap;
    public GameObject miniMapCamera;

    //cameraMove
    Transform caMove;

    //cameraSpeed
    public int cameraSpeed;

    void Start()
    {
        caMove = miniMapCamera.transform;
    }

    void Update()
    {
        //만약 'T'를 누르면 미니맵 On/Off
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (miniMap.activeSelf)
            {   
                miniMap.SetActive(false);
            }
            else
            {
                miniMap.SetActive(true);
            }
        }

        if (miniMap.activeSelf)
        {
            //caMove.position = Vector3.Lerp(caMove.position, caMove.position, 2f * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.J))
            {
                caMove.Translate(-(cameraSpeed), 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                caMove.Translate(cameraSpeed, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                caMove.Translate(0, cameraSpeed, 0);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                caMove.Translate(0, (-cameraSpeed), 0);
            }
        }

        float x = gameManager.player.transform.position.x;
        float y = gameManager.player.transform.position.y;

        if (Input.GetKeyUp(KeyCode.T))
        {
            caMove.position = new Vector3(x, y + 5, -10);
        }
    }
}
