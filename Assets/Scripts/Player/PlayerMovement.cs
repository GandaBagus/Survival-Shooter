using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    private float _normalSpeed;
    private Vector3 _movement;
    private Animator _animator;
    private Rigidbody _playerRigidbody;
    private int _floorMask;
    private float _camRayLength = 100f;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");


    private void Awake()
    {
        _normalSpeed = speed;
        
        //mendapatkan nilai mask dari layer yang bernama Floor
        _floorMask = LayerMask.GetMask("Floor");
        
        //Mendapatkan komponen Animator
        _animator = GetComponent<Animator>();
        
        //Mendapatkan komponen Rigidbody
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Turning();
    }

    public void Move(float horizontal, float vertical)
    {
        if (GetComponent<PlayerHealth>().currentHealth <= 0)
        {
            return;
        }
        
        _movement.Set(horizontal, 0f, vertical);
        
        //Menormalisasi nilai vector agar total panjang dari vector adalah 1
        _movement = _movement.normalized * speed * Time.deltaTime;
        
         //Move to position
        _playerRigidbody.MovePosition(transform.position + _movement);
    }

    public void Turning()
    {
    //Buat Ray dari posisi mouse di layar
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        //Buat raycast untuk floorHit
        RaycastHit floorHit;
        
        //Lakukan raycast
        if (Physics.Raycast(camRay, out floorHit, _camRayLength, _floorMask))
        {
            //Mendapatkan vector daro posisi player dan posisi floorHit
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            
            //Mendapatkan look rotation baru ke hit position
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            
            //Rotasi player
            _playerRigidbody.MoveRotation(newRotation);
        }
    }

    public void Animating(float h, float v)
    {
        bool walking = h != 0 || v != 0f;
        _animator.SetBool(IsWalking, walking);
    }

    public void SpeedUp(float speedUpAmount, float speedupTime)
    {
        speed *= speedUpAmount;

        StartCoroutine(ResetSpeed(speedupTime));
    }

    private IEnumerator ResetSpeed(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        speed = _normalSpeed;
    }
}
