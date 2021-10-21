using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    private Vector3 _offset;

    private void Start()
    {
        //Mendapatkan offset antara target dan camera
        _offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        //Menapatkan posisi untuk camera
        Vector3 targetCamPos = target.position + _offset;
        
        //set posisi camera dengan smoothing
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
