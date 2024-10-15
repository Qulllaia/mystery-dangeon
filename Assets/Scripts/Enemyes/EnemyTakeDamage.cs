using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyTakeDamage : MonoBehaviour
{
    public event Action bossDamaged;
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "ThrowAbleObject"){
            gameObject.GetComponentInParent<EnemyAI>().TakeDamage();
            Destroy(other.gameObject);
        }
    }
}
