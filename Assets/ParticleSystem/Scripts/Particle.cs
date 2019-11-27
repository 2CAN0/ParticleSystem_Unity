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
    protected float aLifetime;

    // x is the start value and y is the max scale value
    protected Vector2 pScaleOLife; // possible scale over lifetime
    protected Vector2 aScaleOLife; // actual scale over lifetime
    protected Vector3 startScale;
    protected Vector3 currentScale;
    protected bool useScaleOLife;

    protected Vector3 pVelocity; // Possible velocity
    protected Vector3 rVelocity; // Randomization value of velocity
    protected Vector3 aVelocity; // Actual velocity
    protected float gravity;

    protected virtual void OnEnable()
    {
        aLifetime = Random.Range(0, lifetime);
        time2die = aLifetime + Time.time;

        float wSpeed = pVelocity.magnitude; // Wanted Velocity

        aVelocity.x = Random.Range(-pVelocity.x * (rVelocity.x / 100.0f), pVelocity.x);
        aVelocity.y = Random.Range(-pVelocity.y * (rVelocity.y / 100.0f), pVelocity.y);
        aVelocity.z = Random.Range(-pVelocity.z * (rVelocity.z / 100.0f), pVelocity.z);

        if (aVelocity.magnitude != wSpeed)
        {
            aVelocity.Normalize();
            aVelocity *= wSpeed;
        }

        if (UseScaleOverLife)
        {
            float maxReach = pVelocity.magnitude;
            float aReach = aVelocity.magnitude;

            aScaleOLife.y = pScaleOLife.y * (aReach / maxReach);

            if(transform.localScale == Vector3.zero)
                transform.localScale = new Vector3(1, 1, 1);

            transform.localScale.Normalize();
            startScale = transform.localScale;
            transform.localScale = startScale * ScaleOverLife.x;
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
            transform.position += aVelocity * Time.deltaTime;
            aVelocity.y -= gravity * Time.deltaTime;
        }

        if (UseScaleOverLife)
        {
            float perc = (aScaleOLife.y - aScaleOLife.x)  * (Time.time / time2die);
            transform.localScale = startScale * perc;

            if(transform.localScale.magnitude > aScaleOLife.y){
                transform.localScale.Normalize();
                transform.localScale *= aScaleOLife.y;
            }
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

    public virtual float Gravity
    {
        get { return gravity; }
        set { gravity = value; }
    }

    public virtual Vector3 RandomVelocity
    {
        get { return rVelocity; }
        set { rVelocity = value; }
    }

    public virtual bool UseScaleOverLife
    {
        get { return useScaleOLife; }
        set { useScaleOLife = value; }
    }

    public virtual Vector2 ScaleOverLife
    {
        get { return pScaleOLife; }
        set { pScaleOLife = value; }
    }
}
