using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentToZero : MonoBehaviour
{
    void Start()
    {
        ReturnZero();
    }

    public void ReturnZero()
    {
        RectTransform contentRectTransform = gameObject.GetComponent<ScrollRect>().content;

        // Définir le pivot pour placer le contenu tout en haut.
        contentRectTransform.pivot = new Vector2(contentRectTransform.pivot.x, 1.0f);
        // Assurez-vous également que la position verticale est correcte.
        contentRectTransform.anchoredPosition = new Vector2(contentRectTransform.anchoredPosition.x, 0);

    }

}
