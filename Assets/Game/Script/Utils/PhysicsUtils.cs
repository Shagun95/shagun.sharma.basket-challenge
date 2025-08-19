using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Util class to manage basic operation of the basket ball
/// </summary>
public class PhysicsUtils
{
    
    /// <summary>
    /// The offset to apply for a wrong shot
    /// </summary>
    private static float wrongShootOffset = 0.5f;
    
    

    /// <summary>
    /// Shoot the ball toward the basket, including error handling
    /// </summary>
    public static Vector3 ShootBall(Vector3 start, Vector3 target, float timeToTarget, ShootType shotType, float vDistance)
    {
        return VelocityToTarget(start, target, timeToTarget, shotType, vDistance);
    }

    /// <summary>
    /// Calculate the velocity to apply to reach the target
    /// </summary>
    /// <param name="start"></param>
    /// <param name="target"></param>
    /// <param name="timeToTarget"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private static Vector3 VelocityToTarget(Vector3 start, Vector3 target, float timeToTarget, ShootType shootType, float vDistance)
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
        
        return ShotWithForceCorrection(shootType, forceToApply, vDistance);
    }


    /// <summary>
    /// Will return the actual force to apply to the object, considering errors
    /// </summary>
    /// <param name="shootType"></param>
    /// <param name="originalForce"></param>
    /// <returns></returns>
    private static Vector3 ShotWithForceCorrection(ShootType shootType, Vector3 originalForce, float vDistance)
    {
        Vector3 restultForce = originalForce;
        //if it's a ring shot, it's already managed
        if (shootType == ShootType.RING)
            return restultForce;
        
        //the tolerance we set in the settingsdata
        float tolerance = GameData.Instance.gameSettings.ImperfectShotTolerance;
        
        //if the cursor is in the perfect position, it's all managed
        if (vDistance == 0)
            return restultForce;
        
        //from this point on, we manage errors
        
        /*
         * what we check: if we attempt a backboard, then it has to be perfectly aligned
         * otherwise we apply an error
         * for the net attempt, we check if the distance between the pointer and the green zone
         * is greater then the tolerance we applied, also in this case we apply an error
         */
        if ((shootType == ShootType.BACK_BOARD && vDistance > 0)
            || (shootType == ShootType.NET && vDistance > tolerance))
        {
            restultForce += Vector3.left * (Random.Range(0, 10) > 5 ? -wrongShootOffset : wrongShootOffset);
        }

        /*
         * to give a more realistic experience, we calculate the amount of magnitude we want
         * to add to our final vector, can be negative or positive, we set a max to 1 so the ball
         * doesn't shoot too far away
         */ 
        float extraMagnitude = Mathf.Min(vDistance, 1f);
        
        float newMagnitude = restultForce.magnitude + extraMagnitude;
        restultForce = restultForce.normalized * newMagnitude;

        return restultForce;
    }
}


