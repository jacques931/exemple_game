using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasButton : MonoBehaviour
{
    [SerializeField] private GameObject informationButton;

    public void MouseEnter(GameObject button)
    {
        informationButton.SetActive(true);
        Vector3 newPosition = new Vector3(button.transform.position.x,button.transform.position.y - 50,button.transform.position.z);
        informationButton.transform.position = newPosition;
        informationButton.transform.GetChild(0).GetComponent<Text>().text = button.name;
    }

    public void MouseExit()
    {
        informationButton.SetActive(false);
    }
}
