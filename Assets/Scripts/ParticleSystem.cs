using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystem : MonoBehaviour
{
    // Particle Variables
    [SerializeField] int maxParticles = 500;
    [SerializeField] float lifetime;
    List<GameObject> particles;
    [SerializeField] GameObject particle;
    [SerializeField] float scale;
    Vector3 ogScale;
    [SerializeField] Vector3 velocity;
    [SerializeField] ParticleType particleType;

    // Billboard Particle stuff
    [SerializeField] GameObject billboardObject;
    [SerializeField] Sprite particleSprite;
    [SerializeField] Color particleColor;

    // Particle System settings
    [SerializeField] bool loop = false;
    [SerializeField] bool rotate = false;
    [SerializeField] Vector3 rotationSpeed;
    private bool playing = false;



    void Awake()
    {
        particles = new List<GameObject>();
        ogScale = new Vector3(Particle.transform.localScale.x, Particle.transform.localScale.y, Particle.transform.localScale.z);

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

        for (int iParticle = 0; iParticle < maxParticles; iParticle++)
        {
            if (particleType == ParticleType.MeshParticle)
            {
                particles.Add(Instantiate(particle));

                MeshParticle p = particles[iParticle].AddComponent<MeshParticle>();
                p.Lifetime = Lifetime;
                p.PossibleVelocity = Velocity;
                p.UseRotation = rotate;
                p.RotationSpeed = RotationSpeed;
            }
            else if (particleType == ParticleType.Billboard)
            {
                particles.Add(Instantiate(billboardObject));

                BillboardParticle p = particles[iParticle].AddComponent<BillboardParticle>();
                p.Lifetime = Lifetime;
                p.PossibleVelocity = Velocity;

                if (ParticleColor != null)
                {
                    p.ParticleColor = ParticleColor;
                }
            }

            // particles[iParticle].hideFlags = HideFlags.HideInHierarchy;
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
                        if(particleType == ParticleType.Billboard)
                            particle.GetComponent<BillboardParticle>().ParticleColor = ParticleColor;

                        particle.transform.position = GetRandomLocation(GetComponent<BoxCollider>());
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
        set { lifetime = value; }
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

    public Vector3 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    public bool Rotate
    {
        get { return rotate; }
        set { rotate = value; }
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
}

public enum ParticleType
{
    MeshParticle,
    Billboard
}