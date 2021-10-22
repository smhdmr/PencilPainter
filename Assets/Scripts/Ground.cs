using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{    
    void OnCollisionStay(Collision collision)
    {
        if (!GameMaker.Instance.isPlayerInAnim)
        {
            //IF PLAYER HITS THE GROUND, STOP THE PLAYER MOVEMENT
            GameMaker.Instance.KillPlayerMovement();
            UIManager.Instance.ShowGameOverMenu();
        }     
        
    }

}
