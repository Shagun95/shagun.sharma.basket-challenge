using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] 
    private List<Transform> postionsFlags;

    [SerializeField, BoxGroup("References")]
    private Transform playerTrasorm, camera, basketTransform;

    [SerializeField, BoxGroup("References")]
    private BasketBallController _basketBallController;
    
    
    private int currentPositionIndex;

    [ShowInInspector]
    private int playerScore;
    [ShowInInspector]
    private int opponentScore;
    
    public int PlayerScore => playerScore;
    public int OpponentScore => opponentScore;

    private void OnEnable()
    {
        EVMLight.Subscribe(GameEvent.ADD_SCORE_TO_PLAYER, AddPlayerScore);
        EVMLight.Subscribe(GameEvent.LAUNCH_BALL, ManageTimer);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.ADD_SCORE_TO_PLAYER, AddPlayerScore);
        EVMLight.Unsubscribe(GameEvent.LAUNCH_BALL, ManageTimer);
    }

    void Start()
    {
        ResetScore();
    }

    private void ManageTimer()
    {
        GenericUtils.StartTimer(gameSettings.timeToNextPosition, GoToNextPostion);
    }

    private void ResetScore()
    {
        playerScore = 0;
        opponentScore = 0;
        currentPositionIndex = 0;
    }
    
    public void AddPlayerScore()
    {
        int points = SessionData.Instance.scoreToAdd;
        playerScore += points;
    }

    public void AddOpponentScore(int points) => opponentScore += points;

    [Button]
    private void GoToNextPostion()
    {
        currentPositionIndex++;
        if (currentPositionIndex > postionsFlags.Count-1)
            currentPositionIndex = 0;
        Vector3 currentPosition = postionsFlags[currentPositionIndex].position;
        _basketBallController.StopBallSpinning();
        ChangePositionAndRotation(currentPosition, playerTrasorm);
        ChangePositionAndRotation(currentPosition, _basketBallController.GetOwnTrasnform, null, .14f);
        ChangePositionAndRotation(currentPosition, camera, 5);

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
}
