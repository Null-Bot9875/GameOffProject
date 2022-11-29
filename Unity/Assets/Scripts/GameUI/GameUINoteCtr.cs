using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
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
        
        

        public void ShowNote(NoteListName name)
        {
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
                    return;
                }
                ShowNote(nowAcitveList);
            }

            if (startListIndex  == textListStart.Count -1 && ShootListIndex == textListShoot.Count-1)
            {
                Destroy(gameObject);
            }
            
        }

        
    }
}