using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpiderBehaviour : MonoBehaviour
{
    public GameObject[,] gameObjectPathPoints = new GameObject[41,21];
    public GameObject item;
    public float speed = 1f;
    private Animator anim;
    [SerializeField] private GameObject smokeEffect;

    [SerializeField] private int health = 3;
    [SerializeField] private GameObject _web;

    private SpriteRenderer sr;
    private GameObject player;
    private Vector3 randomEndpoint;
    int ShootCounter;

    int _currentPosX;
    int _currentPosY;

    private bool avaibleToAttack = true;
    void OnEnable(){
        item = gameObject;
        anim = GetComponent<Animator>();
        anim.SetBool("isRunning",true);
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
    }

    void FixedUpdate(){
        if(player != null){
            if(randomEndpoint != new Vector3(0,0,0)){
                item.transform.position = Vector3.MoveTowards(item.transform.position, randomEndpoint, Time.deltaTime * speed);

                if(Vector3.Distance(item.transform.position, randomEndpoint) < 0.2f) {
                    // StartCoroutine(Stay());
                    int randomPosX = new System.Random().Next(0, gameObjectPathPoints.GetLength(0)-1);
                    int randomPosY = new System.Random().Next(0, gameObjectPathPoints.GetLength(1)-1);
                    if(gameObjectPathPoints[randomPosX,randomPosY] != null){
                        randomEndpoint = gameObjectPathPoints[randomPosX,randomPosY].transform.position;
                    }

                }

                if(item.transform.position.x - randomEndpoint.x < 0){
                    sr.flipX = true;
                }else{
                    sr.flipX = false;
                }                
            }
            else{
                int randomPosX = new System.Random().Next(0, gameObjectPathPoints.GetLength(0)-1);
                int randomPosY = new System.Random().Next(0, gameObjectPathPoints.GetLength(1)-1);
                    if(gameObjectPathPoints[randomPosX,randomPosY] != null){
                        randomEndpoint = gameObjectPathPoints[randomPosX,randomPosY].transform.position;
                    }
            }
           
        }else{
            anim.SetBool("isRunning", false);
        }
    }

    public void SetPathNodes(GameObject[,] gameObjectPathPoints){
        this.gameObjectPathPoints = gameObjectPathPoints;     
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag == "Enemy"){
        }
        if(other.collider.tag == "ThrowAbleObject"){
            TakeDamage();
        }
    }
   public void TakeDamage(){
        health -= 1;
        if(health == 0){
            // Instantiate(smokeEffect,new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.identity);
            Destroy(gameObject);
        }
   }
//    IEnumerator Stay(){
//     anim.SetBool("isRunning", false);
//     anim.SetBool("isShooting", true);
//     yield return new WaitForSeconds(0.5f); 
//     int randomPosX = new System.Random().Next(0, gameObjectPathPoints.GetLength(0)-1);
//     int randomPosY = new System.Random().Next(0, gameObjectPathPoints.GetLength(1)-1);
//     _currentPosX = randomPosX;
//     _currentPosY = randomPosY;
//     Debug.Log($"{randomPosX} {randomPosY}");
//     randomEndpoint = gameObjectPathPoints[randomPosX,randomPosY].transform.position;
//     anim.SetBool("isShooting", false);
//     anim.SetBool("isRunning", true);
//    }

    private void ThrowWeb(){
        ShootCounter++;
        if(ShootCounter < 1){
            GameObject gm = Instantiate(_web, transform.position, Quaternion.identity);
            int random = new System.Random().Next(0,1);
            if(random == 0){
                if(gameObjectPathPoints[_currentPosX+1, _currentPosY] != null){
                    gm.GetComponent<Rigidbody2D>().MovePosition(gameObjectPathPoints[_currentPosX+1,_currentPosY].transform.position);
                }
            }else{
                if(gameObjectPathPoints[_currentPosX-1, _currentPosY] != null){
                    gm.GetComponent<Rigidbody2D>().MovePosition(gameObjectPathPoints[_currentPosX-1,_currentPosY].transform.position);
                }
            }
        }
    }
}
    

