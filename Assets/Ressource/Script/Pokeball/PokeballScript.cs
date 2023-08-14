using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeballScript : MonoBehaviour
{
    private Pokeball pokeball;
    //On condisere que c'est toujours le meme pour tous les pokeball
    [SerializeField] private float time;
    [SerializeField] private float speed;

    private bool hasTriggered;

    public void instantiate(Pokeball _pokeball)
    {
        pokeball = _pokeball;
        GetComponent<SpriteRenderer>().sprite = pokeball.pokeballImg;
    }

    private void Start()
    {
        Destroy(gameObject,time);
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (hasTriggered)
            return;

        if (col.gameObject.CompareTag("Ennemy"))
        {
            MonsterScript monster = col.GetComponent<MonsterScript>();
            monster.CatchMonster(pokeball);
            Destroy(gameObject);
            hasTriggered = true;
        }
    }


}
