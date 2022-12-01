using System;
using Game.GameEvent;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameUIBattlePanel : MonoBehaviour
    {
        [SerializeField] private Button _settingsBtn;
        [SerializeField] private Button _reloadBtn;
        [SerializeField] private GameObject _starGroup;
        [SerializeField] private GameObject _settingsPanel;

        private void Awake()
        {
            _settingsBtn.onClick.AddListener(OnClickSettingsBtn);
            _reloadBtn.onClick.AddListener(OnClickReloadBtn);
            TypeEventSystem.Global.Register<GameShootBulletRequestEvt>(OnGameShootBulletRequestEvt);
            TypeEventSystem.Global.Register<GameFinishEvt>(OnGameFinishEvt);
        }

        private void OnDestroy()
        {
            TypeEventSystem.Global.UnRegister<GameShootBulletRequestEvt>(OnGameShootBulletRequestEvt);
            TypeEventSystem.Global.UnRegister<GameFinishEvt>(OnGameFinishEvt);
        }

        private void OnGameFinishEvt(GameFinishEvt evt)
        {
            _starGroup.SetActive(false);
        }

        private void OnGameShootBulletRequestEvt(GameShootBulletRequestEvt evt)
        {
            if (evt.Count > 1)
            {
                var group = _starGroup.transform;
                for (int i = 0; i < evt.Count - 1; i++)
                {
                    if (i < group.childCount)
                    {
                        group.GetChild(i).transform.Find("StarImg").gameObject.SetActive(false);
                    }
                }
            }
        }

        private void OnClickSettingsBtn()
        {
            GameObject.Instantiate(_settingsPanel, GameDataCache.Instance.Canvas.transform);
            AudioManager.Instance.PlayAudioOnce(GamePath.SFXUIClick);
        }

        private void OnClickReloadBtn()
        {
            GameSceneManager.Instance.ReloadScene();
            AudioManager.Instance.PlayAudioOnce(GamePath.SFXUIClick);
        }
    }
}