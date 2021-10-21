using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;
    public PowerUpManager powerUpManager;


    private Animator _anim;
    private CapsuleCollider _capsuleCollider;
    private AudioSource _enemyAudio;
    private ParticleSystem _hitParticles;
    private bool _isDead;
    private bool _isSinking;
    private static readonly int Dead = Animator.StringToHash("Dead");


    private void Awake()
    {
        powerUpManager = FindObjectOfType<PowerUpManager>();
        //Menapatkan reference komponen
        _anim = GetComponent<Animator>();
        _enemyAudio = GetComponent<AudioSource>();
        _hitParticles = GetComponentInChildren<ParticleSystem>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

        //Set current health
        currentHealth = startingHealth;
    }


    private void Update()
    {
        //Check jika sinking
        if (_isSinking) transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
    }


    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        //Check jika dead
        if (_isDead) return;

        //play audio
        _enemyAudio.Play();

        //kurangi health
        currentHealth -= amount;

         //Ganti posisi particle
        _hitParticles.transform.position = hitPoint;
        
         //Play particle system
        _hitParticles.Play();

        //Dead jika health <= 0
        if (currentHealth <= 0) Death();
    }


    private void Death()
    {
        powerUpManager.SpawnOnDeath(gameObject.transform);
        
        //set isdead
        _isDead = true;

         //SetCapcollider ke trigger
        _capsuleCollider.isTrigger = true;

        //trigger play animation Dead
        _anim.SetTrigger(Dead);

        //Play Sound Dead
        _enemyAudio.clip = deathClip;
        _enemyAudio.Play();
    }


    public void StartSinking()
    {
        //disable Navmesh Component
        GetComponent<NavMeshAgent>().enabled = false;
        
        //Set rigisbody ke kimematic
        GetComponent<Rigidbody>().isKinematic = true;
        _isSinking = true;
        ScoreManager.score += scoreValue;
        Destroy(gameObject, 2f);
    }
}
