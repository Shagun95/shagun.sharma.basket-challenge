using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class BasketBallController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Transform basketTarget, backBoardTarget;
    

    [Button("test shoot")]
    public void ShootBall(BallUtils.ShootType type)
    {
        Vector3 target = basketTarget.position;
        if (type == BallUtils.ShootType.BACK_BOARD)
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
