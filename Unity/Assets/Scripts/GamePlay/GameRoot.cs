using System;
using System.Linq;
using Game.GameEvent;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameRoot : MonoBehaviour
    {
        [SerializeField] private GameObject _diePanel;

        private void Awake()
        {
            GameDataCache.Instance.EnemyList = GameObject.FindObjectsOfType<EnemyController>().ToList();
            GameDataCache.Instance.Player = GameObject.FindObjectOfType<PlayerController>();
            GameDataCache.Instance.CrtSceneIdx = SceneManager.GetActiveScene().buildIndex;
            GameDataCache.Instance.Canvas = GameObject.FindObjectOfType<Canvas>();

            TypeEventSystem.Global.Register<GameOverEvt>(OnGameOverEvt);
        }

        private void OnDestroy()
        {
            TypeEventSystem.Global.UnRegister<GameOverEvt>(OnGameOverEvt);
        }

        private void OnGameOverEvt(GameOverEvt evt)
        {
            GameObject.Instantiate(_diePanel, GameDataCache.Instance.Canvas.transform);
        }
    }
}