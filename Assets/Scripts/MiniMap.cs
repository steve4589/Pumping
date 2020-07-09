using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public GameObject miniMap;

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

                if (Input.GetKeyDown(KeyCode.A))
                {

                }
            }
        }
    }
}
