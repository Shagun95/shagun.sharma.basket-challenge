using TMPro;
using UnityEngine;
using DG.Tweening;

public class ScoreFlyerController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ownText;
    
    [SerializeField]
    private float moveDistance = 100f;
    [SerializeField]
    private float duration = 1f;

    private Vector2 initPos;
    
    private void OnEnable()
    {
        EVMLight.Subscribe(GameEvent.PLAYER_SCORED, StartAnimation);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.PLAYER_SCORED, StartAnimation);
    }

    private void Start()
    {
        initPos = transform.position;
        ownText.alpha = 0;
    }

    private void StartAnimation()
    {
        SetText();
        
        ownText.alpha = 1;
        // Move up
        transform.DOLocalMoveY(transform.localPosition.y + moveDistance, duration)
            .SetEase(Ease.OutCubic);

        // Fade out
        ownText.DOFade(0f, duration)
            .OnComplete(() => Reset());
    }

    private void Reset()
    {
        ownText.alpha = 0;
        transform.position = initPos;
    }

    private void SetText()
    {
        int points = SessionData.Instance.scoreToAdd;

        //use a more efficent way then this control
        bool isPerfectScore = points == 3 && sessionData.currentShootType == ShootType.NET;
        
        //check if it is a temporary bonus
        if (sessionData.currentShootType == ShootType.BACK_BOARD && sessionData.currentTemporaryBonus > 0)
            points = sessionData.currentTemporaryBonus;

        if (sessionData.fireModeIsActive)
            points *= 2;
        
        ownText.text = isPerfectScore 
            ? $"<size=50%>PERFECT SCORE!!!</size>\n+{points}" 
            : $"+{points}";
        
    }

    private SessionData sessionData => SessionData.Instance;
}
