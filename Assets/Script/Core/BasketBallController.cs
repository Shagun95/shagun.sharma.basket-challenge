using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class BasketBallController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Transform basketTarget, backBoardTarget;

    public Transform GetOwnTrasnform => GetComponent<Transform>();

    /// <summary>
    /// will use to calculate how many points to give
    /// </summary>
    private ShootType _currentShootType;

    private void OnEnable()
    {
        EVMLight.Subscribe(GameEvent.LAUNCH_BALL, ShootBall);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.LAUNCH_BALL, ShootBall);
    }

    private void ShootBall()
    {
        ShootBall(SessionData.Instance.currentShootType);
    }

    [Button("test shoot")]
    private void ShootBall(ShootType type)
    {
        _currentShootType = type;
        Vector3 target = basketTarget.position;
        if (type == ShootType.BACK_BOARD)
            target = backBoardTarget.position;

        //take target time from the scriptable settings
        float timeToReachTarget = GameData.Instance.gameSettings.timeToLaunchBall;
        rb.velocity = PhysicsUtils.ShootBall(transform.position, target, timeToReachTarget, type);
    }

    [Button]
    private void RestBall()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;         
        rb.angularVelocity = Vector3.zero;  
    }

    public void StopBallSpinning()
    {
        rb.velocity = Vector3.zero;         
        rb.angularVelocity = Vector3.zero; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BasketTrigger")
        {
            Debug.Log("Point Scored!");
            int points = PointByShoot();
            SessionData.Instance.scoreToAdd = points;
            EVMLight.Trigger(GameEvent.ADD_SCORE_TO_PLAYER);
        }
    }


    /// <summary>
    /// Will take in consideration temporary bonus
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private int PointByShoot()
    {
        if (_currentShootType == ShootType.PERFECT)
            return 3;

        if (_currentShootType == ShootType.RING)
            return 2;

        return 0;
    }
}
