using UnityEngine;

public enum EffectType
{
    Jump,
    Dash,
    Flash,
    Dead,
    Implosion,
    NextStage,
    Clear,
    Explosion
}

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem jumpParticle;
    [SerializeField] ParticleSystem dashParticle;
    [SerializeField] ParticleSystem flashParticle;
    [SerializeField] ParticleSystem deadParticle;
    [SerializeField] ParticleSystem implosionParticle;
    [SerializeField] ParticleSystem NextStageParticle;
    [SerializeField] ParticleSystem ClearParticle;
    [SerializeField] ParticleSystem ExplosionParticle;




    public void TriggerParticle(EffectType type)
    {
        switch (type)
        {
            case EffectType.Jump:
                // 더블 점프 파티클 효과 실행
                jumpParticle.Play();
                break;
            case EffectType.Dash:
                // 대시 파티클 효과 실행
                dashParticle.Play();
                break;
            case EffectType.Flash:
                // 플래시 파티클 효과 실행
                flashParticle.Play();
                break;
            case EffectType.Dead:
                deadParticle.Play();
                break;
            case EffectType.Implosion:
                implosionParticle.Play();
                break;
            case EffectType.NextStage:
                NextStageParticle.Play();
                break;
            case EffectType.Clear:
                ClearParticle.Play();
                break;
            case EffectType.Explosion:
                ExplosionParticle.Play();
                break;

            default:
                break;
        }
    }
}
