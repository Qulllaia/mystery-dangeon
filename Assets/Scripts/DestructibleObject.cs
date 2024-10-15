using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private List<GameObject> collectables;
    private Animator animator;
    void Start(){
        animator = GetComponent<Animator>();
    }
    void Update(){
    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag == "ThrowAbleObject"){
            StartCoroutine(TakeDamage());
            health -= 1;
            Destroy(other.gameObject);
            if(health == 0){
                animator.SetBool("isDestroyed", true);
                if(GetComponent<CapsuleCollider2D>() != null)
                    GetComponent<CapsuleCollider2D>().enabled = false;
                else
                    GetComponent<BoxCollider2D>().enabled = false;
                if(3 == new System.Random().Next(1,4)){
                    StartCoroutine(SpawnCollectable());
                }
            }
        }
    }
    IEnumerator TakeDamage(){
        animator.SetBool("isDamaged", true);
        yield return new WaitForSeconds(0.4f);
        animator.SetBool("isDamaged", false);
    }
    IEnumerator SpawnCollectable(){
        yield return new WaitForSeconds(1.1f);
        Instantiate(collectables[new System.Random().Next(collectables.Count)],transform.position, Quaternion.identity);
    }
}
