using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public GameObject miniMap;
    public GameObject miniMapCamera;

    //cameraMove
    Transform caMove;

    //cameraSpeed
    public int caSpd;

    private float x;
    private float y;

    void Start()
    {
        caMove = miniMapCamera.transform;
        x = miniMapCamera.transform.position.x;
        y = miniMapCamera.transform.position.y;
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
                caMove.Translate(-(caSpd), 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                caMove.Translate(caSpd, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                caMove.Translate(0, caSpd, 0);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                caMove.Translate(0, (-caSpd), 0);
            }
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            caMove.position = new Vector3(x, y, -10);
        }
    }
}
