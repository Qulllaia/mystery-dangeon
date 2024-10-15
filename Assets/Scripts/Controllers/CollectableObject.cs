using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectableObject : MonoBehaviour
{
    // public event Action HealHealthChanged;

    float maxY;
    float minY;
    bool goUp = true;

    void Start(){
        maxY = transform.position.y+0.3f;
        minY = transform.position.y;
    }
    void Update()
    {
        if(maxY > transform.position.y && goUp){
            transform.position = new Vector3(transform.position.x, transform.position.y+0.01f, transform.position.z);
        }
        else{
            goUp = false;
        }
        if(minY < transform.position.y && !goUp){
            transform.position = new Vector3(transform.position.x, transform.position.y-0.01f, transform.position.z);
        }
        else{
            goUp = true;
        }
    }
    public void SetNewMinMax(){
        maxY = transform.position.y-1.5f+0.3f;
        minY = transform.position.y-1.5f;
    }

}
