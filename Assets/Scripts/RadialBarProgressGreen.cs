using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialBarProgressGreen : MonoBehaviour {

    public Image ImgBar;
    public float AmountD;
    public float fillAmountValue;

    void Awake() {
        AmountD = 100f;
        fillAmountValue = 100f;

    }

    // Update is called once per frame
    void FixedUpdate() {
        if (AmountD > 100f)
        {
            AmountD = 100f;
        }
        if (AmountD > 0)
        {
            AmountD = AmountD - Time.deltaTime;
            Vector3 temp = ImgBar.transform.localScale;
            temp.x = AmountD/100;
            temp.y = AmountD/100;
            temp.z = AmountD/100;
            
            ImgBar.transform.localScale = temp;
            fillAmountValue = AmountD;
        }
    }
}
