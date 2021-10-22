using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Wall : MonoBehaviour
{
    //SELECTED COLOR FOR THIS WALL
    public Color wallColor;

    //IS HITTED COLOR CORRECT
    public bool isColorCorrect;

    //MATERIAL OF WALL'S MIDDLE PART
    private Material middlePartMaterial;


    void Start()
    {
        ////SET A RANDOM COLOR FROM THE SELECTED COLOR
        //selectedColor = GameMaker.Instance.selectedColorList[Random.Range(0, GameMaker.Instance.selectedColorList.Count)];

        //transform.GetChild(0).GetComponent<Renderer>().material.color = selectedColor;
        //transform.GetChild(2).GetComponent<Renderer>().material.color = selectedColor;

        //middlePartMaterial = transform.GetChild(1).GetComponent<Renderer>().material;
        //middlePartMaterial.color = selectedColor;
        //middlePartMaterial.SetFloat("_Cutoff", 1f);        
        
        transform.GetChild(0).GetComponent<Renderer>().material.color = wallColor;
        transform.GetChild(2).GetComponent<Renderer>().material.color = wallColor;         
    }


    
    void OnTriggerEnter()
    {
        Player.Instance.SetCollider(false);

        //IF PLAYER HITS THE WALL WITH CORRECT PEN TIP
        if (Player.Instance.frontTipColor == wallColor)
        {
            //DESTROY THE WALL and GET A POINT            
            GameMaker.Instance.GetPoint();
            Player.Instance.currentWall = this.gameObject;
            Player.Instance.EnterDrawingAnim(this.gameObject);
            FindObjectOfType<AudioManager>().Play("PaintWall");
        }

        //IF PLAYER HITS THE WALL WITH WRONG PEN TIP
        else
        {
            //KILL THE PLAYER
            GameMaker.Instance.KillPlayer();
            FindObjectOfType<AudioManager>().Play("GameOver");
        }
    }

    public void FillMiddlePart(float time)
    {
        middlePartMaterial.DOFloat(0f, "_Cutoff", time);
    }
    
}
