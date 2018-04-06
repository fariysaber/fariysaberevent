//This script will only work in editor mode. You cannot adjust the scale dynamically in-game!
using UnityEngine;
using System.Collections;

public class ParticleScaler : MonoBehaviour
{
    public float particleScale = 1.0f;
    public bool alsoScaleGameobject = true;

    public float prevScale = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        if (prevScale != particleScale && particleScale > 0)
        {
            if (alsoScaleGameobject)
                transform.localScale = new Vector3(particleScale, particleScale, particleScale);

            float scaleFactor = particleScale / prevScale;

            //scale legacy particle systems
            ScaleLegacySystems(scaleFactor);

            //scale shuriken particle systems
            ScaleShurikenSystems(scaleFactor);

            //scale trail renders
            ScaleTrailRenderers(scaleFactor);

            prevScale = particleScale;
        }
    }

    void ScaleShurikenSystems(float scaleFactor)
    {
        //get all shuriken systems we need to do scaling on
        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem system in systems)
        {
            system.startSpeed *= scaleFactor;
            system.startSize *= scaleFactor;
            system.gravityModifier *= scaleFactor;
        }
    }

    void ScaleLegacySystems(float scaleFactor)
    {
        //get all emitters we need to do scaling on
        ParticleEmitter[] emitters = GetComponentsInChildren<ParticleEmitter>();

        //get all animators we need to do scaling on
        ParticleAnimator[] animators = GetComponentsInChildren<ParticleAnimator>();

        //apply scaling to emitters
        foreach (ParticleEmitter emitter in emitters)
        {
            emitter.minSize *= scaleFactor;
            emitter.maxSize *= scaleFactor;
            emitter.worldVelocity *= scaleFactor;
            emitter.localVelocity *= scaleFactor;
            emitter.rndVelocity *= scaleFactor;
        }

        //apply scaling to animators
        foreach (ParticleAnimator animator in animators)
        {
            animator.force *= scaleFactor;
            animator.rndForce *= scaleFactor;
        }
    }

    void ScaleTrailRenderers(float scaleFactor)
    {
        //get all animators we need to do scaling on
        TrailRenderer[] trails = GetComponentsInChildren<TrailRenderer>();

        //apply scaling to animators
        foreach (TrailRenderer trail in trails)
        {
            trail.startWidth *= scaleFactor;
            trail.endWidth *= scaleFactor;
        }
    }
}
