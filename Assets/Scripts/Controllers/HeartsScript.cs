using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class HeartsScript : MonoBehaviour
{
    [SerializeField] private GameObject _deathScreen;
    public int heartsCount;
    public int MaxHeartsCount;
    private GameObject player;
    private List<GameObject> hearts = new List<GameObject>();
    private List<GameObject> emptyHearts = new List<GameObject>();

    int countOfHearts;
    void Start()
    {
        if(PlayerPrefs.HasKey("health") && PlayerPrefs.HasKey("maxHealth")){
            heartsCount = PlayerPrefs.GetInt("health");
            MaxHeartsCount = PlayerPrefs.GetInt("maxHealth");
        }else{
            heartsCount = 2;
            MaxHeartsCount = 2;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        countOfHearts = transform.childCount;
        for(int i = 0; i < 7; i++){
            GameObject heart = GameObject.Find($"heart {i}");
            GameObject emptyHeart = GameObject.Find($"background {i}");
            hearts.Add(heart);
            emptyHearts.Add(emptyHeart);
            if(i > MaxHeartsCount){
                GameObject.Find($"background {i}").SetActive(false);
            }
            if(i > heartsCount){
                GameObject.Find($"heart {i}").SetActive(false);
            }
        }
        Subscribe();
    }
    public void Subscribe(){
        player.GetComponent<CharacterMovementScript>().DamageHealthChanged += DamageHealthBar;     
        player.GetComponent<CharacterMovementScript>().MaxHealthChanged += MaxHealthBar; 
        player.GetComponent<CharacterMovementScript>().UpHealthChanged += HealHealthBar; 

    }
    
    private void DamageHealthBar()
    {
        heartsCount -= 1;
        for(int i = 0; i < hearts.Count;i++){
            hearts[i].SetActive(i <= heartsCount);
            PlayerPrefs.SetInt("health",heartsCount);
            PlayerPrefs.Save();
        }
        
        if(heartsCount < 0){
            PlayerPrefs.SetInt("Level",1);
            PlayerPrefs.SetInt("keys",0);
            PlayerPrefs.SetInt("coins",0);
            PlayerPrefs.SetInt("maxHealth",2);
            PlayerPrefs.SetInt("health",2);
            PlayerPrefs.Save();
            Destroy(player);
            _deathScreen.SetActive(true);
        }
    }
    private void MaxHealthBar()
    {
        MaxHeartsCount++;
        for(int i = 0; i < emptyHearts.Count;i++){
            emptyHearts[i].SetActive(i <= MaxHeartsCount);
        }
        PlayerPrefs.SetInt("maxHealth",MaxHeartsCount);
        PlayerPrefs.Save();
    }
    private void HealHealthBar()
    {
        if(heartsCount <= MaxHeartsCount){
            heartsCount += 1;
            for(int i = 0; i < hearts.Count;i++){
                hearts[i].SetActive(i <= heartsCount);
            }
        }
        PlayerPrefs.SetInt("health",heartsCount);
        PlayerPrefs.Save();
    }

    // IEnumerator deathScreenTimer(){
    //     float time = 100;

    //     while(time > 0){
    //         time -= Time.deltaTime*1.5f;
    //         float a = curve.Evaluate(time);
    //         _deathScreen.GetComponentInChildren<Image>().color = new Color(0,0,0, a);
    //         yield return 0;
    //     }

    // }
}
