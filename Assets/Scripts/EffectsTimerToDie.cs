using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsTimerToDie : MonoBehaviour
{
    [SerializeField] private float time = 0.5f;
    void Start()
    {
        StartCoroutine(cor());
    }
    IEnumerator cor(){
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
