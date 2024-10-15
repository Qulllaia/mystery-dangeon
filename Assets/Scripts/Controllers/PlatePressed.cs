using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatePressed : MonoBehaviour
{
    private GameObject _camera;
    [SerializeField] private Sprite _pressedPlateSptire;
    private SpriteRenderer _spriteR;
    private Room _roomConsistsPlate;
    private LevelGenerator _levelGenerator;
    private MapController _mapController;
    private Vector2Int _currentPosition;
    [SerializeField] private GameObject _topDoorAnimation;
    [SerializeField] private GameObject _bottomDoorAnimation;
    [SerializeField] private GameObject _reghtDoorAnimation;
    [SerializeField] private GameObject _leftDoorAnimation;
    private Room r;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.Find("Main Camera");
        _spriteR = gameObject.GetComponent<SpriteRenderer>();
        _roomConsistsPlate = gameObject.GetComponentInParent<Room>();
        _levelGenerator = _camera.GetComponent<LevelGenerator>();
        _mapController = _camera.GetComponent<MapController>();
        _currentPosition = _mapController.transferPositionData();
    }

    void OnTriggerEnter2D(Collider2D info){
        Debug.Log("triggered");
        _currentPosition = _mapController.transferPositionData();
        Room[,] rooms = _levelGenerator.getMap();
        r = rooms[_currentPosition[0],_currentPosition[1]];
        if(info.tag == "Player" && !_roomConsistsPlate.isCharacterEnteredRoomBefore){
            _spriteR.sprite = _pressedPlateSptire;
            _roomConsistsPlate.isCharacterEnteredRoomBefore = true;
            if(r.GetType() == typeof(Room)){
                if(rooms[_currentPosition[0],_currentPosition[1]-1] != null)
                    r.bottomDoor.SetActive(false);
                if(rooms[_currentPosition[0] + 1,_currentPosition[1]] != null)
                    r.topRightDoor.SetActive(false);
                if(rooms[_currentPosition[0]-1,_currentPosition[1]] != null)
                    r.topLeftDoor.SetActive(false);
                if(rooms[_currentPosition[0],_currentPosition[1]+1] != null)
                    r.topDoor.SetActive(false);
            }
            else{
                if(rooms[_currentPosition[0],_currentPosition[1]+1] != null){                    
                    if(rooms[_currentPosition[0],_currentPosition[1]+1] == r){
                        if(rooms[_currentPosition[0], _currentPosition[1]+2] != null)
                            r.topDoor.SetActive(false);
                        if(rooms[_currentPosition[0],_currentPosition[1]-1] != null)
                            r.bottomDoor.SetActive(false);
                        if(rooms[_currentPosition[0]+1,_currentPosition[1]+1] != null)
                            r.topRightDoor.SetActive(false);
                        if(rooms[_currentPosition[0]-1,_currentPosition[1]+1] != null)
                            r.topLeftDoor.SetActive(false);
                        if(rooms[_currentPosition[0] + 1,_currentPosition[1]] != null)
                            r.bottomRightDoor.SetActive(false);
                        if(rooms[_currentPosition[0]-1,_currentPosition[1]] != null)
                            r.bottomLeftDoor.SetActive(false);
                    }
                }
                if(rooms[_currentPosition[0],_currentPosition[1]-1] != null){
                    if(rooms[_currentPosition[0],_currentPosition[1]-1] == r){
                        if(rooms[_currentPosition[0], _currentPosition[1]-2] != null)
                            r.bottomDoor.SetActive(false);
                        if(rooms[_currentPosition[0],_currentPosition[1]+1] != null)
                            r.topDoor.SetActive(false);
                        if(rooms[_currentPosition[0]+1,_currentPosition[1]-1] != null)
                            r.bottomRightDoor.SetActive(false);
                        if(rooms[_currentPosition[0]-1,_currentPosition[1]-1] != null)
                            r.bottomLeftDoor.SetActive(false);
                        if(rooms[_currentPosition[0] + 1,_currentPosition[1]] != null)
                            r.topRightDoor.SetActive(false);
                        if(rooms[_currentPosition[0]-1,_currentPosition[1]] != null)
                            r.topLeftDoor.SetActive(false);
                    }

                }
            }
        }
    }
}
