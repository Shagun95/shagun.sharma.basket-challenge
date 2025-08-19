using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UltimateClean;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField, BoxGroup("Reference")] 
    private RectTransform ownRect;

    [SerializeField, BoxGroup("Reference")] 
    private AudioSource music;

    [SerializeField, BoxGroup("Reference")]
    private Button closeButton, settingsButton;
    
    [SerializeField, BoxGroup("Reference")]
    private Switch AudioSwitch, MusicSwitch;

    [SerializeField, BoxGroup("Reference")]
    private List<Toggle> difficltyToggle;

    
    private bool audioOn, musicOn;

    private void OnEnable()
    {
        MusicSwitch.OnValueChanged.AddListener(ManageMusic);
        AudioSwitch.OnValueChanged.AddListener(ManageAudio);
        closeButton.onClick.AddListener(ClosePopup);
        settingsButton.onClick.AddListener(OpenPopup);
        
        foreach (var toggle in difficltyToggle)
        {
            toggle.onValueChanged.AddListener(isOn => {
                if (isOn) OnDifficultyChanged(toggle);
            });
        }
    }
    
    private void OnDisable()
    {
        MusicSwitch.OnValueChanged.RemoveListener(ManageMusic);
        AudioSwitch.OnValueChanged.RemoveListener(ManageAudio);
        closeButton.onClick.RemoveListener(ClosePopup);
        settingsButton.onClick.RemoveListener(OpenPopup);
        
        foreach (var toggle in difficltyToggle)
        {
            toggle.onValueChanged.RemoveAllListeners();
        }
    }
    
    private void Start()
    {
        //set default values
        audioOn = PlayerPrefs.GetInt("AUDIO", 1) == 1;
        musicOn =  PlayerPrefs.GetInt("MUSIC", 1) == 1;
        
        sessionData.currentAILevel = AI_LEVEL.EASY;
        
        MusicSwitch.SetState(musicOn, false);
        AudioSwitch.SetState(audioOn, false);
        
        ManageMusic(musicOn);
        ManageAudio(audioOn);
    }

    private void OnDifficultyChanged(Toggle activeToggle)
    {
        switch (activeToggle.name)
        {
            case "easy": sessionData.currentAILevel = AI_LEVEL.EASY; break;
            case "medium": sessionData.currentAILevel = AI_LEVEL.MEDIUM; break;
            case "hard": sessionData.currentAILevel = AI_LEVEL.HARD; break;
            case "legend": sessionData.currentAILevel = AI_LEVEL.LEGEND; break;
        }
    }
    

    public void ManageMusic(bool state)
    {
        musicOn = state;
        if (musicOn)
        {
            music.Play();
        }
        else
        {
            music.Stop();
        }
        SavePref();
    }
    
    public void ManageAudio(bool state)
    {
        audioOn = state;
        //we set also this to have a faster way to access this information for various effects in the game
        sessionData.audioOn = audioOn;
        SavePref();
    }

    private void OpenPopup()
    {
        ownRect.DOScale(Vector2.one, .5f).SetEase(Ease.OutBack);
    }

    private void ClosePopup()
    {
        ownRect.DOScale(Vector2.zero, .5f).SetEase(Ease.InBack);
    }

    private SessionData sessionData => SessionData.Instance;

    private void SavePref()
    {
        PlayerPrefs.SetInt("AUDIO", audioOn ? 1 : 0);
        PlayerPrefs.SetInt("MUSIC", musicOn ? 1 : 0);
        PlayerPrefs.Save();
    }

}
