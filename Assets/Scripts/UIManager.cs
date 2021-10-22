using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    //CLICKABLE AREA / FULL AREA
    [Range(0f, 100f)]
    public float clickableAreaRatio;

    //UI ELEMENTS
    public Text pointText;
    public Image progressBar;

    public GameObject gameStartMenu;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject levelFinishMenu;
    public GameObject soundSwitchOn;
    public GameObject soundSwitchOff;
    public GameObject vibrationSwitchOn;
    public GameObject vibrationSwitchOff;

    //INSTANCE
    public static UIManager Instance;

    public bool isGameResume;

    private void Awake()
    {
        //SET INSTANCE
        Instance = this;

        clickableAreaRatio /= 100;
    }


    //PAUSE BUTTON
    public void OnClickPauseButton()
    {
        //FREEZE THE GAME and SHOW PAUSE MENU
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameMaker.Instance.Vibrate();
        isGameResume = false;
    }


    //RESUME BUTTON
    public void OnClickResumeButton()
    {
        //RESUME THE GAME and HIDE PAUSE MENU
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameMaker.Instance.Vibrate();
        isGameResume = true;
    }


    //RESTART BUTTON
    public void OnClickRestartButton()
    {        
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        GameMaker.Instance.Vibrate();
    }


    //INCREASES THE POINT IN THE UI
    public void GetPoint()
    {
        pointText.text = GameMaker.Instance.point.ToString();
    }


    //SETS PROGRESS BAR FILL AMOUNT
    public void SetProgress(float progress)
    {       
       progressBar.fillAmount = progress;
    }

    public void ChangeSoundStat()
    {
        AudioManager.Instance.isMuted = !AudioManager.Instance.isMuted;

        switch(AudioManager.Instance.isMuted)
        {
            case false:
                soundSwitchOff.SetActive(false);
                soundSwitchOn.SetActive(true);
                break;
            case true:
                soundSwitchOn.SetActive(false);
                soundSwitchOff.SetActive(true);
                break;
        }
    }

    public void ChangeVibrationStat()
    {
        GameMaker.Instance.isVibrationOn = !GameMaker.Instance.isVibrationOn;

        switch (GameMaker.Instance.isVibrationOn)
        {
            case false:
                vibrationSwitchOff.SetActive(false);
                vibrationSwitchOn.SetActive(true);
                break;
            case true:
                vibrationSwitchOn.SetActive(false);
                vibrationSwitchOff.SetActive(true);
                break;
        }
    }
    public void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }
    
    public void ShowLevelFinishMenu()
    {
        levelFinishMenu.SetActive(true);
    }
   
    public void GameStartMenu()
    {
        gameStartMenu.SetActive(false);
    }
}
