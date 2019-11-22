using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    // Time Settings
    protected bool paused;
    protected float time2die;
    protected float remainingTime;
    protected float lifetime;

    protected Vector3 pVelocity; // Possible velocity
    protected Vector3 aVelocity; // Actual velocity

    protected virtual void OnEnable()
    {
        time2die = Random.Range(0, lifetime) + Time.time;

        float wSpeed = pVelocity.magnitude; // Wanted Velocity
        
        aVelocity.x = Random.Range(-pVelocity.x, pVelocity.x);
        aVelocity.y = Random.Range(-pVelocity.y, pVelocity.y);
        aVelocity.z = Random.Range(-pVelocity.z, pVelocity.z);

        if (aVelocity.magnitude != wSpeed)
        {
            aVelocity.Normalize();
            aVelocity *= wSpeed;
        }


    }

    protected virtual void Update()
    {
        if (Time.time >= time2die && !paused)
        {
            this.gameObject.SetActive(false);
        }
        else if (!paused)
        {
            transform.position += aVelocity;
        }
    }

    public virtual void Pause()
    {
        paused = true;
        remainingTime = time2die - Time.time;
    }

    public virtual void Continue()
    {
        paused = false;
        time2die = remainingTime + Time.time;
    }

    public virtual float Lifetime
    {
        get { return lifetime; }
        set { lifetime = value; }
    }

    public virtual Vector3 PossibleVelocity
    {
        get { return pVelocity; }
        set { pVelocity = value; }
    }
}
