using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class MobileAttackController : MonoBehaviour,IPointerClickHandler
{
    public UnityEvent startThrowing;

    public virtual void OnPointerClick(PointerEventData ped){
        startThrowing?.Invoke();
    }


}
