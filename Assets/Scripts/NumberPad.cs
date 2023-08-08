using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using TMPro;

public class NumberPad : MonoBehaviour
{

    public TMP_InputField inputField;
    // Start is called before the first frame update

    public void PushNumb(string numberStr)
    {
        inputField.text += numberStr;
    }
    public void Del()
    {
        inputField.text = "";
    }

    public void BackSpace()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        } 
    }
}
