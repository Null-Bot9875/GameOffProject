using System;
using Game.GameEvent;
using UnityEngine;

namespace Game
{
    public class DoorCtr : MonoBehaviour
    {
        [SerializeField] private GameObject _doorOpen;
        [SerializeField] private GameObject _doorClose;
        private bool isOpen;

        private void Awake()
        {
            TypeEventSystem.Global.Register<GameRecycleBulletTriggerEvt>(OnGameRecycleBulletTriggerEvt);
        }

        private void OnDestroy()
        {
            TypeEventSystem.Global.UnRegister<GameRecycleBulletTriggerEvt>(OnGameRecycleBulletTriggerEvt);
        }

        private void OnGameRecycleBulletTriggerEvt(GameRecycleBulletTriggerEvt evt)
        {
            if (GameDataCache.Instance.EnemyList.Count == 0)
            {
                isOpen = true;
                _doorClose.SetActive(false);
                _doorOpen.SetActive(true);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player") && isOpen)
            {
                OnTriggerAction();
            }
        }

        protected virtual void OnTriggerAction()
        {
            TypeEventSystem.Global.Send(new GameFinishEvt());
        }
    }
}