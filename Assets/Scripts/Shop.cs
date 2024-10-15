using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shop : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _price;

    [SerializeField] private List<GameObject> _tierOneListGoods;
    [SerializeField] private List<GameObject> _tierTwoListGoods;

    [SerializeField] private List<Sprite> _tierOnePrices;
    [SerializeField] private List<Sprite> _tierTwoPrices;
    private GameObject _spawnedGM;
    private int _randomCoinPrice;

    void Start()
    {
        int random = new System.Random().Next(0,2);
        // if(random == 0){
            _spawnedGM = Instantiate(_tierOneListGoods[new System.Random().Next(0,_tierOneListGoods.Count)],new Vector3(transform.position.x,transform.position.y +0.5f,transform.position.z), Quaternion.identity);
            _randomCoinPrice = new System.Random().Next(0, _tierOnePrices.Count);
            _price.sprite = _tierOnePrices[_randomCoinPrice];
        // }else{
        //     _spawnedGM = Instantiate(_tierTwoListGoods[new System.Random().Next(0,_tierTwoListGoods.Count)],new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z), Quaternion.identity);
        //     _randomCoinPrice = new System.Random().Next(0, _tierTwoPrices.Count);
        //     _price.sprite = _tierTwoPrices[_randomCoinPrice];
        // }
    }

    void OnCollisionEnter2D(Collision2D other){
        Debug.Log(_randomCoinPrice + 1);
        if(other.gameObject.GetComponent<CollecablesController>().GetCoins() >= (_randomCoinPrice + 1)){
            other.gameObject.GetComponent<CollecablesController>().SetCoins(_randomCoinPrice + 1);
            _spawnedGM.GetComponent<CollectableObject>().SetNewMinMax();   
        }
    }

}
