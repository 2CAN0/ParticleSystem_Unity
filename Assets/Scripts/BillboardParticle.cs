using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardParticle : Particle
{
    Color particleColor;
    bool useRotation;
    float wRotationSpeed;
    float aRotationSpeed;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (useRotation)
        {
            aRotationSpeed = Random.Range(-wRotationSpeed, wRotationSpeed);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (useRotation)
        {
            Transform t = this.transform;
            LookAt(ref t, Camera.main.transform);
            transform.Rotate(transform.forward, RotationSpeed);
        }
        else
            transform.LookAt(Camera.main.transform);
    }

    private void LookAt(ref Transform own, Transform target)
    {
        Quaternion rot = transform.rotation;
        Vector3 rotation = rot.eulerAngles;

        // Y-Rotatie
        float overstaand = Camera.main.transform.position.x + transform.position.x;
        float aanliggend = Camera.main.transform.position.z + transform.position.z;

        float hoek = Mathf.Atan(overstaand / aanliggend) * Mathf.Rad2Deg;
        rotation.y = hoek;

        // X-Rotatie
        overstaand = (transform.position.z + transform.position.x) + (Camera.main.transform.position.z + Camera.main.transform.position.x);
        aanliggend = transform.position.y + Camera.main.transform.position.y;

        hoek = Mathf.Atan(overstaand / aanliggend) * Mathf.Rad2Deg;
        Debug.Log(hoek);
        rotation.x = (hoek < 0) ? hoek + 90 : hoek - 90;

        transform.rotation = Quaternion.Euler(rotation);
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

    public bool UseRotation
    {
        get { return useRotation; }
        set { useRotation = value; }
    }

    public float RotationSpeed
    {
        get { return wRotationSpeed; ; }
        set { wRotationSpeed = value; }
    }
}
