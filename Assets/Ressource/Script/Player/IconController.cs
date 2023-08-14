using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconController : MonoBehaviour
{
    [SerializeField] private GameObject KeyCodeObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void ChangeTextIcon(string texte,bool active)
    {
        KeyCodeObject.SetActive(active);
        KeyCodeObject.transform.GetChild(0).GetComponent<Text>().text = texte;
    }
}
