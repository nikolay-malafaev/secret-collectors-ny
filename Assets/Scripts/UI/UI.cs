using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI : UIObjects
{
    private GameManager gameManager;
    private Animator animator;
    private float maxDistanceNumber;
    private bool timePause;
    private float timeRemaining;
    private int timePauseValue;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        dropdown.value = PlayerPrefs.GetInt("qualitySettings");
        gameManager.ChangeQualitySettings(dropdown.value);
        
        if(!PlayerPrefs.HasKey("valueMusic")) PlayerPrefs.SetFloat("valueMusic", 10);
        if(!PlayerPrefs.HasKey("valueSound")) PlayerPrefs.SetFloat("valueSound", 10);
        if(!PlayerPrefs.HasKey("isMusic")) PlayerPrefs.SetInt("isMusic", 1);
        if(!PlayerPrefs.HasKey("isSound")) PlayerPrefs.SetInt("isSound", 1);

        musicSlider.value = PlayerPrefs.GetFloat("valueMusic");
        musicToggle.isOn = PlayerPrefs.GetInt("isMusic") == 1;
        soundSlider.value = PlayerPrefs.GetFloat("valueSound");
        soundToggle.isOn = PlayerPrefs.GetInt("isSound") == 1;
    }

    void Start()
    {
        GameManager.SendTransitionToGame += TransitionToGame;
        GameManager.SendDefeatGame += DefeatGame;
        GameManager.SendResetGame += ResetGame;
        BuffController.SendBuff += Buff;
        animator = GetComponent<Animator>();
        UpdateProgress();
        game.SetActive(gameManager.Test);
        menu.SetActive(!gameManager.Test);
        
        
    }
    private void Update()
    {
        currentDistance.text = gameManager.Distance.ToString();
        if (maxDistanceNumber < gameManager.Distance) maxDistance.text = "new Record";
        currentCountMutagen.text = gameManager.CountMutagen.ToString();

        if (BuffController.IsWorkBuff)
        { 
            timerBar.fillAmount = BuffController.Time;
        }

        if (timePause)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 1f;
                timePauseValue--;
                if (timePauseValue == 0)
                {
                    timePause = false;
                    SendPause();
                    pauseText.gameObject.SetActive(false);
                }
                pauseText.text = timePauseValue.ToString();
            }
        }
    }

    private void Buff(bool value)
    {
        switch (BuffController.CurrenBuff)
        {
            case global::Buff.Options.Null:
                
                break;
            case global::Buff.Options.Durable:
                durable.SetActive(true);
                break;
            case global::Buff.Options.DoubleMutagen:
                doubleMutagen.SetActive(true);
                break;
            case global::Buff.Options.NoGravity:
                noGravity.SetActive(true);
                noGravityButton.SetActive(true);
                break;
        }
        timerBarParent.gameObject.SetActive(value);
        switch (BuffController.LastBuff)
        {
            case global::Buff.Options.Null:
                break;
            case global::Buff.Options.Durable:
                durable.SetActive(false);
                break;
            case global::Buff.Options.DoubleMutagen:
                doubleMutagen.SetActive(false);
                break;
            case global::Buff.Options.NoGravity:
                noGravity.SetActive(false);
                noGravityButton.SetActive(false);
                break;
        }
    }
    public void PauseText()
    {
        timePause = true;
        pauseText.text = "3";
        timePauseValue = 3;
        timeRemaining = 1f;
        pausePanel.SetActive(false);
        buttonPause.SetActive(true);
        pauseText.gameObject.SetActive(true);
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        buttonPause.SetActive(false);
    }
    
    private void SendPause()
    {
        gameManager.OnSendPauseGame();
    }
    private void DefeatGame()
    {
        defeatGame.SetActive(true);
        buttonPause.SetActive(false);
    }
    public void Approval()
    {
        //ApprovalButton.SetActive(!ApprovalButton.activeSelf);
    }
    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        
    }
    private void TransitionToGame()
    {
        menu.SetActive(false);
        game.SetActive(true);
    }
    private void ResetGame()
    {
        game.SetActive(false);
        menu.SetActive(true);
        pausePanel.SetActive(false);
        buttonPause.SetActive(true);
        defeatGame.SetActive(false);
    }
    public void UpdateProgress()
    {
        maxDistanceNumber = PlayerPrefs.GetFloat($"maxDistance");
        maxDistance.text = maxDistanceNumber.ToString();
        maxDistanceMenu.text = maxDistanceNumber.ToString();
        countMutagen.text = PlayerPrefs.GetInt($"countMutagen").ToString();
    }
    
    #region Settings
    public void CheckDropdown()
    {
        gameManager.ChangeQualitySettings(dropdown.value);
    }

    public void CheckSlider(float value, SettingManager.TypeSetting typeSetting)
    {
        switch (typeSetting)
        {
            case SettingManager.TypeSetting.Music:
                PlayerPrefs.SetFloat("valueMusic", value);
                break;
            case SettingManager.TypeSetting.Sound:
                PlayerPrefs.SetFloat("valueSound", value);
                break;
        }
    }
    public void CheckToggle(bool value, SettingManager.TypeSetting typeSetting)
    {
        switch (typeSetting)
        {
            case SettingManager.TypeSetting.Music:
                PlayerPrefs.SetInt("isMusic", value ? 1 : 0);
                break;
            case SettingManager.TypeSetting.Sound:
                PlayerPrefs.SetInt("isSound", value ? 1 : 0);
                break;
        }
    }

    #endregion
    private void OnDestroy()
    {
        GameManager.SendTransitionToGame -= TransitionToGame;
        GameManager.SendDefeatGame -= DefeatGame;
        GameManager.SendResetGame -= ResetGame;
        BuffController.SendBuff -= Buff;
    }
}
