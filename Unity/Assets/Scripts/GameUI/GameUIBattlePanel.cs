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
        [SerializeField] private GameObject _settingsPanel;

        private void Awake()
        {
            _settingsBtn.onClick.AddListener(OnClickSettingsBtn);
            _reloadBtn.onClick.AddListener(OnClickReloadBtn);
            TypeEventSystem.Global.Register<GameShootBulletRequestEvt>(OnGameShootBulletRequestEvt);
        }

        private void OnDestroy()
        {
            TypeEventSystem.Global.UnRegister<GameShootBulletRequestEvt>(OnGameShootBulletRequestEvt);
        }

        private void OnGameShootBulletRequestEvt(GameShootBulletRequestEvt evt)
        {
            if (evt.Count > 1)
            {
                var group = transform.Find("StarGroup");
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
        }

        private void OnClickReloadBtn()
        {
            GameSceneManager.Instance.ReloadScene();
        }
    }
}