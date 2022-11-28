using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class GameUIShowPointer : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        private GameObject pointerGo;

        private void Start()
        {
            pointerGo = transform.GetChild(0).gameObject;
            pointerGo.SetActive(false);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            pointerGo.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            pointerGo.SetActive(false);
        }
    }
}
