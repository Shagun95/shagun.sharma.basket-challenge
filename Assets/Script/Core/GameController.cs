using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [ShowInInspector]
    private int playerScore;
    [ShowInInspector]
    private int opponentScore;
    
    public int PlayerScore => playerScore;
    public int OpponentScore => opponentScore;

    private void OnEnable()
    {
        EVMLight.Subscribe<int>(GameEvent.ADD_SCORE_TO_PLAYER, AddPlayerScore);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe<int>(GameEvent.ADD_SCORE_TO_PLAYER, AddPlayerScore);
    }

    void Start()
    {
        ResetScore();
    }

    private void ResetScore()
    {
        playerScore = 0;
        opponentScore = 0;
    }
    
    public void AddPlayerScore(int points) => playerScore += points;
    public void AddOpponentScore(int points) => opponentScore += points;
}
