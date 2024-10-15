using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : MonoBehaviour
{
     [SerializeField] private GameObject _cringeTest;

    private PathNode startNode;
    private PathNode endNode;
    private const int MOVE_DIAGONAL = 14;
    private const int MOVE_STRAIGHT = 10;

    public GameObject[,] gameObjectPathPoints = new GameObject[41,21];
    private PathNode[,] pathNodes = new PathNode[41,21];

    private List<PathNode> openList;
    private List<PathNode> closeList;
    List<PathNode> truePath;
    private Transform[] points;
    public GameObject item;
    public float speed = 0.1f;
    private int step = 0;
    public PathNode lastEndPoint;
    private Animator anim;
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private GameObject _attackDistance;

    [SerializeField] private int health = 3;
    private SpriteRenderer sr;
    private GameObject player;
    public Vector3 point;
    private bool isStopped;
    private float timer;
    private PathNode _savePathNode;


    void OnEnable(){
        item = gameObject;
        anim = GetComponent<Animator>();
        anim.SetBool("isRunning",true);
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
    }

    void Update(){
        if(player != null){
            if(timer > 0.5f){
                if(Vector3.Distance(item.transform.position, player.transform.position) < 2.2f ){
                    StartCoroutine(StartAttack());
                }
                timer = 0f;
            }else{
                timer += Time.deltaTime;
            }
            if(!isStopped){
                if(startNode != null){
                    if(endNode != lastEndPoint){
                        step = 0;
                        endNode = lastEndPoint;
                        truePath = wayFinder();
                        points = new Transform[truePath.Count];         
                        for(int i = 0; i < truePath.Count; i++){
                            points[i] = gameObjectPathPoints[truePath[i].x,truePath[i].y].GetComponent<Transform>();
                            truePath[i].isUsedForWay = true;
                        }
                    }
                }else{
                    startNode = _savePathNode;
                }
                if(truePath != null){
                    if(truePath.Count > 0) {
                        if((step+1) < points.Length){
                            item.transform.position = Vector3.MoveTowards(item.transform.position, points[step].transform.position, Time.deltaTime * speed);
                            if(Vector3.Distance(item.transform.position, points[step].transform.position) < 0.2f) {
                                step++;
                                point = points[step].transform.position;
                            }
                            startNode = truePath[step];
                        }
                        if(item.transform.position.x - points[step].transform.position.x > 0){
                            sr.flipX = true;
                        }else{
                            sr.flipX = false;
                        }
                    }
                }
            
            }
        }else{
            anim.SetBool("isRunning", false);
        }
    }   

    public void SetStartNode(int x, int y){
        startNode = pathNodes[x,y];
    }
    public void SetSavedStartPoint(int x, int y){
        _savePathNode = pathNodes[x,y];
    }

    public void SetLastTriggeredNode(int x, int y)
    {
        lastEndPoint = pathNodes[x,y];
        // Debug.Log("lastTriggeredNode");
    }
    public Transform[] GetLastGridInfo(){
        return points;
    }

    public void SetPathNodes(PathNode[,] pathNodes, GameObject[,] gameObjectPathPoints){
        // Debug.Log($"Set");
        this.pathNodes = pathNodes;
        this.gameObjectPathPoints = gameObjectPathPoints;
        endNode = null;
        lastEndPoint =  pathNodes[0,0];
    }

    private List<PathNode> wayFinder(){
        // Debug.Log($"{startNode}{endNode} {lastEndPoint}");
        RefreshGrid();
        openList = new List<PathNode>{startNode};
        closeList = new List<PathNode>();
        startNode.gCost = 0;

        startNode.hCost = CalculateHCost(startNode, endNode);
        startNode.CalculateFCost();
        while(openList.Count > 0){
            PathNode currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode){
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);
            closeList.Add(currentNode);
            foreach(PathNode neighbourNode in GetNeighbourList(currentNode)){
                if(closeList.Contains(neighbourNode)) continue;

                // GameObject gameObjectNode = null;
                // for(int i = 0; i < pathNodes.GetLength(0); i++){
                //     for(int j = 0; j < pathNodes.GetLength(1); j++){
                //         if(pathNodes[i,j] == neighbourNode){
                //             gameObjectNode = gameObjectPathPoints[i,j];
                //         }
                //     }
                // }
// && gameObjectNode.GetComponent<wallChecker>().GetEnemyThatTookNode().IsUnityNull() && !neighbourNode.isUsedForWay
                if(neighbourNode != null && !neighbourNode.isUsedForWay){
                    int tentativeGCost = currentNode.gCost + CalculateHCost(currentNode, neighbourNode);
                    if(tentativeGCost < neighbourNode.gCost){
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateHCost(neighbourNode,endNode);
                        neighbourNode.CalculateFCost();
                        if(!openList.Contains(neighbourNode)){
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }
        }

        return null;
    }
    private void RefreshGrid(){
        for(int i = 0; i < pathNodes.GetLength(0); i++){
            for(int j = 0; j < pathNodes.GetLength(1); j++){
                if(gameObjectPathPoints[i,j] != null){
                    pathNodes[i,j].gCost = int.MaxValue;
                    pathNodes[i,j].CalculateFCost();
                    pathNodes[i,j].cameFromNode = null;
                    pathNodes[i,j].isUsedForWay = false;
                }
            }
        }
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode){
        List<PathNode> neighbourNode = new List<PathNode>();
        if(currentNode.x - 1 >= 0){
            neighbourNode.Add(GetNode(currentNode.x - 1, currentNode.y));
            if(currentNode.y - 1 >= 0 && GetNode(currentNode.x, currentNode.y -1) != null && GetNode(currentNode.x-1, currentNode.y) != null){
                if(currentNode.y - 1 >= 0) neighbourNode.Add(GetNode(currentNode.x - 1, currentNode.y -1));
            }
            if(currentNode.y - 1 >= 0 && GetNode(currentNode.x, currentNode.y+1) != null && GetNode(currentNode.x-1, currentNode.y) != null){
                if(currentNode.y + 1 < pathNodes.GetLength(1)) neighbourNode.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
            }
        }
        if(currentNode.x + 1 < pathNodes.GetLength(0)){
            neighbourNode.Add(GetNode(currentNode.x + 1, currentNode.y));
            if(currentNode.y - 1 >= 0 && GetNode(currentNode.x, currentNode.y-1) != null && GetNode(currentNode.x+1, currentNode.y) != null){
                if(currentNode.y - 1 >= 0) neighbourNode.Add(GetNode(currentNode.x + 1, currentNode.y -1));
            }
            if(currentNode.y - 1 >= 0 && GetNode(currentNode.x, currentNode.y+1) != null && GetNode(currentNode.x+1, currentNode.y) != null){
               if(currentNode.y + 1 < pathNodes.GetLength(1)) neighbourNode.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
            }

        }
        if(currentNode.y - 1 >= 0) neighbourNode.Add(GetNode(currentNode.x, currentNode.y -1));
        if(currentNode.y + 1 < pathNodes.GetLength(1)) neighbourNode.Add(GetNode(currentNode.x, currentNode.y + 1));
        // if(neighbourNode.Count != 0){
        //     foreach(PathNode node in neighbourNode.ToList()){
        //         if(gameObjectPathPoints[node.x, node.y]?.GetComponent<wallChecker>()?.EnemyTakenGet() != null){
        //             if(gameObjectPathPoints[node.x, node.y].GetComponent<wallChecker>().EnemyTakenGet() != gameObject){
        //                 Debug.Log("Removed");
        //                 // Debug.Log(gameObjectPathPoints[node.x, node.y].GetComponent<wallChecker>().EnemyTakenGet());
        //                 // Debug.Log(gameObject);
        //                 neighbourNode.Remove(node);
        //             }
        //         }
        //     }
        // }
        return neighbourNode;
    }

    
    private PathNode GetNode(int x, int y)
    {
        return pathNodes[x,y];
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while(currentNode.cameFromNode != null){
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;   
        }
        path.Reverse();
        return path;
    } 

    private int CalculateHCost(PathNode start, PathNode end){
        // Debug.Log($"{start} {end}");
        int xDistance = Mathf.Abs(start.x - end.x);
        int yDistance = Mathf.Abs(start.y - end.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL*Mathf.Min(xDistance,yDistance) + MOVE_STRAIGHT*remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList){
        PathNode lowestCostNode = pathNodeList[0];
        for(int i = 1; i < pathNodeList.Count; i++){
            if(pathNodeList[i].fCost < lowestCostNode.fCost){
                lowestCostNode = pathNodeList[i];
            }
        }
        return lowestCostNode;
    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag == "Enemy"){
            wayFinder();
        }
        if(other.collider.tag == "ThrowAbleObject"){
            TakeDamage();
        }
    }
    void OnTriggerEnter2D (Collider2D other){
        if(other.tag == "ThrowAbleObject"){
            TakeDamage();
            Destroy(other.gameObject);
        }
    }
    public float GetHealth(){
        return health;
    }
   public void TakeDamage(){
        health -= 1;
        if(health == 0){
            // Instantiate(smokeEffect,new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.identity);
            if(_cringeTest != null){
                _cringeTest.SetActive(true);
            }
            
            Destroy(gameObject);
        }
   }
     IEnumerator StartAttack(){
        isStopped = true;
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", true);
        _attackDistance.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        isStopped = false;
        _attackDistance.SetActive(false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isRunning", true);
    }
}
