using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystem : MonoBehaviour
{
    // Particle Variables
    [SerializeField] int maxParticles = 500;
    [SerializeField] float lifetime;
    List<GameObject> particles = new List<GameObject>();
    [SerializeField] GameObject particle;
    [SerializeField] ParticleType particleType;
    [SerializeField] float scale;
    Vector3 ogScale;
    [SerializeField] Vector3 velocity;
    [SerializeField] bool useRandomVelocity;
    [SerializeField] Vector3 rVelocity; // Random bounds of velocity

    // Mesh particles Variables
    [SerializeField] Material particleMaterial;

    // Billboard Particle stuff
    [SerializeField] GameObject billboardObject;
    [SerializeField] Sprite particleSprite;
    [SerializeField] Color particleColor;

    // Particle System settings
    [SerializeField] bool loop = false;
    [SerializeField] bool useRotation = false;
    [SerializeField] Vector3 rotationSpeed;
    [SerializeField] bool useScaleOverLifetime = false;
    [SerializeField] Vector2 scaleGradient;
    private bool playing = false;

    // Physics Settings
    [SerializeField] bool useWind = false;
    [SerializeField] Vector3 wind;
    [SerializeField] bool useGravity = false;
    [SerializeField] float gravity;


    void Awake()
    {
        // particles = new List<GameObject>();
        ogScale = new Vector3(Particle.transform.localScale.x, Particle.transform.localScale.y, Particle.transform.localScale.z);

        if (!useGravity)
            gravity = 0;
        if (useRandomVelocity)
            rVelocity = Vector3.zero;

        if (particleType == ParticleType.Billboard)
        {
            billboardObject = new GameObject();

            billboardObject.gameObject.name = "Billboard Particle";

            ///// Particle transform setup /////
            billboardObject.transform.rotation = Quaternion.identity;
            billboardObject.transform.position = Vector3.zero;
            billboardObject.transform.localScale = new Vector3(1, 1, 1);
            ////////////////////////////////////

            billboardObject.AddComponent<SpriteRenderer>();
            billboardObject.GetComponent<SpriteRenderer>().sprite = particleSprite;
        }
        else if (particleType == ParticleType.MeshParticle)
        {
            if (!Particle.name.ToLower().Contains("particle"))
                Particle.name = Particle.name + " Particle";
        }


        for (int iParticle = 0; iParticle < maxParticles; iParticle++)
        {
            if (particleType == ParticleType.MeshParticle)
            {
                particles.Add(Instantiate(particle));
                try { if (particleMaterial != null) particles[iParticle].GetComponent<Renderer>().material = particleMaterial; } catch { }

                MeshParticle p = particles[iParticle].AddComponent<MeshParticle>();
                p.Lifetime = Lifetime;
                p.PossibleVelocity = Velocity;
                p.RandomVelocity = rVelocity;
                p.UseRotation = useRotation;
                p.RotationSpeed = RotationSpeed;
                p.Gravity = gravity;

                if (useScaleOverLifetime)
                {
                    p.UseScaleOverLife = true;
                    p.ScaleOverLife = scaleGradient;
                }
                else
                    p.UseScaleOverLife = false;
            }
            else if (particleType == ParticleType.Billboard)
            {
                particles.Add(Instantiate(billboardObject));

                BillboardParticle p = particles[iParticle].AddComponent<BillboardParticle>();
                p.Lifetime = Lifetime;
                p.PossibleVelocity = Velocity;
                p.RandomVelocity = rVelocity;
                p.Gravity = gravity;

                if (useRotation)
                {
                    p.UseRotation = true;
                    p.RotationSpeed = rotationSpeed.z;
                }

                if (ParticleColor != null)
                {
                    p.ParticleColor = ParticleColor;
                }

                if (useScaleOverLifetime)
                {
                    p.UseScaleOverLife = true;
                    p.ScaleOverLife = scaleGradient;
                }
                else
                    p.UseScaleOverLife = false;
            }

            particles[iParticle].hideFlags = HideFlags.HideInHierarchy;
            particles[iParticle].transform.SetParent(this.transform);
            particles[iParticle].SetActive(false);
        }

        Destroy(billboardObject);
    }

    void Update()
    {
        if (playing)
        {
            foreach (GameObject particle in particles)
            {
                if (!particle.activeSelf)
                {
                    if (!loop)
                        particle.transform.position = this.transform.position;
                    else
                    {
                        if (particleType == ParticleType.Billboard)
                            particle.GetComponent<BillboardParticle>().ParticleColor = ParticleColor;

                        particle.transform.position = GetRandomLocation(GetComponent<BoxCollider>());
                        if (!useScaleOverLifetime)
                            SetRandomScale(particle);
                        particle.SetActive(true);
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        other.gameObject.SetActive(false);
    }

    Vector3 GetRandomLocation(BoxCollider box)
    {
        Vector3 randomSpawn = Vector3.zero;
        randomSpawn.x = Random.Range(-box.size.x / 2, box.size.x / 2);
        randomSpawn.y = Random.Range(-box.size.y / 2, box.size.y / 2);
        randomSpawn.z = Random.Range(-box.size.z / 2, box.size.z / 2);

        return randomSpawn;
    }

    public void Play()
    {
        playing = true;
        foreach (GameObject particle in particles)
        {
            particle.transform.position = GetRandomLocation(GetComponent<BoxCollider>());
            SetRandomScale(particle);
            particle.SetActive(true);
        }
    }

    public void Pause()
    {
        playing = false;
        foreach (GameObject particle in particles)
        {
            particle.GetComponent<Particle>().Pause();
        }
    }

    public void Stop()
    {
        playing = false;
        foreach (GameObject particle in particles)
        {
            particle.SetActive(false);
            particle.transform.position = transform.position;
        }
    }

    public void Continue()
    {
        playing = true;
        foreach (GameObject particle in particles)
        {
            particle.GetComponent<Particle>().Continue();
        }
    }

    public void SetRandomScale(GameObject particle)
    {
        particle.transform.localScale = ogScale * Random.Range(Scale - 0.2f, Scale + 0.2f);
    }

    // Particle Variables
    public int MaxParticle
    {
        get { return maxParticles; }
        set { maxParticles = value; }
    }

    public float Lifetime
    {
        get { return lifetime; }
        set
        {
            lifetime = value;
            foreach (GameObject pl in particles)
            {
                Particle _pl = pl.GetComponent<Particle>();
                _pl.Lifetime = Lifetime;
            }
        }
    }

    public GameObject Particle
    {
        get { return particle; }
        set { particle = value; }
    }

    public float Scale
    {
        get { return scale; }
        set { scale = value; }
    }

    public bool UseGradientScale
    {
        get { return useScaleOverLifetime; }
        set { useScaleOverLifetime = value; }
    }

    public Vector2 ScaleGradient
    {
        get { return scaleGradient; }
        set
        {
            scaleGradient = value;
            if (scaleGradient.x <= 0)
                scaleGradient.x = 0.000001f;
        }
    }

    public Vector3 Velocity
    {
        get { return velocity; }
        set
        {
            velocity = value;
            foreach (GameObject pl in particles)
            {
                Particle _pl = pl.GetComponent<Particle>();
                _pl.PossibleVelocity = velocity;
            }
        }
    }

    public bool UseRandomVelocity
    {
        get { return useRandomVelocity; }
        set { useRandomVelocity = value; }
    }

    public bool Rotate
    {
        get { return useRotation; }
        set { useRotation = value; }
    }

    public Vector3 RotationSpeed
    {
        get { return rotationSpeed; }
        set { rotationSpeed = value; }
    }

    public Sprite ParticleSprite
    {
        get { return particleSprite; }
        set { particleSprite = value; }
    }

    public ParticleType ParticleMode
    {
        get { return particleType; }
        set { particleType = value; }
    }

    public Color ParticleColor
    {
        get { return particleColor; }
        set { particleColor = value; }
    }

    public bool UseWind
    {
        get { return useWind; }
        set { useWind = value; }
    }

    public Vector3 Wind
    {
        get { return wind; }
        set { wind = value; }
    }

    public bool UseGravity
    {
        get { return useGravity; }
        set { useGravity = value; }
    }

    public float Gravity
    {
        get { return gravity; }
        set { gravity = value; }
    }

    public Vector3 RandomVelocity
    {
        get { return rVelocity; }
        set
        {
            rVelocity = value;
            if (rVelocity.x > 100)
                rVelocity.x = 100;
            else if (rVelocity.x < 0)
                rVelocity.x = 0;

            if (rVelocity.y > 100)
                rVelocity.y = 100;
            else if (rVelocity.y < 0)
                rVelocity.y = 0;

            if (rVelocity.z > 100)
                rVelocity.z = 100;
            else if (rVelocity.z < 0)
                rVelocity.z = 0;

            foreach (GameObject pl in particles)
            {
                Particle _pl = pl.GetComponent<Particle>();
                _pl.RandomVelocity = rVelocity;
            }
        }
    }
}

public enum ParticleType
{
    MeshParticle,
    Billboard
}