using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIFlowManager : MonoBehaviour
{
    
    [BoxGroup("Buttons")]
    [SerializeField]
    private Button PlayGameButton;
    
    [BoxGroup("Buttons")]
    [SerializeField]
    private Button BackToMainMenuButton, ReplayButton;

    [BoxGroup("Temporary buttons")]
    [SerializeField]
    private Button GoToRewardButton, GoToReplayButton;

    [BoxGroup("Panels")]
    [SerializeField]
    private RectTransform MainMenuPanel, GamePlayPanel, RewardPanel, PlayAgainPanel;
    

    /// <summary>
    /// Let's us an enum to organize the panels flow
    /// </summary>
    enum Panel
    {
        MainMenu,
        GamePlay,
        Reward,
        Replay
    }
    
    void Awake()
    {
        PlayGameButton.onClick.AddListener(() => NavigateToPanel(Panel.GamePlay));
        BackToMainMenuButton.onClick.AddListener(() => NavigateToPanel(Panel.MainMenu));
        ReplayButton.onClick.AddListener(() => NavigateToPanel(Panel.GamePlay));
        
        //tmp
        GoToRewardButton.onClick.AddListener(() => NavigateToPanel(Panel.Reward));
        GoToReplayButton.onClick.AddListener(() => NavigateToPanel(Panel.Replay));
    }

    [Button("Test navigation")]
    private void NavigateToPanel(Panel toPanel)
    {
        /*
         * Fow now, we use the gameobject property of the recttransform,
         * will create a proper animation of panels through dotween later
         */
        SwitchAllPanelsOff();
        switch (toPanel)
        {
            case Panel.MainMenu:
                MainMenuPanel.gameObject.SetActive(true);
                break;
            case Panel.GamePlay:
                GamePlayPanel.gameObject.SetActive(true);
                break;
            case Panel.Reward:
                RewardPanel.gameObject.SetActive(true);
                break;
            case Panel.Replay:
                PlayAgainPanel.gameObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(toPanel), toPanel, null);
        }
    }

    private void SwitchAllPanelsOff()
    {
        MainMenuPanel.gameObject.SetActive(false);
        GamePlayPanel.gameObject.SetActive(false);
        RewardPanel.gameObject.SetActive(false);
        PlayAgainPanel.gameObject.SetActive(false);
    }
    
}
