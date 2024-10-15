using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthManager : MonoBehaviour
{
    [SerializeField] Image _healthBar;
    private GameObject _boss;
    private float _currentHealth;
    private float _maxHealth;

    void Start(){
        _boss = GameObject.FindWithTag("Boss");
        if(_boss.GetComponent<EnemyAI>() != null)
            _currentHealth = _boss.GetComponent<EnemyAI>().GetHealth();
        else if(_boss.GetComponent<Cyclop>() != null)
            _currentHealth = _boss.GetComponent<Cyclop>().GetHealth();
        _maxHealth = _currentHealth;
    }   

    void Update(){
        
        if(_boss == null){
            _healthBar.fillAmount = 0f;
            StartCoroutine(enumerator());
        }
        else{
            float lastInfo = (_boss.GetComponent<EnemyAI>() != null) ? _boss.GetComponent<EnemyAI>().GetHealth() : _boss.GetComponent<Cyclop>().GetHealth();
            if(_currentHealth != lastInfo){
                _currentHealth = lastInfo;
                _healthBar.fillAmount = _currentHealth / _maxHealth;
        }
        }
       
    }
    IEnumerator enumerator(){
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

}
