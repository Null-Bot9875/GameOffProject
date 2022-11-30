using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ItemOrderCtr : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> _spriteList;

        [SerializeField] private int _fontOrderLayer = -11;
        [SerializeField] private int _backOrderLayer = 11;
        private Vector3 _playerPosCache;

        private void Awake()
        {
            InvokeRepeating(nameof(SetSortingLayer), 0, .1f);
        }

        private void SetSortingLayer()
        {
            //位置改变时刷新
            if (_playerPosCache == GameDataCache.Instance.Player.transform.position)
                return;

            foreach (var item in _spriteList)
            {
                _playerPosCache = GameDataCache.Instance.Player.transform.position;
                var isFont = _playerPosCache.y < transform.position.y;
                item.sortingOrder = isFont ? _fontOrderLayer : _backOrderLayer;
            }
        }
    }
}