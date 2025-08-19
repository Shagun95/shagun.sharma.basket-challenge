using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Manage all the visual feedbacks based on events
/// </summary>
public class FeedbackManager : MonoBehaviour
{

    [SerializeField]
    private AudioSource ownAudioSource;

    [SerializeField, BoxGroup("Particles")]
    private ParticleSystem basketParticle, winParticle;

    [SerializeField, BoxGroup("Sound")]
    private AudioClip score, lost, applause;

    private bool audioIsOn => SessionData.Instance.audioOn;

    private void OnEnable()
    {
        EVMLight.Subscribe(GameEvent.PLAYER_SCORED, PlayerScoredEffects);
        EVMLight.Subscribe(GameEvent.GAME_FINISHED, GameFinishedParticle);
    }

    private void OnDisable()
    {
        EVMLight.Unsubscribe(GameEvent.PLAYER_SCORED, PlayerScoredEffects);
        EVMLight.Unsubscribe(GameEvent.GAME_FINISHED, GameFinishedParticle);
    }

    /// <summary>
    /// Player scored
    /// </summary>
    private void PlayerScoredEffects()
    {
        if (audioIsOn)
            ownAudioSource.PlayOneShot(score);

        basketParticle.Play();
    }

    /// <summary>
    /// Game is finished, manage win and lose cases
    /// </summary>
    private void GameFinishedParticle()
    {
        if (SessionData.Instance.playerScoreForThisRound > SessionData.Instance.AIScoreForThisRound)
        {
            //particle for win
            if (audioIsOn)
                ownAudioSource.PlayOneShot(applause);
                
            winParticle.Play();
            
            
        }
        else
        {
            //particle for lose
            if (audioIsOn)
                ownAudioSource.PlayOneShot(lost);
        }
    }
}
