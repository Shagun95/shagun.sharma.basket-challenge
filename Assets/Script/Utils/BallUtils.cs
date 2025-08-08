using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallUtils
{
    
    public enum ShootType 
    {
        PERFECT,
        RING,
        BACK_BOARD,
        WRONG
    }

    
    public static Vector3 ShootBall(Vector3 start, Vector3 target, float timeToTarget, ShootType type)
    {
        Vector3 shot = VelocityToTarget(start, target, timeToTarget, type);
        return ErrorHandling(shot, type);
    }

    /// <summary>
    /// Calculate the velocity to apply to the ball, to reach the target
    /// </summary>
    /// <param name="start"></param>
    /// <param name="target"></param>
    /// <param name="timeToTarget"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private static Vector3 VelocityToTarget(Vector3 start, Vector3 target, float timeToTarget, ShootType type)
    {
        Vector3 toTarget = target - start;
        //separates the "plane" distance to the y distance, they have different calculations
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0, toTarget.z);

        //the distance to cover in height
        float yDistance = toTarget.y;
        //the magnitude of the distance in the xz axis
        float xzDistance = toTargetXZ.magnitude;

        //formula to calculate the vertically accelerated motion (we have to consider gravity)
        float yVelocity = yDistance / timeToTarget + 0.5f * Mathf.Abs(Physics.gravity.y) * timeToTarget;
        //formula to calculate uniform rectilinear motion
        float xzVelocity = xzDistance / timeToTarget;

        //to have the perfect shot, we multiply the direction by the velocity we calculated tiht the time
        Vector3 forceToApply = toTargetXZ.normalized * xzVelocity;
        //the velocity to reach the target considering gravity at the right time
        forceToApply.y = yVelocity;

        return forceToApply;
    }
    
    
    private static Vector3 ErrorHandling (Vector3 perfectShot, ShootType type)
    {
        switch (type)
        {
            case ShootType.PERFECT:
                //no need to intervene
                break;
            case ShootType.RING:
                perfectShot.x += Random.value < 0.5f ? 0.14f : -0.14f;
                break;
            case ShootType.BACK_BOARD:
                break;
            case ShootType.WRONG:
                perfectShot.x += Random.value < 0.5f ? 0.5f : -0.5f;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return perfectShot;
    }
}
