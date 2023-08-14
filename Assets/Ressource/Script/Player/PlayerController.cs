using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject pokeballPrefs;
    private int idItemPokeball;
    private Pokeball pokeball;
    private bool isCatch;

    // Update is called once per frame
    void Update()
    {
        if(!GetComponent<PlayerMove>().GetStopMove())
        {
            if(Input.GetKeyDown(KeyCode.R) && !isCatch)
            {
                (idItemPokeball,pokeball) = CanvasManager.instance.pokeballManager.GetPokeball();

                if(idItemPokeball!=0 && pokeball!=null)
                {
                    StartCoroutine(ThrowPokeball(pokeball.timeThrow));
                    
                    if(PlayerPrefs.GetInt("pokeballInBar")==0)
                        CanvasManager.instance.inventory.RemoveItem(idItemPokeball);
                    else
                        CanvasManager.instance.itemBar.RemoveItem(idItemPokeball);
                    CanvasManager.instance.pokeballManager.CheckHavePokeball();
                }
            }
            
        }
    }

    private IEnumerator ThrowPokeball(float waitTime)
    {
        SoundManager.instance.Sound(19);
        float addPos = (transform.rotation.y !=0 ? -0.45f : 0.45f);
        Vector3 position = transform.position + new Vector3(addPos, 0.1f, 0);
        GameObject pokeballObject = Instantiate(pokeballPrefs,position,transform.rotation);
        pokeballObject.GetComponent<PokeballScript>().instantiate(pokeball);
        isCatch = true;
        
        waitTime  *= (1 - ItemManagerScene.instance.state[0]/100);
        // Comptage le temps restant proportionnellement entre 0 et 1
        float startTime = Time.time;
        float endTime = startTime + waitTime;

        while (Time.time < endTime)
        {
            float timeRemaining = endTime - Time.time;
            float normalizedTime = (timeRemaining / waitTime);

            CanvasManager.instance.pokeballManager.WaitNextPokeball(normalizedTime);

            yield return null; // Attendre jusqu'à la prochaine frame
        }

        isCatch = false;
    }
}
