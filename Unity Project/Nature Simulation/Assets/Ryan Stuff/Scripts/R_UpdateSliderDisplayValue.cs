using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class R_UpdateSliderDisplayValue : MonoBehaviour
{
    private Slider slider;
    public TMP_Text valueDisp;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        valueDisp.text = slider.value.ToString();

        int length = valueDisp.text.Length;
        if(length > 5) { length = 5; }

        valueDisp.text = valueDisp.text.Substring(0, length);
    }
}
