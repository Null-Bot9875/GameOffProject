using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.GameEvent;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public enum NoteListName
    {
        Start,
        Shoot
    }

    public class GameUINoteCtr : MonoBehaviour
    {
        [SerializeField, TextArea] private List<string> textListStart;
        [SerializeField, TextArea] private List<string> textListShoot;
        [SerializeField] private float noteDuartion;
        private int startListIndex = 0;
        private int ShootListIndex = 0;
        private int activeTimes;
        private NoteListName nowAcitveList;

        private Text UIText;
        private GameObject imageGo;
        private bool _canGetPlayerInput;
        private bool _canContinueGame;

        void Awake()
        {
            imageGo = transform.Find("Image").gameObject;
            UIText = imageGo.transform.Find("Text").GetComponent<Text>();
            imageGo.SetActive(false);
            GameDataCache.Instance.Player = GameObject.FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            ShowNote(NoteListName.Start);
            TypeEventSystem.Global.Register<GameBulletShotOnPlaceEvt>(ShowNote).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void ShowNote(GameBulletShotOnPlaceEvt gameBulletShotOnPlaceEvt)
        {
            ShowNote(NoteListName.Shoot);
        }

        public void ShowNote(NoteListName name)
        {
            UIText.text = "";
            imageGo.SetActive(true);
            
            GameDataCache.Instance.Player.IsMove = false;
            var list = name == NoteListName.Start ? textListStart : textListShoot;
            var index = name == NoteListName.Start ? startListIndex : ShootListIndex;

            UIText.DOText(list[index], noteDuartion).OnComplete(() =>
            {
                nowAcitveList = name;
                _canGetPlayerInput = true;
                if (index == list.Count - 1)
                {
                    _canContinueGame = true;
                }
                if (name == NoteListName.Start)
                {
                    startListIndex++;
                }
                else
                {
                    ShootListIndex++;
                }
            });
        }

        private void Update()
        {
            if (!_canGetPlayerInput)
            {
                return;
            }
            if (Input.anyKeyDown)
            {
                _canGetPlayerInput = false;
                if (_canContinueGame)
                {
                    _canContinueGame = false;
                    GameDataCache.Instance.Player.IsMove = true;
                    imageGo.SetActive(false);
                    activeTimes++;
                    if (activeTimes >= 2)
                    {
                        Destroy(gameObject);
                    }
                    return;
                }
                ShowNote(nowAcitveList);
            }
        }

        
    }
}