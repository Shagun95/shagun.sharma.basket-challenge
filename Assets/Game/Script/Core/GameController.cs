using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] 
    private List<Transform> postionsFlags;

    [SerializeField, BoxGroup("Transform References")]
    private Transform playerTrasorm, camera, basketTransform, AITransform;

    [SerializeField, BoxGroup("GUI Labels")] 
    private TextMeshProUGUI timeLabel, pointLabel;

    [SerializeField, BoxGroup("GUI Labels")] 
    private TextMeshProUGUI AIScoreLabel;

    [SerializeField, BoxGroup("Other References")]
    private BasketBallController basketBallController, AIBasketballController;

    [SerializeField] 
    private TextMeshPro tmpBonusLabel;

    #endregion

    #region Properties

    private RandomBonusSettings bonusSettings => GameData.Instance.randomBonusSettings;
    private GameSettings gameSettings => GameData.Instance.gameSettings;
    private SessionData sessionData => SessionData.Instance;

    #endregion

    #region State

    private int playerPositionIndex, AIPositionIndex;

    [ShowInInspector]
    private int playerScore;
    [ShowInInspector]
    private int AIScore;
    
    private int timeRemaining;

    /// <summary>
    /// We check though thi if we have a missed shot
    /// </summary>
    private bool playerScoredInThisShot;

    #endregion

    #region Subscriptions

    private void OnEnable()
    {
        EVMLight.Subscribe(GameEvent.PLAYER_SCORED, AddPlayerScore);
        EVMLight.Subscribe(GameEvent.AI_SCORED, AddAIScore);
        EVMLight.Subscribe(GameEvent.LAUNCH_BALL, ManageTimerPlayer);
        EVMLight.Subscribe(GameEvent.AI_LAUNCHED_BALL, ManageTimerAI);
        EVMLight.Subscribe(GameEvent.GAME_STARTED, StartGame);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.PLAYER_SCORED, AddPlayerScore);
        EVMLight.Unsubscribe(GameEvent.AI_SCORED, AddAIScore);
        EVMLight.Unsubscribe(GameEvent.LAUNCH_BALL, ManageTimerPlayer);
        EVMLight.Unsubscribe(GameEvent.AI_LAUNCHED_BALL, ManageTimerAI);
        EVMLight.Unsubscribe(GameEvent.GAME_STARTED, StartGame);
    }

    #endregion

    #region Game Flow

    private void StartGame()
    {
        ResetGame();
        timeRemaining = gameSettings.GameTime;
        Timing.RunCoroutine(StartGameTimer());
        sessionData.gameIsOn = true;
    }

    private void EndGame()
    {
        sessionData.gameIsOn = false;
        sessionData.ballIsLaunching = false;
        sessionData.playerScoreForThisRound = playerScore;
        sessionData.AIScoreForThisRound = AIScore;
        ResetPositions();
        //prevent previous coroutines from still affecting behaviours
        Timing.KillCoroutines();
        EVMLight.Trigger(GameEvent.GAME_FINISHED);
    }

    private void ResetGame()
    {
        playerScoredInThisShot = false;
        playerScore = 0;
        AIScore = 0;
        pointLabel.text = $"{playerScore}";
        AIScoreLabel.text = $"{AIScore}";
        ResetPositions();
    }

    #endregion

    #region Timers & Position Change

    private void ManageTimerPlayer()
    {
        playerScoredInThisShot = false;
        GenericUtils.StartTimer(gameSettings.TimeToNextPosition, MovePlayerToNextPosition);
    }
    
    private void ManageTimerAI()
    {
        GenericUtils.StartTimer(gameSettings.TimeToNextPosition, MoveAIToNextPosition);
    }

    /// <summary>
    /// Since we use a label to update a timer, I decided not to use the
    /// generic method in the Utils
    /// </summary>
    /// <returns></returns>
    private IEnumerator<float> StartGameTimer()
    {
        while (timeRemaining > 0)
        {
            timeLabel.text = GenericUtils.FormatTime(timeRemaining);
            yield return Timing.WaitForSeconds(1f);
            timeRemaining--;
        }

        timeLabel.text = GenericUtils.FormatTime(0);
        EndGame();
    }

    private void MovePlayerToNextPosition()
    {
        playerPositionIndex++;

        //we had a missed shot
        if (!playerScoredInThisShot)
        {
            EVMLight.Trigger(GameEvent.MISSED_SHOT);
        }
        
        if (playerPositionIndex > postionsFlags.Count-1)
            playerPositionIndex = 0;
        sessionData.currentShootPositionIndex = playerPositionIndex;
        Vector3 newPos = postionsFlags[playerPositionIndex].position;
        basketBallController.StopBallSpinning();
        ChangePositionAndRotation(newPos, playerTrasorm);
        ChangePositionAndRotation(newPos, basketBallController.GetOwnTrasnform, null, .14f);
        ChangePositionAndRotation(newPos, camera, 5);
        ManageRandomBonus();
        sessionData.ballIsLaunching = false;
        EVMLight.Trigger(GameEvent.POSITION_CHANGED);
    }

    private void MoveAIToNextPosition()
    {
        AIPositionIndex++;
        if (AIPositionIndex >= postionsFlags.Count)
            AIPositionIndex = 0;

        Vector3 newPos = postionsFlags[AIPositionIndex].position; 
        AIBasketballController.StopBallSpinning();
        ChangePositionAndRotation(newPos, AITransform);
        ChangePositionAndRotation(newPos, AIBasketballController.GetOwnTrasnform, null, .14f);

        AITransform.Translate(Vector3.right * 1.5f, Space.Self);
        AIBasketballController.GetOwnTrasnform.Translate(Vector3.right * 1.5f, Space.Self);
        EVMLight.Trigger(GameEvent.AI_POSITION_CHANGED);
    }

    private void ResetPositions()
    {
        playerPositionIndex = -1;
        AIPositionIndex = -1;
        MovePlayerToNextPosition();
        MoveAIToNextPosition();
    }

    #endregion

    #region Scoring

    private void AddPlayerScore()
    {
        playerScoredInThisShot = true;
        
        int points = sessionData.scoreToAdd;
        //Check if it is a temporary backboard bonus
        if (sessionData.currentShootType == ShootType.BACK_BOARD && sessionData.currentTemporaryBonus > 0)
            points = sessionData.currentTemporaryBonus;

        if (sessionData.fireModeIsActive)
            points *= 2;
        
        playerScore += points;
        pointLabel.text = $"{playerScore}";
    }

    private void AddAIScore()
    {
        int points = sessionData.AIScoreToAdd;
        //Check if it is a temporary backboard bonus
        if (sessionData.currentShootType == ShootType.BACK_BOARD && sessionData.currentTemporaryBonus > 0)
            points = sessionData.currentTemporaryBonus;
                
        AIScore += points;
        AIScoreLabel.text = $"{AIScore}";
    }

    #endregion

    #region Bonus

    /// <summary>
    /// Check if there is a backboard bonus for this shot
    /// </summary>
    private void ManageRandomBonus()
    {
        if (bonusSettings.BonusActive())
        {
            
            int tmpBonus = bonusSettings.GetRandomBonus();
            sessionData.currentTemporaryBonus = tmpBonus;
            tmpBonusLabel.gameObject.SetActive(true);
            tmpBonusLabel.text = $"+{tmpBonus}";
        }
        else
        {
            tmpBonusLabel.gameObject.SetActive(false);
            sessionData.currentTemporaryBonus = 0;
        }
    }

    #endregion
    
    #region Utilities

    private void ChangePositionAndRotation(Vector3 pos, Transform target, float? zOffset = null, float? customYPosition = null)
    {
        if (customYPosition.HasValue)
        {
            pos.y = customYPosition.Value;
        }
        else
        {
            pos.y = target.position.y;
        }
        
        target.position = pos;
        
        //face the basket
        Vector3 lookPos = basketTransform.position;
        lookPos.y = target.position.y;
        target.LookAt(lookPos);
        
        if (zOffset.HasValue)
            target.position -= target.forward * zOffset.Value;
    }

    [Button]
    private void ChangeAIDifficulty(AI_LEVEL lv)
    {
        sessionData.currentAILevel = lv;
    }

    #endregion
}
