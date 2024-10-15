using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private int health = 3;
    [SerializeField] private GameObject smokeEffect;
    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb2d;
    private Vector3 direction = new Vector2(1,0);

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("isRunning",true);

    }

    void Update()
    {
        rb2d.MovePosition(transform.position + direction*speed);
    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag == "Walls"){

        }
        else if(other.collider.tag == "ThrowAbleObject"){
            anim.SetTrigger("takeDamage");
            health -= 1;
            if(health == 0){
                Instantiate(smokeEffect,new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.identity);
                Destroy(gameObject);
            }
        }
        
    }
    


}
