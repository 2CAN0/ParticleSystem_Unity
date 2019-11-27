using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshParticle : Particle
{

    bool useRotation = false;
    Vector3 aRotationSpeed; // Actual Rotation speed
    Vector3 pRotationSpeed; // Possible Rotation speed


    protected override void OnEnable()
    {
        base.OnEnable();

        if (useRotation)
        {
            gameObject.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

            float wRotationS = pRotationSpeed.magnitude; // Wanted Rotation Speed
            
            aRotationSpeed.x = Random.Range(-pRotationSpeed.x, pRotationSpeed.x);
            aRotationSpeed.y = Random.Range(-pRotationSpeed.y, pRotationSpeed.y);
            aRotationSpeed.z = Random.Range(-pRotationSpeed.z, pRotationSpeed.z);
            
            if (aRotationSpeed.magnitude != wRotationS)
            {
                aRotationSpeed.Normalize();
                aRotationSpeed *= wRotationS;
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (!paused && useRotation)
        {
            transform.Rotate(aRotationSpeed);
        }
    }

    public bool UseRotation
    {
        get { return useRotation; }
        set { useRotation = value; }
    }

    public Vector3 RotationSpeed
    {
        get { return aRotationSpeed; }
        set
        {
            aRotationSpeed = value;
            pRotationSpeed = value;
        }
    }
}
