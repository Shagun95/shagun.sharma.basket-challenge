using UnityEngine;

/// <summary>
/// Manage all the visual feedbacks baed on events
/// </summary>
public class FeedbackManager : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem basketParticle;

    private void OnEnable()
    {
        EVMLight.Subscribe(GameEvent.PLAYER_SCORED, PlayBasketParticle);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.PLAYER_SCORED, PlayBasketParticle);
    }

    private void PlayBasketParticle()
    {
        basketParticle.Play();
    }
}
