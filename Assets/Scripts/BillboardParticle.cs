using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardParticle : Particle
{
    Color particleColor;
    protected override void OnEnable()
    {
        time2die = Random.Range(0, lifetime) + Time.time;

        float wSpeed = pVelocity.magnitude; // Wanted Velocity
        if (aVelocity.magnitude != wSpeed)
        {
            aVelocity.Normalize();
            aVelocity *= wSpeed;
        }
    }

    protected override void Update()
    {
        base.Update();

        this.transform.LookAt(Camera.main.transform);
    }

    public Color ParticleColor
    {
        get { return particleColor; }
        set
        {
            particleColor = value;
            GetComponent<SpriteRenderer>().color = particleColor;
        }
    }
}
