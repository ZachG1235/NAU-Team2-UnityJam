using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class BathtubHide : MonoBehaviour
{
    [SerializeField] private bool touching_player = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is 
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }   

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "PlayerCapsule")
        {
            touching_player = true;
        }
        else
        {
            touching_player = false;
        }
        
    }

}
