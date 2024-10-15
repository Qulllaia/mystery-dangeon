using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Reflection;
public class BossAttack : MonoBehaviour
{
    private Animator anim;

    private GameObject attackDistance;
    float Delay = 0.8f;
    int counter = 0;
    GameObject player;
    void Start(){
        anim = GetComponentInParent<Animator>();
        anim.SetBool("isRunning",true);
        attackDistance = GameObject.Find("Axe").gameObject;
        player = GameObject.FindWithTag("Player");
        Subscribe();
    }
    public void Subscribe(){
        player.GetComponent<CharacterMovementScript>().DamagedByBoss += Counter;
    }
    void Counter(){
        counter++;
        Debug.Log(counter);
    }
    //  IEnumerator enumerable(){
    //     Delay -= Time.deltaTime;
    //     gameObject.GetComponent<EnemyAI>().enabled = false;
    //     if(Delay <= 0){
    //         anim.SetTrigger("Attack");   
    //         attackDistance.SetActive(true);
    //         Delay = 1.5f;
    //     }
    //     yield return new WaitForSeconds(2f);
    //     gameObject.GetComponent<EnemyAI>().enabled = true;
    //     attackDistance.SetActive(false);
        
    // }
    IEnumerator enableAttackDistance(){
        yield return new WaitForSeconds(0.5f);
        attackDistance.SetActive(true);
    }
    IEnumerator startBossMoving(){
        yield return new WaitForSeconds(0.8f);
        anim.SetBool("isRunning", true);
        gameObject.GetComponentInParent<EnemyAI>().enabled = true;
    }
    void OnTriggerStay2D(Collider2D other){
        if(other.tag == "Player"){
            // StartCoroutine(enumerable());
            Delay -= Time.deltaTime;
            gameObject.GetComponentInParent<EnemyAI>().enabled = false;
            if(Delay <= 0){
                StartCoroutine(enableAttackDistance());
                System.Random random = new System.Random();
                if(random.Next(0,2)%2 == 0){
                    anim.SetTrigger("Attack");   
                }
                else{
                    anim.SetTrigger("Attack2");   
                }
                Delay = 0.8f;
                
            }else{
                anim.SetBool("isRunning", false);
                attackDistance.SetActive(false);
                StartCoroutine(startBossMoving());
            }
        }
    }
}
