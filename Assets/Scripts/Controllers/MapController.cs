using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    private Image[,] miniMapMatrix;
    [SerializeField] private Image _miniMap;
    [SerializeField] private Image _miniMapRoom;

    [SerializeField] private Image _miniMapBigRoom;
    [SerializeField] private Image _miniMapBossRoom;

    [SerializeField] private Image _knightMinimapHead;
    [SerializeField] private GameObject _camera;
    private Room[,] spawnedRooms;
    private int _preiousPosX = 5;
    private int _preiousPosY = 5;
    private int _currentPosX = 5;
    private int _currentPosY = 5;

    void Start(){
        //Получение данных о расставленных комнатах.
        spawnedRooms = _camera.GetComponent<LevelGenerator>().getMap();
        MapLoader();
    }
    //Начало загрузки мини-карты.
     void MapLoader(){
        _miniMap.rectTransform.localPosition = new Vector2(-213,-113);
        miniMapMatrix = new Image[11,11];
        List<Room> occupiedPlaces = new List<Room>();
        //Начало вложенного цикла, расставляющего объекты на мини-карта в соответствии с расставленными комнатами в игре.
        for(int i = 0; i < 11; i++){
            for(int j = 0; j < 11; j++){
                //Размещение комнаты босса.
                if(spawnedRooms[i,j] != null && spawnedRooms[i,j]?.GetType() == typeof(BossRoom)){
                    Image miniBossRoom = Instantiate(_miniMapBossRoom, new Vector3(i,j),Quaternion.identity,_miniMap.rectTransform);
                    miniBossRoom.rectTransform.localPosition = new Vector3(i*60-87,j*40-87);
                    miniBossRoom.rectTransform.localScale = new Vector3(1,1,1);
                    miniMapMatrix[i,j] = miniBossRoom;
                }
                //Размещение маленькой комнаты.
                if(spawnedRooms[i,j] != null && spawnedRooms[i,j]?.GetType() == typeof(Room)){
                    Image miniRoom = Instantiate(_miniMapRoom, new Vector3(i,j),Quaternion.identity,_miniMap.rectTransform);
                    miniRoom.rectTransform.localPosition = new Vector3(i*60-87,j*40-87);
                    miniRoom.rectTransform.localScale = new Vector3(1,1,1);
                    miniMapMatrix[i,j] = miniRoom;
                }
                //Размещение большой комнаты.
                else if(spawnedRooms[i,j] != null && spawnedRooms[i,j]?.GetType() == typeof(BigRoom) && !occupiedPlaces.Contains(spawnedRooms[i,j])){
                    Image miniBigRoom = Instantiate(_miniMapBigRoom, new Vector3(i,j),Quaternion.identity,_miniMap.rectTransform);
                    miniBigRoom.rectTransform.localPosition = new Vector3(i*60-87,j*40-87);
                    miniBigRoom.rectTransform.localScale = new Vector3(1,1,1);
                    occupiedPlaces.Add(spawnedRooms[i,j]);
                    miniMapMatrix[i,j] = miniBigRoom;
                    miniMapMatrix[i,j+1] = miniBigRoom;

                }
            }    
        }
        //Размещение иконки игрока в стартовой клетке.
        Image knighthead = Instantiate(_knightMinimapHead, new Vector3(0,0,0),Quaternion.identity,miniMapMatrix[5,5].transform);
        knighthead.rectTransform.localPosition = new Vector3(0,0,0);
        miniMapMatrix[5,5].gameObject.SetActive(true);
        Destroy(miniMapMatrix[5,5].transform.Find("questionMark").gameObject);
        ShowUnknownRooms(5,5,false);
    }
    //Перемещает иконку игрока на мини-карте в зависимости от положения пользователя в игре.
    public void CharacterMapPosition(int x, int y, bool isCameFromBottom,bool isInBigRoom){
        _currentPosX = x;
        _currentPosY = y;
        if(isInBigRoom){
            _miniMap.rectTransform.localPosition = new Vector2(_miniMap.rectTransform.localPosition.x + (_preiousPosX - x)*60,
                                                               _miniMap.rectTransform.localPosition.y + (_preiousPosY - y)*40);
        }
        else{
            _miniMap.rectTransform.localPosition = new Vector2(_miniMap.rectTransform.localPosition.x + (_preiousPosX - x)*60,
                                                               _miniMap.rectTransform.localPosition.y + (_preiousPosY - y)*40);
        }   
        //Удаление обозначения неизвестной комнаты, если игрок в неё заходит.
        GameObject questionMark = miniMapMatrix[x,y].transform.Find("questionMark")?.gameObject;
        if(questionMark != null){
            Destroy(questionMark);
        }
        if(miniMapMatrix[x,y] != null && miniMapMatrix[x,y].GetType() != typeof(BossRoom)){
            Image knighthead = Instantiate(_knightMinimapHead, new Vector3(0,0,0),Quaternion.identity,miniMapMatrix[x,y].transform);
            if(spawnedRooms[x,y].GetType() == typeof(BigRoom)){
                knighthead.rectTransform.localPosition = new Vector3(0,20,0);
            }
            else{
                knighthead.rectTransform.localPosition = new Vector3(0,0,0);
            }
            Destroy(miniMapMatrix[_preiousPosX,_preiousPosY].transform.Find("knightHead(Clone)").gameObject);
            _preiousPosX = x;
            _preiousPosY = y;
            ShowUnknownRooms(x,y, isCameFromBottom);
        }
    }

    public Vector2Int transferPositionData(){
        return new Vector2Int(_currentPosX,_currentPosY);
    }
    //Открывает неизвестные комнаты вокруг игрока.
    void ShowUnknownRooms(int x, int y, bool isCameFromBottom){
        int maxX = miniMapMatrix.GetLength(0);
        int maxY = miniMapMatrix.GetLength(1);
        //При открытии обычной комнаты.
        if((x+1) < maxX){
            if(miniMapMatrix[x+1,y] != null){
                miniMapMatrix[x+1,y].gameObject.SetActive(true);
            }
        }
        if((x-1) > 0){
            if(miniMapMatrix[x-1,y] != null){
                miniMapMatrix[x-1,y].gameObject.SetActive(true);
            }
        }
        if((y+1) < maxY){
            if(miniMapMatrix[x,y+1] != null){
                    miniMapMatrix[x,y+1].gameObject.SetActive(true);
            }
        }
        //При открытии большой комнаты.
        if(spawnedRooms[x,y].GetType() == typeof(BigRoom)){
            if(isCameFromBottom){
                if((y+2) < maxY){
                    if(miniMapMatrix[x,y+2] != null){
                        miniMapMatrix[x,y+2].gameObject.SetActive(true);
                    }
                }
                if((x+1) < maxX && (y+1) < maxY){
                    if(miniMapMatrix[x+1,y+1] != null){
                        miniMapMatrix[x+1,y+1].gameObject.SetActive(true);
                    }
                }
                if((x-1) > 0 && (y+1) < maxY){
                    if(miniMapMatrix[x-1,y+1] != null){
                        miniMapMatrix[x-1,y+1].gameObject.SetActive(true);
                    }
                }
            }else{
                if((y-2) > 0){
                    if(miniMapMatrix[x,y-2] != null){
                        miniMapMatrix[x,y-2].gameObject.SetActive(true);
                    }
                }
                if((x+1) < maxX && (y-1) > 0){
                    if(miniMapMatrix[x+1,y-1] != null){
                        miniMapMatrix[x+1,y-1].gameObject.SetActive(true);
                    }
                }
                if((x-1) > 0 && (y-1) > 0){
                    if(miniMapMatrix[x-1,y-1] != null){
                        miniMapMatrix[x-1,y-1].gameObject.SetActive(true);
                    }
                }
            }
           
        }
        else{
            if((y-1) > 0){
                if(miniMapMatrix[x,y-1] != null){
                    miniMapMatrix[x,y-1].gameObject.SetActive(true);
                }
            }
        }
        
       
    }
}
