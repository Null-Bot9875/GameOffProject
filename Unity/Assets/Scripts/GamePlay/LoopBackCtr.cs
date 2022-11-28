using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class LoopBackCtr : MonoBehaviour
    {
        [SerializeField] private float _speed = .5f;

        private void Awake()
        {
            var mat = GetComponent<SpriteRenderer>().material;
            mat.DOOffset(new Vector2(10000000, 0), _speed).SetSpeedBased();
        }
    }
}