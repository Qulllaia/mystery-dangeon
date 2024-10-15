using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LazerTimeout : MonoBehaviour
{
    private GameObject player;
    private float _rotateVector = 0f;
    float fPPos;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        fPPos = player.transform.position.y; 
        StartCoroutine(timeout());
    }
    IEnumerator timeout(){
        StartCoroutine(activateTrigger());
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
    IEnumerator activateTrigger(){
        yield return new WaitForSeconds(1f);
        if(player.transform.position.x > transform.position.x ){
            if(transform.position.y > player.transform.position.y){
                _rotateVector = -1f;
            }
            else{
                _rotateVector = 1f;
            }
        }
        else{
            if(transform.position.y > player.transform.position.y){
                _rotateVector = 1f;
            }
            else{
                _rotateVector = -1f;
            }
        }
        GetComponent<BoxCollider2D>().enabled = true; 
        Debug.Log("ACT");
    }
    void Update(){
        transform.Rotate(new Vector3(0,0,_rotateVector));

    }
}
