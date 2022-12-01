using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameUIFinishPanel : MonoBehaviour
    {
        [SerializeField] private Button _loadBtn;
        [SerializeField] private Transform _group;
        [SerializeField] private Transform _item;
        [SerializeField] private Transform _lockItem;

        private void Awake()
        {
            GameDataCache.Instance.Player.IsMove = false;
            _loadBtn.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayAudioOnce(GamePath.SFXUIClick);
                GameSceneManager.Instance.LoadNextScene();
            });

            for (int i = 0; i < 4 - GameDataCache.Instance.ShootCount; i++)
            {
                var go = GameObject.Instantiate(_item, _group);
                go.gameObject.SetActive(true);
            }


            for (int i = 0; i < Mathf.Clamp(GameDataCache.Instance.ShootCount - 1, 0, 3); i++)
            {
                var go = GameObject.Instantiate(_lockItem, _group);
                go.gameObject.SetActive(true);
            }

            Time.timeScale = 0;
        }

        private void OnDestroy()
        {
            Time.timeScale = 1;
        }
    }
}