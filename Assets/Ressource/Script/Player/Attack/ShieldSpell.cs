using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpell : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Spell"))
        {
            Destroy(col.gameObject);
        }
    }
}
