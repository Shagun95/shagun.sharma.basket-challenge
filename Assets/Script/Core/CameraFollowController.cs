using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    
    [SerializeField]
    private Vector3 offset;
    
    [SerializeField]
    private Transform basket;
    
    private float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    private bool follow = false;

    private void OnEnable()
    {
        EVMLight.Subscribe(GameEvent.LAUNCH_BALL, StartTimerFollow);
        EVMLight.Subscribe(GameEvent.POSITION_CHANGED, RestartFollow);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.LAUNCH_BALL, StartTimerFollow);
        EVMLight.Unsubscribe(GameEvent.POSITION_CHANGED, RestartFollow);
    }

    private void RestartFollow() => follow = true;

    /// <summary>
    /// When ball is launched, the camera will follow the ball for
    /// the indicated timer
    /// </summary>
    private void StartTimerFollow()
    {
        follow = true;
        float timerTime = GameData.Instance.gameSettings.TimeToLaunchBall;
        GenericUtils.StartTimer(timerTime, () => follow = false);
    }

    void LateUpdate()
    {
        if (follow)
        {
            Vector3 targetPosition = target.TransformPoint(offset);
            
            // move the camera towards that target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            //while looking at the basket
            transform.LookAt(basket);
        }
    }
}
