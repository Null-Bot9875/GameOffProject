using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ItemOrderCtr : MonoBehaviour
    {
        private List<SpriteRenderer> _spriteList;

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
                item.sortingOrder = isFont ? SortingLayer.NameToID("FontGround") : SortingLayer.NameToID("Default");
            }
        }
    }
}