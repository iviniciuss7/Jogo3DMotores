using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioSource srcCoin;
    public int coinValue;
    void Awake()
    {
        srcCoin = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider colCoin)
    {

        if (colCoin.gameObject.tag == "Player")
        {
            srcCoin.Play();
            GameController.instance.UpdateScore(coinValue);
            Destroy(gameObject);
            Debug.Log("Bateu");
        }
    }
}
