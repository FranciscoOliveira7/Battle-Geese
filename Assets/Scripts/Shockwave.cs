using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class Shockwave : MonoBehaviour
{
    //attributes of the shockwave which we can customize
    public int points;
    public float maxRadius;
    public float speed;
    public float startWidth;
    public float force;
    public float damage;
    public float thiccness;
    private float currentRadius;
    
    [SerializeField] AudioClip _sound;
    [SerializeField] ParticleSystem GroundParticles;
    ParticleSystem _particlesInstance;
    
    private LineRenderer linerender;
    private CinemachineImpulseSource _impulseSource;
    
    private List<HealthComponent> entities;
    
    // private List<IDamageable> entities;

    private void Awake()
    {
        entities = new List<HealthComponent>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        linerender = GetComponent<LineRenderer>(); //sets the number of points for line renderer to make a circle
        linerender.positionCount = points + 1;
        StartCoroutine(Blast());
    }

    private IEnumerator Blast() //responsible for blast effect
    {
        CameraShakeManager.instance.CameraShake(_impulseSource);
        SoundManager.instance.PlayClip(_sound, transform, 0.08f);
        SpawnParticles();
        currentRadius = 0f; // finds the current radius of shockwave
        while(currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed; //increases the radius till it reaches the target radius
            Draw(currentRadius);
            Damage(currentRadius);
            yield return null;
        }
        Destroy(gameObject);
    }
    
    private void Damage(float currentRadius) //responsible for the force exerted on objects
    {
        Collider[] _colliders = Physics.OverlapSphere(transform.position, currentRadius, 1<<3);

        for(int i = 0; i < _colliders.Length; i++)
        {
            if (_colliders[i].TryGetComponent(out HealthComponent entity))
            {
                if (_colliders[i].TryGetComponent(out Player player))
                {
                    if (player.IsDashing || entities.Contains(entity)) continue;
                    
                    float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

                    if (distanceFromPlayer < currentRadius - thiccness) continue;
                }
                
                entities.Add(entity);
                entity.Damage(damage, _colliders[i].transform.position - transform.position, null);
                if (_colliders[i].TryGetComponent(out Rigidbody rb))
                {
                    Vector3 direction = (_colliders[i].transform.position - transform.position).normalized;
                    rb.AddForce(direction * force, ForceMode.Impulse); //adds an impulse to every object that comes into contact
                }
            }
        }
    }


    private void Draw(float currentRadius) //responsible for drawing the shockwave
    {
        float anglebetween = 360f / points;

        for(int i=0; i <= points; i++) //in this part the shockwave travels with respect to the direction and position of the points
        {
            float angle = i * anglebetween * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
            Vector3 position = direction * currentRadius; 

            linerender.SetPosition(i, position); 
        }

        linerender.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius/maxRadius);
    }
    
    private void SpawnParticles()
    {
        _particlesInstance = Instantiate(GroundParticles, transform.position, Quaternion.identity);
        _particlesInstance.Play();
    }

    // private void Update()
    // {
    //     if(Input.GetKeyDown("space")) //Shockwave starts on pressing space
    //     {
    //         StartCoroutine(Blast());
    //         
    //     }
    //     
    // }

}
