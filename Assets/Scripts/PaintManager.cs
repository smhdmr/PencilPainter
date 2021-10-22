using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintManager : MonoBehaviour
{
    public GameObject brush;
    public float brushSize;
    public List<GameObject> activeBrushes = new List<GameObject>();

    float currentPenTipY;
    public GameObject frontPenTipPoint;
    public GameObject backPenTipPoint;
    public GameObject currentPenTipPoint;

    private bool isPainting = false;
    public static PaintManager Instance;


    void Awake()
    {
        Instance = this;   
    }    


    public IEnumerator PaintWall(GameObject wall, Color color)
    {        
        Vector3 brushSpawnPoint;        
        GameObject currentBrush;

        currentPenTipY = currentPenTipPoint.transform.position.y;        

        if (!isPainting)
        {
            isPainting = true;

            brushSpawnPoint = new Vector3(0f, currentPenTipY, wall.transform.position.z);
            currentBrush = Pooler.Instance.SpawnFromPool("Brush", brushSpawnPoint, Quaternion.identity);
            currentBrush.GetComponentInChildren<Renderer>().material.color = color;
            activeBrushes.Add(currentBrush);

        }  


        while (isPainting)
        {
            currentPenTipY = currentPenTipPoint.transform.position.y;

            brushSpawnPoint = new Vector3(0f, currentPenTipY, wall.transform.position.z);
            currentBrush = Pooler.Instance.SpawnFromPool("Brush", brushSpawnPoint, Quaternion.identity);            
            currentBrush.GetComponentInChildren<Renderer>().material.color = color;
            activeBrushes.Add(currentBrush);

            yield return new WaitForSeconds(0.01f);

        }
        
    }


    public void StopPainting()
    {
        isPainting = false;
    }  
    
    
    public void ChangeCurrentPenTipPoint()
    {
        if (currentPenTipPoint == frontPenTipPoint)
            currentPenTipPoint = backPenTipPoint;

        else if (currentPenTipPoint == backPenTipPoint)
            currentPenTipPoint = frontPenTipPoint;
    }


    public void ClearActiveBrushes()
    {
        foreach(GameObject brush in activeBrushes)
        {
            brush.SetActive(false);
        }

        activeBrushes.Clear();
    }
        

}
