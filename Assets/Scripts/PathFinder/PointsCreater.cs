using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PointsCreator : MonoBehaviour
{
    [SerializeField] private bool _isItBigRoom;
    [SerializeField] private GameObject gm;
    public GameObject[,] gameObjectPathPoints = new GameObject[41,21];
    private PathNode[,] pathNodes = new PathNode[41,21];
    void Start()
    {
        //Проверка находится ли человек в большой комнате.
        if(!_isItBigRoom){
            Vector3 roomPos = transform.position;
            //Формирование точек для маленькой комнаты.
            for(int x = 0; x < 20; x++){
                for(int y = 0; y < 8; y++){ 
                    gameObjectPathPoints[x,y] = Instantiate(gm, new Vector3(-9.5f+1f*x,-3.5f+1f*y,0) + roomPos, Quaternion.identity, gameObject.transform);
                    //Если точка коснулась какого-либо препятствия, то она удаляется.
                    if(!gameObjectPathPoints[x,y].GetComponent<wallChecker>().touched){
                        pathNodes[x,y] = new PathNode(x,y);
                        pathNodes[x,y].gCost = int.MaxValue;
                        pathNodes[x,y].CalculateFCost();
                        pathNodes[x,y].cameFromNode = null;
                        gameObjectPathPoints[x,y].GetComponent<wallChecker>().x = x;
                        gameObjectPathPoints[x,y].GetComponent<wallChecker>().y = y;
                    }
                    else{
                        Destroy(gameObjectPathPoints[x,y]);
                        gameObjectPathPoints[x,y] = null;
                        pathNodes[x,y] = null;
                    }
                }       
            }
        }
        else{
            Vector3 roomPos = transform.position;
            //Формирование точек для большой комнаты.
            for(int x = 0; x < 20; x++){
                for(int y = 0; y < 18; y++){ 
                    gameObjectPathPoints[x,y] = Instantiate(gm, new Vector3(-9.5f+1f*x,-13.5f+1f*y,0) + roomPos, Quaternion.identity, gameObject.transform);
                    //Если точка коснулась какого-либо препятствия, то она удаляется.
                    if(!gameObjectPathPoints[x,y].GetComponent<wallChecker>().touched){
                        pathNodes[x,y] = new PathNode(x,y);
                        pathNodes[x,y].gCost = int.MaxValue;
                        pathNodes[x,y].CalculateFCost();
                        pathNodes[x,y].cameFromNode = null;
                        gameObjectPathPoints[x,y].GetComponent<wallChecker>().x = x;
                        gameObjectPathPoints[x,y].GetComponent<wallChecker>().y = y;
                    }
                    else{
                        Destroy(gameObjectPathPoints[x,y]);
                        gameObjectPathPoints[x,y] = null;
                        pathNodes[x,y] = null;
                    }
                }       
            }
        }
       
    }    

    public PathNode[,] GetPathNodes(){
        return pathNodes;
    }
    public GameObject[,] GetGameObjectPathPoints(){
        return gameObjectPathPoints;
    }
}
