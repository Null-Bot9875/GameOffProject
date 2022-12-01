using System.Collections.Generic;
using DG.Tweening;
using Game.GameEvent;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameUIEnemyTalk : MonoBehaviour
    {
        [SerializeField] private GameObject go;
        [SerializeField, TextArea] private List<string> textList;
        [SerializeField] private float noteDuartion;
        private int ListIndex = 0;

        private Text UIText;
        private GameObject imageGo;
        private bool _canGetPlayerInput;
        private bool _canContinueGame;

        private Tween _doTween;
        

        void Awake()
        {
            imageGo = transform.Find("Image").gameObject;
            UIText = imageGo.transform.Find("Image").transform.Find("Text").GetComponent<Text>();
            imageGo.SetActive(false);
            GameDataCache.Instance.Player = GameObject.FindObjectOfType<PlayerController>();
            go.SetActive(false);
        }

        private void Start()
        {
            go.GetComponent<TipsCtr>().StopCt();
            ShowNote();
        }

        public void ShowNote()
        {
            UIText.text = "";
            imageGo.SetActive(true);
            GameDataCache.Instance.Player.IsMove = false;
            GameDataCache.Instance.Player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (ListIndex == 2)
            {
                imageGo.GetComponent<RectTransform>().DOShakeAnchorPos(0.5f, 20, 32, 360);
            }
            _doTween = UIText.DOText(textList[ListIndex], noteDuartion).OnComplete(() =>
            {
                _canGetPlayerInput = true;
                if (ListIndex == textList.Count - 1)
                {
                    _canContinueGame = true;
                }
                ListIndex++;
            });
        }

        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                AudioManager.Instance.PlayAudioOnce(GamePath.SFXUIClick);
                _canGetPlayerInput = false;
                _doTween.Kill(false);
                _canContinueGame = false;
                GameDataCache.Instance.Player.IsMove = true;
                imageGo.SetActive(false);
                go.SetActive(true);
                go.GetComponent<TipsCtr>().StartCt();
                Destroy(gameObject);
                
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
                    _canContinueGame = false;
                    GameDataCache.Instance.Player.IsMove = true;
                    imageGo.SetActive(false);
                    go.SetActive(true);
                    go.GetComponent<TipsCtr>().StartCt();
                    Destroy(gameObject);
                    return;
                }
                ShowNote();
            }
        }
    }
}