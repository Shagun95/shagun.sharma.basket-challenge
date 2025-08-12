using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] 
    private List<Transform> postionsFlags;

    [SerializeField, BoxGroup(" Transform References")]
    private Transform playerTrasorm, camera, basketTransform;

    [SerializeField, BoxGroup("GUI Labels")] 
    private TextMeshProUGUI timeLabel, pointLabel;

    [SerializeField, BoxGroup("Other References")]
    private BasketBallController basketBallController;

    [SerializeField] 
    private TextMeshPro tmpBonusLabel;

    private RandomBonusSettings bonusSettings => GameData.Instance.randomBonusSettings;
    private int currentPositionIndex;

    [ShowInInspector]
    private int playerScore;
    [ShowInInspector]
    private int opponentScore;
    
    private int timeRemaining;
    
    public int PlayerScore => playerScore;
    public int OpponentScore => opponentScore;

    

    private void OnEnable()
    {
        EVMLight.Subscribe(GameEvent.ADD_SCORE_TO_PLAYER, AddPlayerScore);
        EVMLight.Subscribe(GameEvent.LAUNCH_BALL, ManageTimer);
        EVMLight.Subscribe(GameEvent.GAME_STARTED, StartGame);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.ADD_SCORE_TO_PLAYER, AddPlayerScore);
        EVMLight.Unsubscribe(GameEvent.LAUNCH_BALL, ManageTimer);
        EVMLight.Unsubscribe(GameEvent.GAME_STARTED, StartGame);
    }

    private void StartGame()
    {
        ResetGame();
        timeRemaining = gameSettings.GameTime;
        Timing.RunCoroutine(StartGameTimer());
        SessionData.Instance.gameIsOn = true;
    }

    private void EndGame()
    {
        SessionData.Instance.gameIsOn = false;
        SessionData.Instance.ballIsLaunching = false;
        SessionData.Instance.scoreForThisRound = playerScore;
        currentPositionIndex = 0;
        SetPostion();
        EVMLight.Trigger(GameEvent.GAME_FINISHED);
        
    }

    private void ManageTimer()
    {
        GenericUtils.StartTimer(gameSettings.TimeToNextPosition, GoToNextPostion);
    }

    /// <summary>
    /// Reset all values to default
    /// </summary>
    private void ResetGame()
    {
        playerScore = 0;
        opponentScore = 0;
        currentPositionIndex = 0;
        pointLabel.text = $"{playerScore}";
        SetPostion();
    }
    
    public void AddPlayerScore()
    {
        int points = sessionData.scoreToAdd;
        //temporary bonus will be more then 0 only if active, if the player achieved a back board score, we can safely add it
        if (sessionData.currentShootType == ShootType.BACK_BOARD)
            points += sessionData.currentTemporaryBonus;
                
        playerScore += points;
        pointLabel.text = $"{playerScore}";
    }

    public void AddOpponentScore(int points) => opponentScore += points;

    [Button]
    private void GoToNextPostion()
    {
        //manage "phantom" change positions when the game is already finished
        if (!sessionData.gameIsOn)
            return;
        currentPositionIndex++;
        SetPostion();
    }

    private void SetPostion()
    {
        if (currentPositionIndex > postionsFlags.Count-1)
            currentPositionIndex = 0;
        SessionData.Instance.currentShootPositionIndex = currentPositionIndex;
        Vector3 currentPosition = postionsFlags[currentPositionIndex].position;
        basketBallController.StopBallSpinning();
        ChangePositionAndRotation(currentPosition, playerTrasorm);
        ChangePositionAndRotation(currentPosition, basketBallController.GetOwnTrasnform, null, .14f);
        ChangePositionAndRotation(currentPosition, camera, 5);
        ManageRandomBonus();
        SessionData.Instance.ballIsLaunching = false;
    }

    /// <summary>
    /// Check if there is a backboard bonus for this shot
    /// </summary>
    private void ManageRandomBonus()
    {
        if (bonusSettings.BonusActive())
        {
            
            int tmpBonus = bonusSettings.GetRandomBonus();
            SessionData.Instance.currentTemporaryBonus = tmpBonus;
            tmpBonusLabel.gameObject.SetActive(true);
            tmpBonusLabel.text = $"+{tmpBonus}";
        }
        else
        {
            tmpBonusLabel.gameObject.SetActive(false);
            SessionData.Instance.currentTemporaryBonus = 0;
        }
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
        EVMLight.Trigger(GameEvent.POSITION_CHANGED);
        
    }

    private GameSettings gameSettings => GameData.Instance.gameSettings;

    private SessionData sessionData => SessionData.Instance;
}
