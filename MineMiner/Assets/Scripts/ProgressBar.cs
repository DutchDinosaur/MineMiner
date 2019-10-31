using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    public Image progressBar;
    public float endScore;

    // Update is called once per frame
    void Update()
    {
        CheckScore();
    }

    public void CheckScore()
    {
        //progressBar.fillAmount = //hier komt je score value bv: currentScore / endScore; Als endScore 100 is en current score op 25 dan zal de bar voor een 4e gevuld zijn
    }
}
