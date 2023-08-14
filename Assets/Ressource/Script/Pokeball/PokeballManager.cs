using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokeballManager : MonoBehaviour
{
    [SerializeField] private PokeballDatabase pokeballDatabase;
    [SerializeField] private Image pokeballIcon;
    [SerializeField] private Image pokeballIconWait;

    private Sprite startPokeballIcon;
    private Pokeball pokeball;
    private int currentItemId;
    private bool isBarItem;

    private void Start()
    {
        startPokeballIcon = pokeballIcon.sprite;
        bool pokeballInBar = PlayerPrefs.GetInt("pokeballInBar") != 0;
        SetPokeball(PlayerPrefs.GetInt("pokeballItemId"),PlayerPrefs.GetInt("pokeballId"),pokeballInBar);
            
    }

    public (int,Pokeball) GetPokeball()
    {
        return (currentItemId,pokeball);
    }

    public void WaitNextPokeball(float pourcent)
    {
        pokeballIconWait.fillAmount = pourcent;
    }

    public void DeletePokeball()
    {
        if(currentItemId!=-0)
        {
            SoundManager.instance.Sound(0);
            currentItemId=-1;
            CheckHavePokeball();
        }
    }

    public void CheckHavePokeball()
    {
        if((!isBarItem && CanvasManager.instance.inventory.CheckItemInInventory(currentItemId)==-1) || (isBarItem && CanvasManager.instance.itemBar.CheckItemInBar(currentItemId)==-1))
        {
           currentItemId = 0;
           pokeball = null;
           PlayerPrefs.SetInt("pokeballId",0);
           PlayerPrefs.SetInt("pokeballItemId",0);
           PlayerPrefs.SetInt("pokeballInBar",0);
           pokeballIcon.sprite = startPokeballIcon;
        }
    }

    public void SetPokeball(int idItem,int idPokeball,bool _isBarItem)
    {
        if((!_isBarItem && CanvasManager.instance.inventory.CheckItemInInventory(idItem)!=-1) || (_isBarItem && CanvasManager.instance.itemBar.CheckItemInBar(idItem)!=-1))
        {
            PlayerPrefs.SetInt("pokeballId",idPokeball);
            PlayerPrefs.SetInt("pokeballItemId",idItem);
            int boolValue = _isBarItem ? 1 : 0;
            PlayerPrefs.SetInt("pokeballInBar",boolValue);

            pokeball = pokeballDatabase.pokeball[idPokeball];;
            currentItemId = idItem;
            isBarItem = _isBarItem;
            
            pokeballIcon.sprite = pokeball.pokeballImg;
        }
        
    }
}
