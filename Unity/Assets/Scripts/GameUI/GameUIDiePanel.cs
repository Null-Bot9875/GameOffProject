using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameUIDiePanel : MonoBehaviour
    {
        [SerializeField] private Button _respawnBtn;
        [SerializeField] private Button _returnBtn;

        private void Awake()
        {
            GameDataCache.Instance.IsOver = true;
            Time.timeScale = 0;
            _respawnBtn.onClick.AddListener(() =>
            {
                GameObject.Destroy(gameObject);
                GameSceneManager.Instance.ReloadScene();
            });
        }

        private void OnDestroy()
        {
            GameDataCache.Instance.IsOver = false;
            Time.timeScale = 1;
        }
    }
}