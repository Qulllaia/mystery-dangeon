using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwableObjectScript : MonoBehaviour
{
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private GameObject destroyEffect;
    private Rigidbody2D rg;
    public int horizontal;
    public int vertical;

    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rg.MovePosition(transform.position + new Vector3(horizontal,vertical,0)*speed);
        transform.Rotate(0,0,5f);
    }

    public void transitionOfVector(int hor, int vert){
        horizontal = hor;
        vertical = vert;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Walls"){
            Instantiate(destroyEffect, new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity);
            Destroy(gameObject);
        }
        else if(other.collider.tag == "Enemy" || other.collider.tag == "Body" || other.collider.tag == "CollectableObject"){
            Destroy(gameObject);
        }
    }
}
