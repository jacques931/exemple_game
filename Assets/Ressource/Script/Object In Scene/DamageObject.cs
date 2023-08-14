using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] private int damagePourcent;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            PlayerState playerState = col.GetComponent<PlayerState>();
            playerState.ApplyDamageObject(damagePourcent);
        }
    }
}
