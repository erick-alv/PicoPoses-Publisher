using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayManager : MonoBehaviour
{

    public TMP_Text errorDisplayText;
    public List<TMP_Text> displaysTexts;

    
    // Start is called before the first frame update
    void Start()
    {

        for(int i=0; i<displaysTexts.Count; i++)
        {
            displaysTexts[i].text = string.Format("Display {0}", i);
        }
        
    }

    public void LogRed(string msg)
    {
        errorDisplayText.text = msg;
    }

    public void LogToDisplay(string msg, int disId)
    {
        if (disId >= displaysTexts.Count)
        {
            LogRed(string.Format("The index '{0}' is more than the number of existing displays", disId));
        } else
        {
            displaysTexts[disId].text = msg;
        }
    }
}
