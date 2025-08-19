using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Coffee.UIEffects;

public class UIFlowManager : MonoBehaviour
{
    
    [BoxGroup("Buttons")]
    [SerializeField]
    private Button PlayGameButton;
    
    [BoxGroup("Buttons")]
    [SerializeField]
    private Button BackToMainMenuButton, ReplayButton, GoToReplayButton;

    [BoxGroup("Panels")]
    [SerializeField]
    private RectTransform MainMenuPanel, GamePlayPanel, RewardPanel, PlayAgainPanel;

    [BoxGroup("Labels")] 
    [SerializeField]
    private TextMeshProUGUI plaeyrScoreLabel, AIScoreLabel, resultDescriptionLabel;

    /// <summary>
    /// Let's use an enum to organize the panels flow
    /// </summary>
    enum Panel
    {
        MainMenu,
        GamePlay,
        Reward,
        Replay
    }

    private void OnEnable()
    {
        EVMLight.Subscribe(GameEvent.GAME_FINISHED, OnGameFinished);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.GAME_FINISHED, OnGameFinished);
    }

    void Awake()
    {
        PlayGameButton.onClick.AddListener(OnStartGameClicked);
        BackToMainMenuButton.onClick.AddListener(() => NavigateToPanel(Panel.MainMenu));
        ReplayButton.onClick.AddListener(OnReplayClicked);
        GoToReplayButton.onClick.AddListener(() => NavigateToPanel(Panel.Replay));
    }

    private void OnReplayClicked()
    {
        EVMLight.Trigger(GameEvent.GAME_STARTED);
        NavigateToPanel(Panel.GamePlay);
    }

    private void OnStartGameClicked()
    {
        EVMLight.Trigger(GameEvent.GAME_STARTED);
        NavigateToPanel(Panel.GamePlay);
    }

    private void OnGameFinished()
    {
        int playerScore = SessionData.Instance.playerScoreForThisRound;
        int AIScore = SessionData.Instance.AIScoreForThisRound;
        //improve colors etx
        resultDescriptionLabel.text = playerScore > AIScore ? "YOU WON!" : "YOU LOST!";
        plaeyrScoreLabel.text = $"{playerScore}";
        AIScoreLabel.text = $"{AIScore}";
        NavigateToPanel(Panel.Reward);
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
