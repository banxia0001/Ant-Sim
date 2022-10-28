using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Slider slider;
    //public TMP_Text barText;

    private float valueNow;
    private float valueShould;
    private float valueMax;


    public void SetValue_Initial(float valueCurrent)
    {
        valueNow = valueCurrent;
    }

    public void SetValue(float valueCurrent, float value)
    {
        valueShould = valueCurrent;
        valueMax = value;


        //if (barText != null)
        //    barText.text = valueCurrent + " / " + value;

    }


    void FixedUpdate()
    {
        slider.maxValue = valueMax;
        slider.value = valueNow;


        if (valueNow > valueShould) valueNow -= valueMax / 75;
        if (valueNow < valueShould) valueNow += valueMax / 75;
    }
}
