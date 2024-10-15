using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    Animator animator;

    void OnEnable(){
        animator = GetComponent<Animator>();    
        StartCoroutine(portalTimer());
    }

    IEnumerator portalTimer(){
        yield return new WaitForSeconds(0.4f);
        animator.SetBool("isOpen", true);
    }
    IEnumerator teleportTimer(){
        animator.SetBool("isOpen", false);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            if(PlayerPrefs.HasKey("Level")){
                PlayerPrefs.SetInt("Level",2);
            };
            PlayerPrefs.Save();
            Destroy(other.gameObject);
            StartCoroutine(teleportTimer());
        }
    }

}
