using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    [SerializeField] private List<GameObject> collectables;
    private Animator animator;
    private bool _isOpen;
    void Start(){
        animator = GetComponent<Animator>();
    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag == "Player" && !_isOpen){
            if(other.gameObject.GetComponent<CollecablesController>().GetKeys() != 0){
                animator.SetBool("isOpened", true);
                other.gameObject.GetComponent<CollecablesController>().SetKeys();
                StartCoroutine(SpawnCollectable());
                _isOpen = true;
            }
        }
        if(other.collider.tag == "ThrowAbleObject"){
            Destroy(other.gameObject);
        }
    }
     IEnumerator SpawnCollectable(){
        yield return new WaitForSeconds(1.1f);
        Instantiate(collectables[new System.Random().Next(collectables.Count)],transform.position, Quaternion.identity);
    }
}
