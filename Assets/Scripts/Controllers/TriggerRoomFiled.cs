using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerRoomFiled : MonoBehaviour
{
    [SerializeField] private GameObject movepoint;

    public GameObject[,] gameObjectPathPoints = new GameObject[41,21];
    private PathNode[,] pathNodes = new PathNode[41,21];
    private GameObject _camera;
    [SerializeField] private GameObject _topDoorAnimation;
    [SerializeField] private GameObject _bottomDoorAnimation;
    [SerializeField] private GameObject _reghtDoorAnimation;
    [SerializeField] private GameObject _leftDoorAnimation;

    private LevelGenerator _levelGenerator;
    private MapController _mapController;
    private Room r;
    private Room[,] rooms;
    [SerializeField] private List<GameObject> enemiesInRoom;
    [SerializeField] private EnemyAI _testEnemy;
    private bool _isUserInRoom;
    private bool _isRoomClean;
    Vector2Int position;
    void Start(){
        _camera = GameObject.Find("Main Camera");
        _mapController = _camera.GetComponent<MapController>();
        _levelGenerator = _camera.GetComponent<LevelGenerator>();
        rooms = _levelGenerator.getMap();

    }
    void Update(){
        position = _mapController.transferPositionData();
        r = rooms[position[0],position[1]];
        if(enemiesInRoom.Count != 0){
            foreach(var i in enemiesInRoom.ToList()){
                if(i == null){
                    enemiesInRoom.Remove(i);
                }
            }
        }
        // && rooms[position[0],position[1]].GetType() != typeof(BigRoom)
        if(enemiesInRoom?.Count == 0 ){
            _isRoomClean = true;
        }
        if(_isRoomClean){
            if(r.GetType() != typeof(BigRoom)){
                if(rooms[position[0],position[1]-1] != null)
                    r.bottomDoor.SetActive(false);
                if(rooms[position[0]+1,position[1]] != null)
                    r.topRightDoor.SetActive(false);
                if(rooms[position[0]-1,position[1]] != null)
                    r.topLeftDoor.SetActive(false);
                if(rooms[position[0],position[1]+1] != null)
                    r.topDoor.SetActive(false);
            }else{
                if(rooms[position[0],position[1] +1] == rooms[position[0],position[1]]){
                    if(rooms[position[0],position[1]-1] != null)
                        r.bottomDoor.SetActive(false);
                    if(rooms[position[0]+1,position[1]] != null)
                        r.bottomRightDoor.SetActive(false);
                    if(rooms[position[0]-1,position[1]] != null)
                        r.bottomLeftDoor.SetActive(false);
                    if(rooms[position[0],position[1]+2] != null)
                        r.topDoor.SetActive(false);
                    if(rooms[position[0]+1,position[1]+1] != null)
                        r.topRightDoor.SetActive(false);
                    if(rooms[position[0]-1,position[1]+1] != null)
                        r.topLeftDoor.SetActive(false);
                }
                else if(rooms[position[0],position[1] - 1] == rooms[position[0],position[1]]){
                    if(rooms[position[0],position[1]-2] != null)
                        r.bottomDoor.SetActive(false);
                    if(rooms[position[0]+1,position[1]] != null)
                        r.topRightDoor.SetActive(false);
                    if(rooms[position[0]-1,position[1]] != null)
                        r.topLeftDoor.SetActive(false);
                    if(rooms[position[0],position[1]+1] != null)
                        r.topDoor.SetActive(false);
                    if(rooms[position[0]-1,position[1]-1] != null)
                        r.bottomLeftDoor.SetActive(false);
                    if(rooms[position[0]+1,position[1]-1] != null)
                        r.bottomRightDoor.SetActive(false);
                }
            // Debug.Log("Doors Open");
            }
            r.isCharacterEnteredRoomBefore = true;
        }
    }
    
    void OnTriggerEnter2D(Collider2D info){
        position = _mapController.transferPositionData();
        r = rooms[position[0],position[1]];
        if(info.tag == "Player"){
            pathNodes = transform.Find("PointsCreater").gameObject.GetComponent<PointsCreator>().GetPathNodes();
            gameObjectPathPoints = transform.Find("PointsCreater").gameObject.GetComponent<PointsCreator>().GetGameObjectPathPoints();
            foreach(GameObject enemy in enemiesInRoom){
                if(enemy.GetComponent<EnemyAI>() != null){
                    enemy.GetComponent<EnemyAI>().enabled = true;
                    enemy.GetComponent<EnemyAI>().SetPathNodes(pathNodes, gameObjectPathPoints);
                }
                if(enemy.GetComponent<SpiderBehaviour>() != null){
                    enemy.GetComponent<SpiderBehaviour>().enabled = true;
                    enemy.GetComponent<SpiderBehaviour>().SetPathNodes(gameObjectPathPoints);
                }
                if(enemy.GetComponent<ImpBehavour>() != null){
                    enemy.GetComponent<ImpBehavour>().enabled = true;
                    enemy.GetComponent<ImpBehavour>().SetPathNodes(pathNodes,gameObjectPathPoints);
                }
                if(enemy.GetComponent<Ghoul>() != null){
                    enemy.GetComponent<Ghoul>().enabled = true;
                    enemy.GetComponent<Ghoul>().SetPathNodes(pathNodes,gameObjectPathPoints);
                }
                if(enemy.GetComponent<Cyclop>() != null){
                    enemy.GetComponent<Cyclop>().enabled = true;
                    enemy.GetComponent<Cyclop>().SetPathNodes(pathNodes,gameObjectPathPoints);
                }
            }      
        }


        if(info.tag == "Player"){
            if(!r.isCharacterEnteredRoomBefore){
                r.bottomDoor.SetActive(true);
                r.topRightDoor.SetActive(true);
                r.topLeftDoor.SetActive(true);
                r.topDoor.SetActive(true);
                if(r.GetType() == typeof(BigRoom)){
                    r.bottomLeftDoor?.SetActive(true);
                    r.bottomRightDoor?.SetActive(true);
                }
            }
        }
        
    }
    public void AddEnemy(GameObject newEnemy){
        enemiesInRoom.Add(newEnemy);
    }
    public bool isUserInRoom() {return _isUserInRoom;}
}
