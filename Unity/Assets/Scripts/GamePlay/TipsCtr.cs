using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class TipsCtr : MonoBehaviour
    {
        private SpriteRenderer go1;
        private SpriteRenderer go2;
        private void Start()
        {
            go1 = transform.GetChild(0).GetComponent<SpriteRenderer>();
            go2 = transform.GetChild(1).GetComponent<SpriteRenderer>();
            StartCoroutine(TipsShow());
        }

        IEnumerator TipsShow()
        {
            go1.color = Color.clear;
            go2.color = Color.clear;
            yield return new WaitForSeconds(4f);
            go1.DOColor(Color.white, 0.5f);
            go2.DOColor(Color.white, 0.5f);
            transform.DOShakeScale(0.55f, 0.28f, 4, 90f, true).SetLoops(-1);
            yield return new WaitForSeconds(10f);
            go1.DOColor(Color.clear, 1);
            go2.DOColor(Color.clear, 1);
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
    }
}
