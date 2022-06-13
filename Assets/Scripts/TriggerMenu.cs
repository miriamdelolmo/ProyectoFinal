using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerLvl1 : MonoBehaviour
{
    private Rigidbody2D rb;

    private void OnTriggerEnter2D( Collider2D collision ){
        if(collision.CompareTag("Player")){
            rb = GetComponent<Rigidbody2D>();
            /*//if(collision.transform.gameObject.name == "Scene1TRIGGER")
            if(collision.gameObject.tag=="Trigger1")
            {
                SceneManager.LoadScene("Level1");
            }*/
            if(rb.position.x<0){
                SceneManager.LoadScene("Level1");
            }
        }
    }
}
