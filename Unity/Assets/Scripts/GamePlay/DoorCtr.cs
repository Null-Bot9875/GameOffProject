using System;
using Animancer;
using Game.GameEvent;
using UnityEngine;

namespace Game
{
    public class DoorCtr : MonoBehaviour
    {
        [SerializeField] private AnimationClip _doorOpen;
        [SerializeField] private AnimancerComponent _animancerComponent;
        private GameObject light;
        private bool isOpen;

        private void Awake()
        {
            TypeEventSystem.Global.Register<GameRecycleBulletTriggerEvt>(OnGameRecycleBulletTriggerEvt);
            light = transform.Find("Light").gameObject;
            light.SetActive(false);
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
                _animancerComponent.Play(_doorOpen);
                light.SetActive(true);
                transform.Find("Light").GetComponent<LightAniCtr>().PlayDoorLight();
                AudioManager.Instance.PlayAudioOnce(GamePath.SFXOpenDoor);
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
            AudioManager.Instance.PlayAudioOnce(GamePath.SFXGamePass);
            TypeEventSystem.Global.Send(new GameFinishEvt());
        }
    }
}