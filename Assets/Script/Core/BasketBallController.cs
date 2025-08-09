using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class BasketBallController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Transform basketTarget, backBoardTarget;

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
        }
    }
}
