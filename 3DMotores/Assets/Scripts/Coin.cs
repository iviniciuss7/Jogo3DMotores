using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider colCoin){
        
        if (colCoin.gameObject.tag == "Player"){
            GameController.instance.UpdateScore(coinValue);
            Destroy(gameObject);
        }
    }

}
