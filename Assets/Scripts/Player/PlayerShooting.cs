using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 5f;
    public float range = 100f;
    public int maxBullets = 100;

    public Slider bulletSlider;
    
    
    private readonly float _effectsDisplayTime = 0.2f;
    private AudioSource _gunAudio;
    private Light _gunLight;
    private LineRenderer _gunLine;
    private ParticleSystem _gunParticles;
    private int _shootableMask;
    private RaycastHit _shootHit;
    private Ray _shootRay;

    private float _timer;

    private void Awake()
    {
        //GetMask
        _shootableMask = LayerMask.GetMask("Shootable");
        
        //Mendapatkan Reference component
        _gunParticles = GetComponent<ParticleSystem>();
        _gunLine = GetComponent<LineRenderer>();
        _gunAudio = GetComponent<AudioSource>();
        _gunLight = GetComponent<Light>();

        
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= timeBetweenBullets * _effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        //disable line renderer
        _gunLine.enabled = false;
        
        //disable light
        _gunLight.enabled = false;
    }

    public void Shoot()
    {
        
        
        

        _timer = 0f;

         //Play audio
        _gunAudio.Play();
        
        //enable Light
        _gunLight.enabled = true;

        //Play gun particle
        _gunParticles.Stop();
        _gunParticles.Play();

        //enable Line renderer dan set first position
        _gunLine.enabled = true;
        _gunLine.SetPosition(0, transform.position);

        //Set posisi ray shoot dan direction
        _shootRay.origin = transform.position;
        _shootRay.direction = transform.forward;

        //Lakukan raycast jika mendeteksi id nemy hit apapun
        if (Physics.Raycast(_shootRay, out _shootHit, range, _shootableMask))
        {
            //Lakukan raycast hit hace component Enemyhealth
            var enemyHealth = _shootHit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                //Lakukan Take Damage
                enemyHealth.TakeDamage(damagePerShot, _shootHit.point);
            }
            
            //Set line end position ke hit position
            _gunLine.SetPosition(1, _shootHit.point);
        }
        else
        {
            //set line end position ke range freom barrel
            _gunLine.SetPosition(1, _shootRay.origin + _shootRay.direction * range);
        }
    }

    
}
