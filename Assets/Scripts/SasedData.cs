using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SasedData : MonoBehaviour
{
    public int difficulty;
    public int frequency;
    public float volume;
    public int health;
    public int keys;
    public int coins;


    void Start()
    {
        if(PlayerPrefs.HasKey("Difficulty")){
            difficulty = PlayerPrefs.GetInt("Difficulty",0);
        }
        if(PlayerPrefs.HasKey("SaveFrequency")){
            frequency = PlayerPrefs.GetInt("SaveFrequency",0);
        }
        if(PlayerPrefs.HasKey("Volume")){
            volume = PlayerPrefs.GetFloat("Volume",0);
        }
    }
    public void ChangeOptionsData(int difficulty, int frequency, float volume){
        this.difficulty = difficulty;
        this.frequency = frequency;
        this.volume = volume;
    }
 

    
}
