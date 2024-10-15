using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.UI;

public class doMesh : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(enumerator());
        Debug.Log(1);
        var navMeshSurface = gameObject.GetComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
    }

    IEnumerator enumerator(){
        yield return new WaitForSeconds(4);
    }

}
