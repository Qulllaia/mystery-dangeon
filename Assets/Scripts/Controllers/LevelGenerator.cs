using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class LevelGenerator : MonoBehaviour
{
    
    [SerializeField] private List<Room>_room;
    [SerializeField] private Room _StartRoom;
    [SerializeField] private Room _shopRoom;
    [SerializeField] private Room _goldenRoom;



    [SerializeField]  private Room _cyclopBossRoom;
    [SerializeField]  private Room _minotaurBossRoom;

    [SerializeField] private List<BigRoom> _bigRoom;
    [SerializeField] private TextMeshProUGUI textFPS;

    private float _timer = 0f;
    private int _frequency = 1;
    private int _level;


    private Room[,] spawnedRooms;
    // IEnumerator Start()
    // {
    //     spawnedRooms = new Room[11,11];
    //     spawnedRooms[5,5] = _room;
    //     for(int i = 0; i < 12; i++){
    //         if(i % 3 == 1){
    //             PlaceOneBigRoom();
    //         }
    //         else{
    //             PlaceOneRoom();
    //         }
    //         yield return new WaitForSeconds(1f);
    //     }    
    // }

    public Room[,] getMap(){
        return spawnedRooms;
    }
    void Awake()
    {
        spawnedRooms = new Room[11,11];
        spawnedRooms[5,5] = _StartRoom;
        if(!PlayerPrefs.HasKey("Level")){
            PlayerPrefs.SetInt("Level", 1);
        }
        else{
            _level = PlayerPrefs.GetInt("Level");
        }
        PlayerPrefs.Save();

        for(int i = 0; i < 5; i++){
            if(i % 3 == 0){
                PlaceOneBigRoom();
            }
            else{
                PlaceOneRoom();
            }
        }
        PlaceDifferentRoom(_shopRoom);
        PlaceDifferentRoom(_goldenRoom);

        if(_level == 1){
            PlaceBossRoom(_cyclopBossRoom);
        }
        else{
            PlaceBossRoom(_minotaurBossRoom);
        }
        if(spawnedRooms[5,6] != null){
            spawnedRooms[5,5].topDoor.SetActive(false);
        }
        if(spawnedRooms[6,5] != null){
            spawnedRooms[5,5].topRightDoor.SetActive(false);
        }
        if(spawnedRooms[5,4] != null){
            spawnedRooms[5,5].bottomDoor.SetActive(false);
        }
        if(spawnedRooms[4,5] != null){
            spawnedRooms[5,5].topLeftDoor.SetActive(false);
        }
        
    }
     private void PlaceBossRoom(Room _bossRoom)
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for(int x = 0; x < spawnedRooms.GetLength(0); x++){
            for(int y = 0; y < spawnedRooms.GetLength(1); y++){
                if(spawnedRooms[x,y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;
                if(x-1 > 0 && x+1 < maxX && y-1 > 0 && y+1 < maxY){
                    if(spawnedRooms[x-1, y] == null && spawnedRooms[x-1, y-1] == null && spawnedRooms[x-1, y+1] == null && spawnedRooms[x-2, y] == null) vacantPlaces.Add(new Vector2Int(x-1,y));
                    if(spawnedRooms[x, y-1] == null && spawnedRooms[x, y-2] == null && spawnedRooms[x-1, y-1] == null && spawnedRooms[x+1, y-1] == null) vacantPlaces.Add(new Vector2Int(x,y-1));
                    if(spawnedRooms[x+1, y] == null && spawnedRooms[x+2, y] == null && spawnedRooms[x+1, y-1] == null && spawnedRooms[x+1, y+1] == null) vacantPlaces.Add(new Vector2Int(x+1,y));
                    if(spawnedRooms[x, y+1] == null && spawnedRooms[x, y+2] == null && spawnedRooms[x+1, y+1] == null && spawnedRooms[x-1, y+1] == null) vacantPlaces.Add(new Vector2Int(x,y+1));
                }
                

            }
        }
        // Debug.Log(_bossRoom.GetType());
        // Debug.Log(vacantPlaces.Count);
        Room newRoom = Instantiate(_bossRoom);
        newRoom.gameObject.SetActive(false);
        int limit = 500;
        while(limit-- > 0){
            Vector2Int position = vacantPlaces.ElementAt(UnityEngine.Random.Range(0,vacantPlaces.Count));
            if(ConnectRooms(newRoom, position)){
                newRoom.transform.position = new Vector3((position.x - 5)*22, (position.y - 5)*10,0);
                spawnedRooms[position.x, position.y] = newRoom;
                break;
            }
        }
        
    }
    private void PlaceOneBigRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for(int x = 0; x < spawnedRooms.GetLength(0); x++){
            for(int y = 0; y < spawnedRooms.GetLength(1); y++){
                if(spawnedRooms[x,y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if(x > 0 && y > 0 &&
                spawnedRooms[x-1, y] == null && spawnedRooms[x-1, y-1] == null
                ) vacantPlaces.Add(new Vector2Int(x-1,y));
                if(x < maxX && y > 0 &&
                spawnedRooms[x+1, y] == null && spawnedRooms[x+1, y-1] == null
                ) vacantPlaces.Add(new Vector2Int(x+1,y));
                if(y > 1 && y < maxY-2 &&
                    spawnedRooms[x, y+2] == null && spawnedRooms[x, y+1] == null
                ) vacantPlaces.Add(new Vector2Int(x,y+2));
                if(y > 1 && y < maxY-2 &&
                    spawnedRooms[x, y-1] == null && spawnedRooms[x, y-2] == null
                ) vacantPlaces.Add(new Vector2Int(x,y-1));
            }
        }

        
        BigRoom newBigRoom = Instantiate(_bigRoom[new System.Random().Next(0,_bigRoom.Count)]);
        newBigRoom.gameObject.SetActive(false);
        int limit = 500;
        while(limit-- > 0){
            Vector2Int position = vacantPlaces.ElementAt(UnityEngine.Random.Range(0,vacantPlaces.Count));
            if(ConnectBigRooms(newBigRoom, position)){
                newBigRoom.transform.position = new Vector3((position.x - 5)*22, (position.y - 5)*10,0);
                spawnedRooms[position.x, position.y] = newBigRoom;
                spawnedRooms[position.x, position.y-1] = newBigRoom;
                break;
            }
        }
    }

     private void PlaceDifferentRoom(Room roomPrefab)
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for(int x = 0; x < spawnedRooms.GetLength(0); x++){
            for(int y = 0; y < spawnedRooms.GetLength(1); y++){
                if(spawnedRooms[x,y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if(x > 0 && spawnedRooms[x-1, y] == null) vacantPlaces.Add(new Vector2Int(x-1,y));
                if(y > 0 && spawnedRooms[x, y-1] == null) vacantPlaces.Add(new Vector2Int(x,y-1));
                if(x < maxX && spawnedRooms[x+1, y] == null) vacantPlaces.Add(new Vector2Int(x+1,y));
                if(y < maxY && spawnedRooms[x, y+1] == null) vacantPlaces.Add(new Vector2Int(x,y+1));

            }
        }
        Room shop = Instantiate(roomPrefab);
        shop.gameObject.SetActive(false);
        int limit = 500;
        while(limit-- > 0){
            Vector2Int position = vacantPlaces.ElementAt(UnityEngine.Random.Range(0,vacantPlaces.Count));
            if(ConnectRooms(shop, position)){
                shop.transform.position = new Vector3((position.x - 5)*22, (position.y - 5)*10,0);
                spawnedRooms[position.x, position.y] = shop;
                break;
            }
        }
        
    }

    private void PlaceOneRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for(int x = 0; x < spawnedRooms.GetLength(0); x++){
            for(int y = 0; y < spawnedRooms.GetLength(1); y++){
                if(spawnedRooms[x,y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if(x > 0 && spawnedRooms[x-1, y] == null) vacantPlaces.Add(new Vector2Int(x-1,y));
                if(y > 0 && spawnedRooms[x, y-1] == null) vacantPlaces.Add(new Vector2Int(x,y-1));
                if(x < maxX && spawnedRooms[x+1, y] == null) vacantPlaces.Add(new Vector2Int(x+1,y));
                if(y < maxY && spawnedRooms[x, y+1] == null) vacantPlaces.Add(new Vector2Int(x,y+1));

            }
        }
        Room newRoom = Instantiate(_room[new System.Random().Next(0,_room.Count)]);
        newRoom.gameObject.SetActive(false);
        int limit = 500;
        while(limit-- > 0){
            Vector2Int position = vacantPlaces.ElementAt(UnityEngine.Random.Range(0,vacantPlaces.Count));
            if(ConnectRooms(newRoom, position)){
                newRoom.transform.position = new Vector3((position.x - 5)*22, (position.y - 5)*10,0);
                spawnedRooms[position.x, position.y] = newRoom;
                break;
            }
        }
        
    }
    bool ConnectBigRooms(BigRoom thatRoom, Vector2Int p){
        thatRoom.topDoor.SetActive(true);
        thatRoom.bottomDoor.SetActive(true);
        thatRoom.bottomLeftDoor.SetActive(true);
        thatRoom.bottomRightDoor.SetActive(true);
        thatRoom.bottomLeftDoor.SetActive(true);
        thatRoom.bottomRightDoor.SetActive(true);

        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 2;

        List<Vector2Int> neighbours = new List<Vector2Int>();
        if(thatRoom.topDoor != null && p.y < maxY && spawnedRooms[p.x,p.y+1]?.bottomDoor != null) neighbours.Add(Vector2Int.up);
        if(thatRoom.bottomDoor != null && p.y-2 > 0 && spawnedRooms[p.x,p.y-2]?.topDoor != null) neighbours.Add(new Vector2Int(0,-2));

        if(thatRoom.topLeftDoor != null && p.x > 0 && spawnedRooms[p.x-1,p.y]?.topRightDoor != null) neighbours.Add(Vector2Int.left);
        if(thatRoom.bottomLeftDoor != null && p.x > 0 && spawnedRooms[p.x-1,p.y-1]?.topLeftDoor != null) neighbours.Add(new Vector2Int(-1,-1));
        if(thatRoom.bottomLeftDoor != null && p.x > 0 && spawnedRooms[p.x-1,p.y-1]?.bottomLeftDoor != null) neighbours.Add(new Vector2Int(-1,-1));

        if(thatRoom.topRightDoor != null && p.x < maxX && spawnedRooms[p.x+1,p.y]?.topLeftDoor != null) neighbours.Add(Vector2Int.right);
        if(thatRoom.bottomRightDoor != null && p.x > 0 && spawnedRooms[p.x+1,p.y-1]?.topRightDoor != null) neighbours.Add(new Vector2Int(1,-1));
        if(thatRoom.bottomRightDoor != null && p.x > 0 && spawnedRooms[p.x+1,p.y-1]?.bottomRightDoor != null) neighbours.Add(new Vector2Int(1,-1));
       
        if(neighbours.Count == 0) return false;
        foreach(var selectedDirection in neighbours){
            if (selectedDirection == Vector2Int.up){
                thatRoom.topDoor.SetActive(false);
                Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                selectedRoom.bottomDoor.SetActive(false);
            }
            else if (selectedDirection == new Vector2Int(0,-2)){
                thatRoom.bottomDoor.SetActive(false);
                Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                selectedRoom.topDoor.SetActive(false);
            }
            else if (selectedDirection == Vector2Int.left){
                thatRoom.topLeftDoor.SetActive(false);
                 if(spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y]?.GetType() == typeof(BigRoom) &&
                spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y+1]?.GetType() == typeof(BigRoom)
                ){
                    Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                    selectedRoom.bottomRightDoor.SetActive(false);
                }
                else{
                    Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                    selectedRoom.topRightDoor.SetActive(false);
                }
            }
            else if (selectedDirection == new Vector2Int(-1,-1)){
                thatRoom.bottomLeftDoor.SetActive(false);
                if(spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y]?.GetType() == typeof(BigRoom) &&
                spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y+1]?.GetType() == typeof(BigRoom)
                ){
                    Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                    selectedRoom.bottomRightDoor.SetActive(false);
                }
                else{
                    Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                    selectedRoom.topRightDoor.SetActive(false);
                }
            }
            else if (selectedDirection == new Vector2Int(1,-1)){
                thatRoom.bottomRightDoor.SetActive(false);
                // Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                // selectedRoom.topLeftDoor.SetActive(false);
                if(spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y]?.GetType() == typeof(BigRoom) &&
                spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y + 1]?.GetType() == typeof(BigRoom)
                ){
                    Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                    selectedRoom.bottomLeftDoor.SetActive(false);
                }
                else{
                    Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                    selectedRoom.topLeftDoor.SetActive(false);
                }
            }
            else if (selectedDirection == Vector2Int.right){
                thatRoom.topRightDoor.SetActive(false);
                if(spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y]?.GetType() == typeof(BigRoom) &&
                spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y + 1]?.GetType() == typeof(BigRoom)
                ){

                    Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                    selectedRoom.bottomLeftDoor.SetActive(false);
                }
                else{
                    Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                    selectedRoom.topLeftDoor.SetActive(false);
                }
            }
        }
        return true;
    }
    bool ConnectRooms(Room thatRoom, Vector2Int p){
        thatRoom.topDoor.SetActive(true);
        thatRoom.bottomDoor.SetActive(true);
        thatRoom.topLeftDoor.SetActive(true);
        thatRoom.topRightDoor.SetActive(true);
        // thatRoom.topDoorAnimation.SetActive(false); 
        // thatRoom.bottomDoorAnimation.SetActive(false); 
        // thatRoom.leftDoorAnimation.SetActive(false); 
        // thatRoom.rightDoorAnimation.SetActive(false); 

        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        List<Vector2Int> neighbours = new List<Vector2Int>();
        if(thatRoom.topDoor != null && p.y < maxY && spawnedRooms[p.x,p.y+1]?.bottomDoor != null) neighbours.Add(Vector2Int.up);
        if(thatRoom.bottomDoor != null && p.y > 0 && spawnedRooms[p.x,p.y-1]?.topDoor != null) neighbours.Add(Vector2Int.down);

        if(thatRoom.topLeftDoor != null && p.x > 0 && spawnedRooms[p.x-1,p.y]?.topRightDoor != null && spawnedRooms[p.x-1,p.y+1]?.GetType() != typeof(BigRoom)
        ) neighbours.Add(Vector2Int.left);
        else if(thatRoom.topLeftDoor != null && p.x > 0 && spawnedRooms[p.x-1,p.y]?.bottomRightDoor != null
        ) neighbours.Add(new Vector2Int(-1,-1));

        if(thatRoom.topRightDoor != null && p.x < maxX && spawnedRooms[p.x+1,p.y]?.topLeftDoor != null && spawnedRooms[p.x+1,p.y+1]?.GetType() != typeof(BigRoom)
        ) neighbours.Add(Vector2Int.right);
        else if(thatRoom.topRightDoor != null && p.x < maxX && spawnedRooms[p.x+1,p.y]?.bottomLeftDoor != null
        ) neighbours.Add(new Vector2Int(1,-1));
       
        if(neighbours.Count == 0) return false;
        
        // Vector2Int selectedDirection = neighbours[UnityEngine.Random.Range(0,neighbours.Count)];
        foreach(var selectedDirection in neighbours){

            if (selectedDirection == Vector2Int.up){
                thatRoom.topDoor.SetActive(false);
                // thatRoom.topDoorAnimation.SetActive(true); 
                Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                selectedRoom.bottomDoor.SetActive(false);
            }
            else if (selectedDirection == Vector2Int.down){
                thatRoom.bottomDoor.SetActive(false);
                // thatRoom.bottomDoorAnimation.SetActive(true); 
                Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                selectedRoom.topDoor.SetActive(false);
            }
            else if(selectedDirection == new Vector2Int(-1,-1)){
                thatRoom.topLeftDoor.SetActive(false);
                //нет веркотора по высоте, поскольку дверь находится снизу, а для состыковочного на одном уровне
                Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y];
                selectedRoom?.bottomRightDoor.SetActive(false);

            }
            else if(selectedDirection == new Vector2Int(1,-1)){
                thatRoom.topRightDoor.SetActive(false);
                //нет веркотора по высоте, поскольку дверь находится снизу, а для состыковочного на одном уровне
                Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y];
                selectedRoom?.bottomLeftDoor.SetActive(false);

            }
            else if (selectedDirection == Vector2Int.left){
                thatRoom.topLeftDoor.SetActive(false);
                // thatRoom.leftDoorAnimation.SetActive(true); 
                Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                selectedRoom.topRightDoor.SetActive(false);
            }
            else if (selectedDirection == Vector2Int.right){
                thatRoom.topRightDoor.SetActive(false);
                // thatRoom.rightDoorAnimation.SetActive(true); 
                Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];
                selectedRoom.topLeftDoor.SetActive(false);
            }
        }
        
        return true;
    }
   
}

