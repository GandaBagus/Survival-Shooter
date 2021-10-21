using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    private Animator _anim;
    private GameObject _player;
    private PlayerHealth _playerHealth;
    private EnemyHealth _enemyHealth;
    private bool _playerInRange;
    private float _timer;
    
    private static readonly int PlayerDead = Animator.StringToHash("PlayerDead");


    private void Awake()
    {
        //Mencari game object dengan tag "Player"
        _player = GameObject.FindGameObjectWithTag("Player");
        
        //mendapatkan komponen player health
        _playerHealth = _player.GetComponent<PlayerHealth>();
        _enemyHealth = GetComponent<EnemyHealth>();
        
        //mendapatkan komponen Animator
        _anim = GetComponent<Animator>();
    }


    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= timeBetweenAttacks && _playerInRange && _enemyHealth.currentHealth > 0)
        {
            Attack();
        }
        
        //mentrigger animasi PlayerDead jika darah player kurang dari sama dengan 0
        if (_playerHealth.currentHealth <= 0)
        {
            _anim.SetTrigger(PlayerDead);
        }
    }

     //Callback jika ada suatu object masuk kedalam trigger
    private void OnTriggerEnter(Collider other)
    {
           //Set player in range
        if (other.gameObject == _player && other.isTrigger == false)
        {
            _playerInRange = true;
        }
    }

    //Callback jika ada object yang keluar dari trigger
    private void OnTriggerExit(Collider other)
    {
        //Set player not in range
        if (other.gameObject == _player)
        {
            _playerInRange = false;
        }
    }


    private void Attack()
    {
        //Reset timer
        _timer = 0f;

        //Taking Damage
        if (_playerHealth.currentHealth > 0)
        {
            _playerHealth.TakeDamage(attackDamage);
        }
    }
}
