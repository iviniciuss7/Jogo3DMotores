using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour{
    public Text textHealth;
    public int scoreCoin;
    public Text scoreText;

    public static GameController instance;
    
    void Awake(){
        
        instance = this;
    }

   

    public void UpdateScore(int value){
        scoreCoin += value;
        scoreText.text = scoreCoin.ToString();
    }


    public void UpdateLives(int value)
    {
        textHealth.text = "x " + value.ToString();
    }

}
