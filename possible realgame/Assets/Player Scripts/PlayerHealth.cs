using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Transform spawnPoint;
    private Transform playerPos;
    int maxHealth = 100;
    public int playerHealth;
    void Start()
    {
        playerHealth = maxHealth;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
    }

    // Update is called once per frame
    public void PlayerTakeDamage()
    {
        if (playerHealth <= 0)
        {

            playerHealth = maxHealth;
            playerPos.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);

        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            playerHealth = 0;
            
            PlayerTakeDamage();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            playerHealth = 0;
          
            PlayerTakeDamage();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            playerHealth = 0;
          
            PlayerTakeDamage();
        }

    }
}
