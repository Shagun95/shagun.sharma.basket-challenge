using Sirenix.OdinInspector;
using UnityEngine;

public class BasketBallController : MonoBehaviour
{

    public Vector3 testShake;

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
        EVMLight.Subscribe(GameEvent.LAUNCH_BALL, ShootBall);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.LAUNCH_BALL, ShootBall);
    }

    private void Start()
    {
        toleranceForRingShot = GameData.Instance.gameSettings.ImperfectShotTolerance;
    }

    private void ShootBall()
    {
        if (!SessionData.Instance.gameIsOn)
            return;
        ShootBall(SessionData.Instance.currentShootType);
    }

    [Button("test shoot")]
    private void ShootBall(ShootType type)
    {
        float vDistance = Mathf.Abs(SessionData.Instance.verticalDistance);
        Debug.Log(vDistance);
        SessionData.Instance.ballIsLaunching = true;
        _currentShootType = type;

        //first we set the right target
        Vector3 target = basketTarget.position;

        if (_currentShootType == ShootType.BACK_BOARD)
            target = backBoardTarget.position;

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
            _currentShootType);
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
            SessionData.Instance.scoreToAdd = points;
            EVMLight.Trigger(GameEvent.PLAYER_SCORED);
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
    /// Will take in consideration temporary bonus
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
