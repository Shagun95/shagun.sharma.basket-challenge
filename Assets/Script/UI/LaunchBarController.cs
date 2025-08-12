using System;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class LaunchBarController : MonoBehaviour
{
    [SerializeField]
    Slider ownSlider;

    
    [SerializeField]
    private RectTransform greenZoneImage, blueZoneImage;

    /// <summary>
    /// An invisble recttransform that will check if the pointer is in the green or blue area
    /// </summary>
    [SerializeField]
    private RectTransform checkPointer;
    
    [SerializeField, BoxGroup("Green zone"), Tooltip("Setup the distance in height from base of the green area")] 
    private float yDistanceGreenZone1, yDistanceGreenZone2, yDistanceGreenZone3;

    [SerializeField, BoxGroup("Blue zone"), Tooltip("Setup the distance in height from base of the blue area")] 
    private float yDistanceBlueZone1, yDistanceBlueZone2, yDistanceBlueZone3;

    private void OnEnable()
    {
        EVMLight.Subscribe(GameEvent.POSITION_CHANGED, ResetBar);
        EVMLight.Subscribe(GameEvent.GAME_STARTED, ResetBar);
        ResetBar();
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.POSITION_CHANGED, ResetBar);
        EVMLight.Unsubscribe(GameEvent.GAME_STARTED, ResetBar);
    }

    [Button]
    public void SetFillBar(float fill)
    {
        ownSlider.value = fill;
    }

    [Button]
    public void SetupBar(Position position)
    {
        switch (position)
        {
            case Position.LAUNCH_ONE:
                SetImagePosition(greenZoneImage, yDistanceGreenZone1);
                SetImagePosition(blueZoneImage, yDistanceBlueZone1);
                break;
            case Position.LAUNCH_TWO:
                SetImagePosition(greenZoneImage, yDistanceGreenZone2);
                SetImagePosition(blueZoneImage, yDistanceBlueZone2);
                break;
            case Position.LAUNCH_THREE:
                SetImagePosition(greenZoneImage, yDistanceGreenZone3);
                SetImagePosition(blueZoneImage, yDistanceBlueZone3);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(position), position, null);
        }
    }


    /// <summary>
    /// Check from the bar what kind of shot has been achieved
    /// </summary>
    /// <returns></returns>
    public ShootType CheckShoot()
    {
        bool greenArea = GenericUtils.RectOverlaps(greenZoneImage, checkPointer);
        bool blueArea = GenericUtils.RectOverlaps(blueZoneImage, checkPointer);

        //in this point, we will check if there's a small distance between the pointer and
        //the area, and will trigger an imperfet shoot if that is the case
        if (!greenArea && !blueArea)
            return ShootType.WRONG;

        return greenArea ? ShootType.PERFECT : ShootType.BACK_BOARD;
    }
    
    //utils----
    
    private void SetImagePosition(RectTransform image, float yPos)
    {
        Vector2 pos = image.anchoredPosition;
        pos.y = yPos;
        image.anchoredPosition = pos;
    }

    private void ResetBar()
    { 
        ownSlider.value = 0f;
    }

}


