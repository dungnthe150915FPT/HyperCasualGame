using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackablePoint : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("I have been hit.");
    }
}
