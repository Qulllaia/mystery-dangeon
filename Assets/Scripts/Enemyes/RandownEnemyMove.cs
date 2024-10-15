using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RandownEnemyMove : MonoBehaviour
{
    [SerializeField] private GameObject _miniSlime;
    int vec1;
    int vec2;
    float time = 5;
    float health = 3;
    Rigidbody2D _rigidbody2D;
    GameObject player;
    void Start(){
        _rigidbody2D = GetComponent<Rigidbody2D>();
        vec1 = new System.Random().Next(-5,5);
        vec2 = new System.Random().Next(-5,5);
        player = GameObject.FindWithTag("Player");
    }
    void FixedUpdate()
    {
        time -= Time.deltaTime;
        _rigidbody2D.AddForce(new Vector3(vec1, vec2,0));
        // Debug.Log(time);
        if(time < 0){
            time = 5;
            if(new System.Random().Next(0,2) == 0){
                vec1 = new System.Random().Next(-5,5);
                vec2 = new System.Random().Next(-5,5);
            }
            else{
                if(player != null){
                    if(player.transform.position.x > transform.position.x){
                    vec1 = new System.Random().Next(1,5);
                    }else{
                    vec1 = new System.Random().Next(-5,0);
                    }
                    if(player.transform.position.y > transform.position.y){
                    vec2 = new System.Random().Next(1,5);
                    }else{
                    vec2 = new System.Random().Next(-5,0);
                    }
                }
                }

        }
    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag == "ThrowAbleObject"){
            health -= 1;
        }
        if(health < 0){
            if(_miniSlime != null){
                GameObject miniSlime1 = Instantiate(_miniSlime, transform.position, Quaternion.identity);
                GameObject miniSlime2 = Instantiate(_miniSlime, transform.position, Quaternion.identity);

                GameObject.FindWithTag("Trigger").GetComponent<TriggerRoomFiled>().AddEnemy(miniSlime1);
                GameObject.FindWithTag("Trigger").GetComponent<TriggerRoomFiled>().AddEnemy(miniSlime2);
            }


            Destroy(gameObject);
        }
    }
}
