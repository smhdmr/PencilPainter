using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameMaker : MonoBehaviour
{      
    //GAME VARIABLES
    public int point;
    public bool isPlayerAlive = true;

    //LEVEL AREA
    public float startZ;
    public float endZ;

    public bool isVibrationOn;
    public bool isPlayerInAnim = false;



    //SELECTABLE COLORS
    private List<Color> colorList = new List<Color>()
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.magenta
};
    //SELECTED 2 COLOR
    public List<Color> selectedColorList = new List<Color>();

    //INSTANCE
    public static GameMaker Instance;


    void Awake()
    {
        //SET INSTANCE
        Instance = this;

        //INIT VIBRATION
        Vibration.Init();

        //SELECT 2 COLORS FROM THE SELECTABLE COLORS LIST
        selectedColorList.Clear();
        Color firstColor = colorList[Random.Range(0, colorList.Count)];
        selectedColorList.Add(firstColor);
        colorList.Remove(firstColor);
        selectedColorList.Add(colorList[Random.Range(0, colorList.Count)]);
        colorList.Add(firstColor);
    }


    void Update()
    {
        //SET PROGRESS LEVEL
        SetProgress();
    }


    //VIBRATES PHONE
    public void Vibrate()
    {
        if(isVibrationOn)
            Vibration.VibratePop();
    }

    //INCREASES THE POINT VARIABLE and CALLS UIMANAGER JOBS
    public void GetPoint()
    {
        point++;
        UIManager.Instance.GetPoint();
    }    

    //DESTROYS PLAYER
    public void KillPlayer()
    {
        DOTween.KillAll();
        UIManager.Instance.ShowGameOverMenu();
        GameMaker.Instance.isPlayerAlive = false;
        Destroy(Player.Instance.gameObject);
    }

    //STOPS PLAYER MOVEMENT
    public void KillPlayerMovement()
    {
        DOTween.KillAll();
        Player.Instance.isRotationAllowed = false;
        Player.Instance.rb.velocity = Vector3.zero;
        isPlayerAlive = false;
    }


    //SETS THE PROGRESS BAR FILL AMOUNT
    public void SetProgress()
    {
        if (isPlayerAlive)
        {
            float progress;

            if (startZ == 0)
                progress = Player.Instance.transform.position.z / endZ;

            else
                progress = Mathf.Abs((Player.Instance.transform.position.z - startZ) / (endZ - startZ));

            if (progress >= 1)
                progress = 1;
        
            //Debug.Log("Progress: %" + progress * 100);
            UIManager.Instance.SetProgress(progress);
        }        
        
    }


    //PAUSES THE GAME WHEN PLAYER IS NOT FOCUSING TO GAME
    private void OnApplicationFocus(bool isFocused)
    {
        if(!Application.isEditor && !isFocused)
            UIManager.Instance.OnClickPauseButton();
    }


    //REMAP METHOD
    public static float RemapAdvanced(float value, float from1, float to1, float from2, float to2, bool isReverseProportion = false)
    {

        float result;

        switch (isReverseProportion)
        {
            case false:
                result = (to1 - from1) * (to2 - from2);
                result = (value - from1) / result;
                result += from2;
                return result;

            case true:
                result = (to1 - from1) * (to2 - from2);
                result = (value - from1) / result;
                result += from2;
                result = to2 - result + from2;
                return result;            

        }

    }

}
