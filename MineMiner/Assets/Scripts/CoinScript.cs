using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinScript : MonoBehaviour
{

    Text text;
    public int coinAmount;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>(); 
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Total earned +" + coinAmount.ToString();
    }


}
