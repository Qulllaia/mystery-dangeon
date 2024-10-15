using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

public class wallChecker : MonoBehaviour
{
    public int x;
    public int y;
    public bool touched = false; 
    public GameObject enemyTaken;
    void Awake(){
        Collider2D[] collider2D = Physics2D.OverlapCircleAll(transform.position,0f);
        foreach(Collider2D col in collider2D){
            if(col.tag == "Walls"){
                touched = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
            if(GameObject.FindWithTag("Boss") != null)
                enemies.Add(GameObject.FindWithTag("Boss"));
            foreach(GameObject enemy in enemies){
                EnemyAI e = enemy.GetComponent<EnemyAI>();
                if(e != null)
                    e.SetLastTriggeredNode(x,y);   
                ImpBehavour imp = enemy.GetComponent<ImpBehavour>();
                if(imp != null)
                    imp.SetLastTriggeredNode(x,y);
                Ghoul ghoul = enemy.GetComponent<Ghoul>();
                if(ghoul != null)
                    ghoul.SetLastTriggeredNode(x,y);
                Cyclop cyclop = enemy.GetComponent<Cyclop>();
                if(cyclop != null)
                    cyclop.SetLastTriggeredNode(x,y);
            }
        }
        if(other.tag == "Enemy" || other.tag == "Boss"){
            enemyTaken = other.gameObject;
            EnemyAI defaultEnemy = other.gameObject.GetComponent<EnemyAI>();
            ImpBehavour imp = other.gameObject.GetComponent<ImpBehavour>();
            Ghoul ghoul = other.gameObject.GetComponent<Ghoul>();
            Cyclop cyclop = other.gameObject.GetComponent<Cyclop>();

            
            if(defaultEnemy != null){
                defaultEnemy.SetStartNode(x,y);
            }
            if(imp != null)
            {
                imp.SetStartNode(x,y);
            }
            if(ghoul != null)
            {
                ghoul.SetStartNode(x,y);
            }
            if(cyclop != null){
                cyclop.SetStartNode(x,y);
            }
            // Debug.Log("enemyStart");
        }
    }
    void OnTriggerStay2D(Collider2D other){
        if(other.tag == "Enemy"){
            EnemyAI defaultEnemy = other.gameObject.GetComponent<EnemyAI>();
            if(defaultEnemy != null){
                defaultEnemy.SetSavedStartPoint(x,y);
            }
            ImpBehavour imp = other.gameObject.GetComponent<ImpBehavour>();
            if(imp != null){
                imp.SetSavedStartPoint(x,y);
            }
            Ghoul ghoul = other.gameObject.GetComponent<Ghoul>();
            if(ghoul != null){
                ghoul.SetSavedStartPoint(x,y);
            }
            Cyclop cyclop = other.gameObject.GetComponent<Cyclop>();
            if(cyclop != null){
                cyclop.SetSavedStartPoint(x,y);
            }
        }
    }
    public GameObject GetEnemyThatTookNode(){
        return enemyTaken;
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Enemy"){
            enemyTaken = null;
        }
    }
}
