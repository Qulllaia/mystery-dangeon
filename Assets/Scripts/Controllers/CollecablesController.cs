using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CollecablesController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _keysText;
    private int coins;
    private int keys;
    void Start(){
        if(PlayerPrefs.HasKey("keys") && PlayerPrefs.HasKey("coins")){
            coins = PlayerPrefs.GetInt("coins",0);
            keys = PlayerPrefs.GetInt("keys",0);
            _keysText.text= keys.ToString();
            _coinsText.text= coins.ToString();
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Coin"){
            coins++;
            _coinsText.text= coins.ToString();
            Destroy(other.gameObject);
        }
        if(other.tag == "Key"){
            keys++;
            _keysText.text= keys.ToString();
            Destroy(other.gameObject);
        }
        PlayerPrefs.SetInt("keys",keys);
        PlayerPrefs.SetInt("coins",coins);
        PlayerPrefs.Save();

    }
    public int GetKeys(){
        return keys;
    }
    public void SetKeys()
    {
        keys--;
        _keysText.text= keys.ToString();
        PlayerPrefs.SetInt("keys",keys);
        PlayerPrefs.Save();
    }
    public int GetCoins(){
        return coins;
    }
    public void SetCoins(int spentCoins)
    {
        coins -= spentCoins;
        _coinsText.text= coins.ToString();
        PlayerPrefs.SetInt("coins",coins);
        PlayerPrefs.Save();
    }
}
