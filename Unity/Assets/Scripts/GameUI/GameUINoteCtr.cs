using System.Collections.Generic;
using DG.Tweening;
using Game.GameEvent;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public enum NoteListName
    {
        None,
        Start,
        Shoot,
        Back
    }

    public class GameUINoteCtr : MonoBehaviour
    {
        [SerializeField] private GameObject tip;
        [SerializeField, TextArea] private List<string> textListStart;
        [SerializeField, TextArea] private List<string> textListShoot;
        [SerializeField, TextArea] private List<string> textListBack;
        [SerializeField] private float noteDuartion;
        private int ListIndex = 0;
        private int activeTimes;
        private List<string> nowActiveTempList;
        private NoteListName nowAcitveListEnum = NoteListName.None;

        private Text UIText;
        private GameObject imageGo;
        private bool _canGetPlayerInput;
        private bool _canContinueGame;

        private Tween _doTween;
        

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
            TypeEventSystem.Global.Register<GameBulletShotOnPlaceEvt>(ShowNote)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            TypeEventSystem.Global.Register<GameRecycleBulletTriggerEvt>(ShowNote)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }
        private void ShowNote(GameRecycleBulletTriggerEvt gameRecycleBulletTriggerEvt)
        {
            ShowNote(NoteListName.Back);
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
            GameDataCache.Instance.Player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            switch (name)
            {
                case  NoteListName.Start :
                    nowActiveTempList = textListStart;
                    break;
                case  NoteListName.Back :
                    nowActiveTempList = textListBack;
                    break;
                case  NoteListName.Shoot :
                    nowActiveTempList = textListShoot;
                    break;
                case NoteListName.None : return;
            }

            _doTween = UIText.DOText(nowActiveTempList[ListIndex], noteDuartion).OnComplete(() =>
            {
                nowAcitveListEnum = name;
                _canGetPlayerInput = true;
                if (ListIndex == nowActiveTempList.Count - 1)
                {
                    _canContinueGame = true;
                }
                ListIndex++;
            });
        }

        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.Escape) && nowAcitveListEnum!= NoteListName.None)
            {
                
                AudioManager.Instance.PlayAudioOnce(GamePath.SFXUIClick);
                _canGetPlayerInput = false;
                _doTween.Kill(false);
                ListIndex = 0;
                _canContinueGame = false;
                GameDataCache.Instance.Player.IsMove = true;
                imageGo.SetActive(false);
                activeTimes++;
                if (activeTimes >= 3)
                {
                    Destroy(gameObject);
                }
                nowAcitveListEnum = NoteListName.None;
                return;
            }
            if (!_canGetPlayerInput)
            {
                return;
            }
            if (Input.anyKeyDown)
            {
                AudioManager.Instance.PlayAudioOnce(GamePath.SFXUIClick);
                _canGetPlayerInput = false;
                if (_canContinueGame)
                {
                    ListIndex = 0;
                    _canContinueGame = false;
                    GameDataCache.Instance.Player.IsMove = true;
                    imageGo.SetActive(false);
                    activeTimes++;
                    if (activeTimes == 1)
                    {
                        Instantiate(tip, new Vector2(-3.38f, 2.69f), Quaternion.identity);
                    }
                    if (activeTimes >= 3)
                    {
                        Destroy(gameObject);
                        Instantiate(tip, new Vector2(-3.38f, 2.69f), Quaternion.identity);
                    }
                    nowAcitveListEnum = NoteListName.None;
                    return;
                }
                ShowNote(nowAcitveListEnum);
            }
        }

        
    }
}