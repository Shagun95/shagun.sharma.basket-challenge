using System;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Responsible for managing input though different sources
/// </summary>
public class InputManager : MonoBehaviour
{
    [SerializeField, Required]
    private LaunchBarController launchBar;

    [SerializeField, BoxGroup("Input Settings")]
    private float maxSwipeDistance;
    
    [SerializeField, BoxGroup("Input Settings")]
    private float minSwipeDistance;
    
    private float accumulatedDistance = 0f;
    
    private Vector2 lastPos;
    private bool isSwiping = false;

    private Vector2 currentPosition;

    private void OnEnable()
    {
        EVMLight.Subscribe(GameEvent.GAME_FINISHED, ResetValues);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.GAME_FINISHED, ResetValues);
    }

    // Update is called once per frame
    void Update()
    {
        if (!SessionData.Instance.gameIsOn || SessionData.Instance.ballIsLaunching)
            return;
        
        #if UNITY_EDITOR || UNITY_STANDALONE
                ManageMouseInput();
                
        #elif UNITY_ANDROID //no iOS needed for prototype
                ManageTouchInput();
                
        #endif
    }
    
    #region device dependent mehtods
    
    #if UNITY_EDITOR || UNITY_STANDALONE
        
        /// <summary>
        /// Manage the mouse input in the editor
        /// </summary>
        private void ManageMouseInput()
        {
            currentPosition = Input.mousePosition;
            if (!isSwiping && Input.GetMouseButtonDown(0))
            {
                StartSwipe(Input.mousePosition);
            }

            if (isSwiping && Input.GetMouseButton(0))
            {
                SwipeFeedback(Input.mousePosition);
            }

            if (isSwiping && Input.GetMouseButtonUp(0))
            {
                EndSwipe();
            }

        }
        
    #endif
        
    #if UNITY_ANDROID
       
        /// <summary>
        /// Manage the touch input in Android
        /// </summary>
        private void ManageTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(0);

                currentPosition = t.position;

                if (!isSwiping &&  t.phase == TouchPhase.Began)
                {
                    StartSwipe(t.position);
                }

                if (isSwiping)
                {
                    SwipeFeedback(t.position);
                }

                if (isSwiping && t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                {
                    EndSwipe();
                }
            }
        }

    #endif  
    
    #endregion


    #region centralized methods

    /// <summary>
    /// starts the swipe input, indipendent of input source
    /// </summary>
    /// <param name="pos"></param>
    private void StartSwipe(Vector2 pos)
    {
        isSwiping = true;
        lastPos = pos;
        GenericUtils.StartTimer(GameData.Instance.gameSettings.BarTimer, EndSwipe);
    }

    /// <summary>
    /// checks if the swipe is going up, will use it to manage the feedback bar
    /// </summary>
    /// <param name="currentPos"></param>
    private void SwipeFeedback(Vector2 currentPos)
    {
        float deltaPos = currentPos.y - lastPos.y;
        
        //the swipe works as long as it goes up
        if (deltaPos >= 0)
        {
            accumulatedDistance += deltaPos;
            float progress = Mathf.Clamp01(accumulatedDistance / maxSwipeDistance);
            launchBar.SetFillBar(progress);
            
            lastPos = currentPos;
        }
        else
        {
            EndSwipe();
        }
    }

    /// <summary>
    /// The swipe either ended or went down
    /// </summary>
    private void EndSwipe()
    {
        //the endswipe might be called with the timer even if the ball is launching, we'll prevent that
        if (SessionData.Instance.ballIsLaunching)
            return;
        
        Vector2 pos = currentPosition;
        //check if the swipe was too short (probably just a touch on the screen)
        if (accumulatedDistance < minSwipeDistance)
        {
            isSwiping = false;
            accumulatedDistance = 0f;
            launchBar.SetFillBar(0f);
            return;
        }
        lastPos = pos;
        
        ShootType shootType = launchBar.CheckShoot();
        SessionData.Instance.currentShootType = shootType;
        //trigger the event the ball has subscribed to
        EVMLight.Trigger(GameEvent.LAUNCH_BALL);
        
        isSwiping = false;
        accumulatedDistance = 0f;
    }

    private void ResetValues()
    {
        isSwiping = false;
        accumulatedDistance = 0f;
    }
    
    #endregion
    
    
    
}
