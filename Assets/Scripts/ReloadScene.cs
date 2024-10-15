using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReloadScene : MonoBehaviour
{

    void Update()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            StartCoroutine(reloadScene());    
    }
    IEnumerator reloadScene(){
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }
}
