using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Reflection;

public class CharacterMovementScript : MonoBehaviour
{
    public event Action DamageHealthChanged;
    public event Action MaxHealthChanged;
    public event Action UpHealthChanged;


    public event Action DamagedByBoss;

    [SerializeField] private GameObject BossBorder;
    [SerializeField] private float _speed = 0.001f; 
    [SerializeField] private GameObject _knife; 
    [SerializeField] private GameObject _camera;
    // [SerializeField] private bool ableToTakeDamage = true;

    private GameObject[] _enemies;

    private Rigidbody2D _rig;
    private Animator _animator;
    private SpriteRenderer sr;
    private MobileController _mobileController;
    private int _attackDirectionX = 0;
    private int _attackDirectionY = 0;
    private int _mapPosX = 5;
    private int _mapPosY = 5;
    private bool isInBigRoom;
    private bool isInBottomOfBigRoom;
    private bool isCameFromBottom;
    private Room[,] _rms;
    private Vector3 _movement;

    void Start()
    {
        Application.targetFrameRate = 60;
        _rig = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        _mobileController = GameObject.FindGameObjectWithTag("Joystick").GetComponent<MobileController>();
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
    

    void Update()
    {
        moveSomeWhere();
        if(_attackDirectionX != 0 || _attackDirectionY != 0 ){
            GameObject throwableObject = Instantiate(_knife,new Vector3(transform.position.x + _attackDirectionX,transform.position.y + _attackDirectionY,transform.position.z), Quaternion.identity);
            throwableObject.GetComponent<throwableObjectScript>().transitionOfVector(_attackDirectionX,_attackDirectionY);
            _attackDirectionX = 0;
            _attackDirectionY = 0;
        }
        if(isInBigRoom){
            if(transform.position.y < _rms[_mapPosX,_mapPosY].transform.position.y &&
            transform.position.y > (_rms[_mapPosX,_mapPosY].transform.position.y-10)
            ){
                _camera.transform.position = new Vector3((_mapPosX-5)*22,transform.position.y,-1);
            }
            if(
                transform.position.y < _rms[_mapPosX,_mapPosY].transform.position.y-5 
                ){
                isInBottomOfBigRoom = true;
            }
            else{
                isInBottomOfBigRoom = false;
            }
        }

    }
    void moveSomeWhere(){
        _movement = new Vector3(_mobileController.Horizontal(), _mobileController.Vertical(), 0);
        if(_movement != new Vector3(0,0,0)){

            if(_movement.x < 0){
                sr.flipX = true;
            }
            else{
                sr.flipX = false;
            }
            if(Mathf.Abs(_movement.x) < 0.25f && Mathf.Abs(_movement.y) < 0.25f){
                _animator.SetBool("isRunning",false);
                _animator.SetBool("isWalking",true);
            }
            else{
                _animator.SetBool("isWalking",false);
                _animator.SetBool("isRunning",true);
            }
            _rig.MovePosition(transform.position + _movement*_speed);        
            // _camera.transform.position = new Vector3(transform.position.x, transform.position.y, _camera.transform.position.z);
        }
        else{
            _animator.SetBool("isRunning",false);
            _animator.SetBool("isWalking",false);
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag == "Enemy" || other.collider.tag == "Body"){
            _animator.SetTrigger("takeDamage");
            NotifyObservers();
            // disableColliders(true);
            // StartCoroutine(enableCollider());
            // Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
            // rigidbody2D.AddForce((transform.position - transform.position)*10000000);
        }
    }
    void NotifyObservers(){
         if(DamageHealthChanged != null){
                DamageHealthChanged?.Invoke();
            }
    }
    void NotifyObserversAboutBossDamage(){
         if(DamagedByBoss != null){
                DamagedByBoss?.Invoke();
            }
    }
    // void disableColliders(bool condition){
    //     if(_enemies.Length != 0)
    //         foreach(var enemy in _enemies){
    //             Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemy.GetComponent<Collider2D>(), condition);
    //         }

    // }
    // IEnumerator enableCollider(){
    //     yield return new WaitForSeconds(2f);
    //     disableColliders(false);
    // }
    public void throwUp(){
        _attackDirectionY = 1;
        _attackDirectionX = 0;
    }
    public void throwDown(){
        _attackDirectionY = -1;
        _attackDirectionX = 0;
    }
    public void throwLeft(){
        _attackDirectionY = 0;
        _attackDirectionX = -1;
    }
    public void throwRight(){
        _attackDirectionY = 0;
        _attackDirectionX = 1;
    }
   
    void OnTriggerEnter2D(Collider2D info){
         if(info.tag == "Axe"){
            _animator.SetTrigger("takeDamage");
            NotifyObservers();
            NotifyObserversAboutBossDamage();
            // disableColliders(true);
            // StartCoroutine(enableCollider());
        }
        if(info.tag == "Enemy"){
            _animator.SetTrigger("takeDamage");
            NotifyObservers();
        }
        if(info.tag == "RightMoveCameraTrigger" 
        || info.tag == "BottomMoveCameraTrigger" 
        || info.tag == "TopMoveCameraTrigger"
        || info.tag == "LeftMoveCameraTrigger"){
            moveMapPos(info);
        }
    }
    void moveMapPos(Collider2D info){


        LevelGenerator lg = _camera.GetComponent<LevelGenerator>();
        MapController mapController = _camera.GetComponent<MapController>();
        Room[,] spawnedRooms = GameObject.Find("Main Camera").GetComponent<LevelGenerator>().getMap();
        spawnedRooms[_mapPosX,_mapPosY].gameObject.SetActive(false);

        _rms = lg.getMap();
        if(isInBigRoom){
            if(isInBottomOfBigRoom && !isCameFromBottom){
                _mapPosY -= 1;
            }
            else if(!isInBottomOfBigRoom && isCameFromBottom){
                _mapPosY += 1;
            }
        }
        
        if(info.tag == "TopMoveCameraTrigger"){
            _mapPosY += 1;
            transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
        }
        if(info.tag == "BottomMoveCameraTrigger"){
            _mapPosY -= 1; 
            transform.position = new Vector3(transform.position.x, transform.position.y - 4, transform.position.z);
        }
        if(info.tag == "LeftMoveCameraTrigger"){
            _mapPosX -= 1;
            transform.position = new Vector3(transform.position.x - 4, transform.position.y, transform.position.z);
        }
        if(info.tag == "RightMoveCameraTrigger"){
            _mapPosX += 1;
            transform.position = new Vector3(transform.position.x + 4, transform.position.y, transform.position.z);
        }
        isInBigRoom = _rms[_mapPosX, _mapPosY].GetType() == typeof(BigRoom);
        if(isInBigRoom){
            if(transform.position.y < _rms[_mapPosX,_mapPosY].transform.position.y-5){
                isCameFromBottom = true;
            }
            else{
                isCameFromBottom = false;
            }
        }
        float cameraPosX = _rms[_mapPosX,_mapPosY].transform.position.x;
        float cameraPosY;
        if(isInBigRoom && isCameFromBottom){
            cameraPosY = _rms[_mapPosX,_mapPosY].transform.position.y-10;
        }
        else{
            cameraPosY = _rms[_mapPosX,_mapPosY].transform.position.y;
        }
        _camera.transform.position = new Vector3(cameraPosX,cameraPosY,-1);
        mapController.CharacterMapPosition(_mapPosX,_mapPosY,isCameFromBottom,isInBigRoom);
        spawnedRooms[_mapPosX,_mapPosY].gameObject.SetActive(true);
       
        if(spawnedRooms[_mapPosX,_mapPosY].GetType() == typeof(BossRoom) && !spawnedRooms[_mapPosX,_mapPosY].isCharacterEnteredRoomBefore){
            BossBorder.SetActive(true);
        }
        else{
            BossBorder.SetActive(false);
        }
    }

    public void UpdateCharacteristics(float newSpeed, float newSpeedAttack, float maxHealth, float health){
        Debug.Log($"{newSpeed}{newSpeedAttack}");
        if(maxHealth != 0){
            MaxHealthChanged?.Invoke();
        }
        if(health != 0){
            UpHealthChanged?.Invoke();
        }
    }
}
