using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UltimateClean;
using UnityEngine;
using UnityEngine.UI;

public class DoubleUpBarController : MonoBehaviour
{
    [SerializeField]
    private SlicedFilledImage slider;

    [SerializeField, BoxGroup("Settings"), Tooltip("How much the bar fills at each consecutive shot")]
    private float addValue;

    [SerializeField, BoxGroup("Settings"), Tooltip("How much the bar decrease over time")]
    private float decreaseValue;

    
    private SessionData sessionData => SessionData.Instance;
    private CoroutineHandle sliderRoutine;

    private void OnEnable()
    {
        EVMLight.Subscribe(GameEvent.GAME_STARTED, StartSliderDecrease);
        EVMLight.Subscribe(GameEvent.PLAYER_SCORED, FillBar);
        EVMLight.Subscribe(GameEvent.MISSED_SHOT, ResetBar);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.GAME_STARTED, StartSliderDecrease);
        EVMLight.Unsubscribe(GameEvent.PLAYER_SCORED, FillBar);
        EVMLight.Unsubscribe(GameEvent.MISSED_SHOT, ResetBar);

        Timing.KillCoroutines(sliderRoutine);
    }

    /// <summary>
    /// Starts the decrease process of the bar
    /// </summary>
    private void StartSliderDecrease()
    {
        slider.fillAmount = 0f;
        Timing.KillCoroutines(sliderRoutine);
        sliderRoutine = Timing.RunCoroutine(SliderDecrease());
    }

    /// <summary>
    /// At every shot, an amont is added to the bar
    /// </summary>
    private void FillBar()
    {
        if (sessionData.fireModeIsActive)
            return;
        
        slider.fillAmount += addValue;
        
        if (slider.fillAmount >= 1f)
            ActivateFireMode();
    }

    /// <summary>
    /// Routine that will handle the decrease process
    /// </summary>
    /// <returns>the handler to kill once the session is finished</returns>
    private IEnumerator<float> SliderDecrease()
    {
        while (true)
        {
            if (slider.fillAmount <= 0f)
            {
                if (sessionData.fireModeIsActive)
                    ResetBar();

                yield return Timing.WaitForOneFrame;
                continue;
            }

            slider.fillAmount -= decreaseValue;
            yield return Timing.WaitForSeconds(0.1f);
        }
    }

    private void ResetBar()
    {
        slider.fillAmount = 0f;
        sessionData.fireModeIsActive = false;
        EVMLight.Trigger(GameEvent.FIREBALL_MODE_CHANGED);
    }

    private void ActivateFireMode()
    {
        sessionData.fireModeIsActive = true;
        EVMLight.Trigger(GameEvent.FIREBALL_MODE_CHANGED);
    }
}