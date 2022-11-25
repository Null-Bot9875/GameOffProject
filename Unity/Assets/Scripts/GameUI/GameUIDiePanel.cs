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
            Time.timeScale = 0;
            GameDataCache.Instance.IsOver = true;
            foreach (var enemy in GameDataCache.Instance.EnemyList)
            {
                enemy.enabled = false;
            }
            _respawnBtn.onClick.AddListener(() =>
            {
                GameObject.Destroy(gameObject);
                GameSceneManager.Instance.ReloadScene();
            });
        }

        private void OnDestroy()
        {
            Time.timeScale = 1;
            GameDataCache.Instance.IsOver = false;
        }
    }
}