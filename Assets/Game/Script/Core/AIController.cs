using System;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    
    private CoroutineHandle timer;
    private void OnEnable()
    {
        EVMLight.Subscribe(GameEvent.GAME_STARTED, WaitAndShoot);
        EVMLight.Subscribe(GameEvent.AI_POSITION_CHANGED, WaitAndShoot);
        EVMLight.Subscribe(GameEvent.GAME_STARTED, KillCoroutines);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.GAME_STARTED, WaitAndShoot);
        EVMLight.Unsubscribe(GameEvent.AI_POSITION_CHANGED, WaitAndShoot);
        
        
        Timing.KillCoroutines(timer);
    }

    /// <summary>
    /// prevent previous coroutines from acting again
    /// </summary>
    private void KillCoroutines() => Timing.KillCoroutines(timer);

    /// <summary>
    /// Wait the specified amount of time, perform a shoot
    /// </summary>
    private void WaitAndShoot()
    {
        timer = GenericUtils.StartTimer(GetShootTime(), ShootBall);
    }

    [Button]
    private void ShootBall()
    {
        sessionData.AIcurrentShootType = GetShootType();
        sessionData.AIVerticalDistance = GetPrecision();
        EVMLight.Trigger(GameEvent.AI_LAUNCHED_BALL);
    }


    /// <summary>
    /// Calculates the shoot type based on the data found on the correct
    /// profiling, then, randomly calculates the shoot
    /// </summary>
    /// <returns></returns>
    private ShootType GetShootType()
    {
        //default
        ShootType currentShoot = ShootType.NET;
        int roll = Random.Range(0, 100);
        
        //chances to have a backboard, taken from the profile
        if (roll < CurrentProfile().BackBoardChance)
        {
            currentShoot = ShootType.BACK_BOARD;
        }
        
        //chance to have a ring shoot, we have to consider that this launch will always go
        //on the target, so a worse AI should have less chances to have a ring chance
        if (roll < CurrentProfile().RingChance)
        {
            currentShoot = ShootType.RING;
        }
        return currentShoot;
    }

    /// <summary>
    /// Calculates the precision based on the profile, adding an offset if the shot is wrong
    /// </summary>
    /// <returns></returns>
    private float GetPrecision()
    {
        int precision = CurrentProfile().Precision;

        if (Random.Range(0, 100) < precision)
        {
            return 0f;
        }
        
        float offset = Random.Range(-1f, 1f);

        return offset;
    }

    /// <summary>
    /// Calculates randomly a shoot time, for a more realistic effect
    /// </summary>
    /// <returns></returns>
    private float GetShootTime()
    {
        return Random.Range(gameData.AIProfiling.minShootTime, gameData.AIProfiling.maxShootTime);
    }

    private DifficultyProfile CurrentProfile()
    {
        switch (sessionData.currentAILevel)
        {
            case AI_LEVEL.EASY:
                return gameData.AIProfiling.Easy;
            case AI_LEVEL.MEDIUM:
                return gameData.AIProfiling.Medium;
            case AI_LEVEL.HARD:
                return gameData.AIProfiling.Hard;
            case AI_LEVEL.LEGEND:
                return gameData.AIProfiling.Legend;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private SessionData sessionData => SessionData.Instance;
    private GameData gameData => GameData.Instance;
}
