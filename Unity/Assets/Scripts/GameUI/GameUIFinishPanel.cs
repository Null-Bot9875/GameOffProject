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

        private void Awake()
        {
            GameDataCache.Instance.Player.IsMove = false;
            _loadBtn.onClick.AddListener(() => GameSceneManager.Instance.LoadNextScene());

            for (int i = 0; i < 4 - GameDataCache.Instance.ShootCount; i++)
            {
                var go = GameObject.Instantiate(_item, _group);
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