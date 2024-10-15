using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableChararteristics : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _health;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedAttack;


    void OnCollisionEnter2D(Collision2D other){
        other.collider.gameObject.GetComponent<CharacterMovementScript>().UpdateCharacteristics(_speed,_speedAttack, _maxHealth, _health);
        if(other.collider.tag == "Player"){
            Destroy(gameObject);
        }
    }

}
