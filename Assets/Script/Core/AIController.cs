using Sirenix.OdinInspector;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private void OnEnable()
    {
        EVMLight.Subscribe(GameEvent.GAME_STARTED, WaitAndShoot);
        EVMLight.Subscribe(GameEvent.AI_POSITION_CHANGED, WaitAndShoot);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.GAME_STARTED, WaitAndShoot);
        EVMLight.Unsubscribe(GameEvent.AI_POSITION_CHANGED, WaitAndShoot);
    }

    private void WaitAndShoot()
    {
        GenericUtils.StartTimer(GetShootTime(), ShootBall);
    }

    [Button]
    private void ShootBall()
    {
        sessionData.AIcurrentShootType = GetShootType();
        sessionData.AIVerticalDistance = GetRandomOffset();
        EVMLight.Trigger(GameEvent.AI_LAUNCHED_BALL);
    }


    private ShootType GetShootType()
    {
        return ShootType.NET;
    }

    private float GetRandomOffset()
    {
        return 0;
    }

    private float GetShootTime()
    {
        return 2;
    }

    private SessionData sessionData => SessionData.Instance;
}
