using Sirenix.OdinInspector;
using UnityEngine;

public class BasketBallController : MonoBehaviour
{
    [SerializeField]
    private BallOwner ballOwner;

    [SerializeField] private Rigidbody rb;

    [SerializeField] private Transform basketTarget, backBoardTarget;

    private float toleranceForRingShot;

    public Transform GetOwnTrasnform => GetComponent<Transform>();

    /// <summary>
    /// will use to calculate how many points to give
    /// </summary>
    private ShootType _currentShootType;

    private void OnEnable()
    {
        EVMLight.Subscribe(ballOwner == BallOwner.Player ? GameEvent.LAUNCH_BALL : GameEvent.AI_LAUNCHED_BALL, ShootBall);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(ballOwner == BallOwner.Player ? GameEvent.LAUNCH_BALL : GameEvent.AI_LAUNCHED_BALL, ShootBall);
    }

    private void Start()
    {
        toleranceForRingShot = GameData.Instance.gameSettings.ImperfectShotTolerance;
    }

    [Button("test shoot")]
    private void ShootBall()
    {
        
        if (!SessionData.Instance.gameIsOn)
            return;
        
        float vDistance = 0;
        Vector3 target = basketTarget.position;
        
        //first, we set the right values, checking if its an AI or the Player
        SetCorrectData(ref vDistance, ref target);
        
        /*
         * then we see if its a ring shot (imperfect)
         * to check this, we see if the player was closer to the green zone (perfect shot attempt)
         * then we check if the distance of the pointer was greater then 0 (not perfect)
         * lastly we check if that distance is lesser then the tolerance we set in the scriptable data
         */

        if (_currentShootType == ShootType.NET &&
            vDistance > 0 &&
            vDistance < toleranceForRingShot
           )
        {
            _currentShootType = ShootType.RING;
        }


        //take target time from the scriptable settings
        float timeToReachTarget = GameData.Instance.gameSettings.TimeToLaunchBall;


        rb.velocity = PhysicsUtils.ShootBall(transform.position, target, timeToReachTarget,
            _currentShootType, vDistance);
    }

    /// <summary>
    /// Set the correct data checking if it is the player or the AI
    /// </summary>
    /// <param name="vDistance"></param>
    /// <param name="target"></param>
    private void SetCorrectData(ref float vDistance, ref Vector3 target)
    {
        if (ballOwner == BallOwner.Player)
        {
            vDistance = Mathf.Abs(SessionData.Instance.verticalDistance);
            SessionData.Instance.ballIsLaunching = true;
            _currentShootType = SessionData.Instance.currentShootType;

            //first we set the right target
            target = basketTarget.position;

            if (_currentShootType == ShootType.BACK_BOARD)
                target = backBoardTarget.position;
        }
        else
        {   
            //IF AI--
            vDistance = Mathf.Abs(SessionData.Instance.AIVerticalDistance);
            _currentShootType = SessionData.Instance.AIcurrentShootType;

            //first we set the right target
            target = basketTarget.position;

            if (_currentShootType == ShootType.BACK_BOARD)
                target = backBoardTarget.position;
        }
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
        if (other.gameObject.CompareTag("BasketTrigger"))
        {
            int points = PointByShoot();
            if (ballOwner == BallOwner.Player)
            {
                SessionData.Instance.scoreToAdd = points;
                EVMLight.Trigger(GameEvent.PLAYER_SCORED);
            }
            else
            {
                //IF AI--
                SessionData.Instance.AIScoreToAdd = points;
                EVMLight.Trigger(GameEvent.AI_SCORED);
            }
            
        }

        if (_currentShootType == ShootType.RING && other.gameObject.CompareTag("ShakeTrigger"))
        {
            RingShakeEffect();
        }
    }

    /// <summary>
    /// Apply a shake effect, used to handle ring shots
    /// </summary>
    private void RingShakeEffect()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        for (int i = 0; i < 5; i++)
        {
            Vector3 impulse = new Vector3(
                Random.Range(-1f, 1f) * 5,
                0,
                Random.Range(-1f, 1f) * 5
            );

            rb.AddForce(impulse, ForceMode.VelocityChange);
        }
        
        rb.useGravity = true;
        //make sure it fals down correctly
        rb.AddForce(Vector3.down, ForceMode.VelocityChange);
    }




    /// <summary>
    /// Calculates the correct points to add
    /// </summary>
    /// <returns></returns>
    private int PointByShoot()
    {
        if (_currentShootType == ShootType.NET || _currentShootType == ShootType.BACK_BOARD)
            return 3;

        if (_currentShootType == ShootType.RING)
            return 2;

        return 0;
    }
}
