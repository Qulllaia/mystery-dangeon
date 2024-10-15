using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public void OnClick(string sceneName){
        Debug.Log("Clicked");
        SceneManager.LoadScene(sceneName);
    }
    public void ExitClick(){
        Application.Quit();
    }
}
