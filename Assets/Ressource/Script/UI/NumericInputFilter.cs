using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumericInputFilter : MonoBehaviour
{
    private InputField inputField;

    private void Start()
    {
        inputField = GetComponent<InputField>();
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    }

    private void OnInputFieldValueChanged(string value)
    {
        // Vérifier chaque caractère dans la chaîne d'entrée
        string filteredValue = "";
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];

            // Vérifier si le caractère est un chiffre et n'est pas un zéro seul
            if (char.IsDigit(c) && !(i == 0 && c == '0'))
            {
                filteredValue += c;
            }
        }

        // Mettre à jour la valeur du champ de saisie avec la chaîne filtrée
        inputField.text = filteredValue;

        // Vérifier si la chaîne filtrée est vide, alors mettre la valeur par défaut à "1"
        if (string.IsNullOrEmpty(filteredValue))
        {
            inputField.text = "1";
        }
    }
}


