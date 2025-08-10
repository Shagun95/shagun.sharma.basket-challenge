using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class BasketBallController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Transform basketTarget, backBoardTarget;

    /// <summary>
    /// will use to calculate how many points to give
    /// </summary>
    private ShootType _currentShootType;

    private void OnEnable()
    {
        EVMLight.Subscribe<ShootType>(GameEvent.LAUNCH_BALL, ShootBall);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe<ShootType>(GameEvent.LAUNCH_BALL, ShootBall);
    }

    [Button("test shoot")]
    private void ShootBall(ShootType type)
    {
        _currentShootType = type;
        Vector3 target = basketTarget.position;
        if (type == ShootType.BACK_BOARD)
            target = backBoardTarget.position;
        rb.velocity = BallUtils.ShootBall(transform.position, target, 2, type);
    }

    [Button]
    private void RestBall()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;         
        rb.angularVelocity = Vector3.zero;  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BasketTrigger")
        {
            Debug.Log("Point Scored!");
            int points = PointByShoot();
            EVMLight.Trigger(GameEvent.ADD_SCORE_TO_PLAYER, points);
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
