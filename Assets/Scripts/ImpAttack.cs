using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpAttack : MonoBehaviour
{
    [SerializeField] private GameObject _collisionEffect;
    private GameObject _player;
    private int _throwVector;
    private float _speed = 0.35f;
    private Rigidbody2D rigidbody2D;
    private SpriteRenderer _sprite;
    private int _effectVector;
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();  
        _player = GameObject.FindWithTag("Player");
        if(transform.position.x > _player.transform.position.x){
            _throwVector = -1;
            _sprite.flipX = true;
        }
        else{
            _throwVector = 1;
        }
        
    }
    void Update(){
        rigidbody2D.MovePosition(transform.position + new Vector3(_throwVector,0,0)*_speed);
    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag != "Enemy"){
            if(transform.position.x > other.collider.transform.position.x)
            {
                _effectVector = -1;
            }
            else{
                _effectVector = 1;
            }
            Destroy(gameObject);
        }
    }
    void OnDestroy(){
        if(_effectVector == -1){
            Instantiate(_collisionEffect, transform.position, new Quaternion(0,0,0,0));
        }else{
            Instantiate(_collisionEffect, transform.position, new Quaternion(0,0,180,0));
        }
    }
}
