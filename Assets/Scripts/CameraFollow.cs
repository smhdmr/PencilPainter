using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //CAMERA FOLLOW OFFSET
    private Vector3 cameraFollowVector = new Vector3(0f, 0f, -15f);

    //PLAYER POSITION
    private Vector3 playerPos;


    //void Awake()
    //{
    //    //GET PLAYER POS
    //    playerPos = Player.Instance.transform.position;
    //    //SET FOLLOW VECTOR
    //    cameraFollowVector.z = Camera.main.transform.position.z - playerPos.z;
    //}

    // Update is called once per frame
    void Update()
    {
        if (GameMaker.Instance.isPlayerAlive)
        {            
            //GET PLAYER POS
            playerPos = Player.Instance.transform.position;
            //SET CAMERA POS
            transform.position = new Vector3(transform.position.x, transform.position.y, playerPos.z + cameraFollowVector.z);
        }        
    }
}
