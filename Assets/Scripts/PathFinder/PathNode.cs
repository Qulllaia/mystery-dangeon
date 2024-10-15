using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathNode{
    public bool isUsedForWay;
    public int fCost;
    public int gCost;
    public int hCost;
    public int x;
    public int y;
    public PathNode cameFromNode;
    public PathNode (int x, int y){
        this.x = x;
        this.y = y;
    }
    public void CalculateFCost(){
        fCost = gCost + hCost;
    }
    

}