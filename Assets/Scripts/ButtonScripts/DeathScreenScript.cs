using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenScript : MonoBehaviour
{
    public void Reload(){
        SceneManager.LoadScene("SampleScene");
    }
    public void MainMenu(){
        SceneManager.LoadScene("Menu");
    }
}
