using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    private Animator _anim;

    private bool _damaged;

    private PlayerShooting _playerShooting;
    private bool _isDead;
    private AudioSource _playerAudio;
    private PlayerMovement _playerMovement;
    private static readonly int Die = Animator.StringToHash("Die");


    private void Awake()
    {
        //Mendapatkan refernce komponen
        _anim = GetComponent<Animator>();
        _playerAudio = GetComponent<AudioSource>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerShooting = GetComponentInChildren<PlayerShooting>();

        currentHealth = startingHealth;
    }


    private void Update()
    {
        //Jika terkena damaage
        if (_damaged)
        {
            //Merubah warna gambar menjadi value dari flashColour
            damageImage.color = flashColour;
        }
        else
        {
            //Fade out damage image
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        //Set damage to false
        _damaged = false;
    }

    //fungsi untuk mendapatkan damage
    public void TakeDamage(int amount)
    {
        _damaged = true;
        //mengurangi health
        UpdateHealth(-amount);
        
        //Memainkan suara ketika terkena damage
        _playerAudio.Play();

         //Memanggil method Death() jika darahnya kurang dari sama dengan 10 dan belu mati
        if (currentHealth <= 0 && !_isDead)
        {
            Death();
        }
    }

    public void Heal(int amount)
    {
        UpdateHealth(amount);
    }

    private void Death()
    {
        _isDead = true;

        _playerShooting.DisableEffects();
        
        //mentrigger animasi Die
        _anim.SetTrigger(Die);

        //Memainkan suara ketika mati
        _playerAudio.clip = deathClip;
        _playerAudio.Play();

        //mematikan script player movement
        _playerMovement.enabled = false;
        _playerShooting.enabled = false;
    }

    private void UpdateHealth(int healthValue)
    {
        int newHealth = currentHealth += healthValue;
        currentHealth = Mathf.Clamp(newHealth, 0, startingHealth);
        healthSlider.value = currentHealth;
    }
}
