using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialBarProgressRed : MonoBehaviour {

    public Image ImgBar;
    //public Text TxtProgress;
    //private float WholeAmount = 60f;
    public float AmountD;
    public float fillAmountValue;


    //[Range(0, 100)] public int Amount = 0;

    // Use this for initialization
    void Awake() {
        AmountD = 100f;
        fillAmountValue = 100f;
        //ImgBar = GameObject.Find("ImgBG").GetComponent<Image>();
        //ImgBar.fillAmount = WholeAmount;

    }

    // Update is called once per frame
    void FixedUpdate() {
        if (AmountD > 100f) {
            AmountD = 100f;
        }
        if (AmountD > 0) {
            AmountD = AmountD - Time.deltaTime;
            Vector3 temp = ImgBar.transform.localScale;
            temp.x = AmountD / 100;
            temp.y = AmountD / 100;
            temp.z = AmountD / 100;

            //AmountD -= Time.deltaTime;
            //ImgBar.fillAmount = AmountD / WholeAmount;
            ImgBar.transform.localScale = temp;
            fillAmountValue = AmountD;
            //fillAmountValue = ImgBar.fillAmount;
            //TxtProgress.text = string.Format(" {0}%", Convert.ToInt32(AmountD / WholeAmount * 100));
        }

        //1. + dlya togo chtobi uvelichit 
        //    2. bolle maximuma
    }

    //public void OnValidate()
    //{
    //    float amount = Amount / 100.0f;
    //    ImgBar.fillAmount = amount;
    //    TxtProgress.text = string.Format("{0}", Amount);
    //}




}
