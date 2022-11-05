using System.Linq;
using Game.GameEvent;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameSceneRoot : MonoBehaviour
    {
        private void Awake()
        {
            GameDataCache.Instance.EnemyList = GameObject.FindObjectsOfType<EnemyController>().ToList();
            GameDataCache.Instance.Player = GameObject.FindObjectOfType<PlayerController>();

            TypeEventSystem.Global.Register<GameOverEvt>(OnGameOverEvt);
        }


        private void OnDestroy()
        {
            TypeEventSystem.Global.UnRegister<GameOverEvt>(OnGameOverEvt);
        }

        private void OnGameOverEvt(GameOverEvt obj)
        {
            ReloadScene();
        }


        public void ReloadScene()
        {
            SceneManager.LoadScene(GameDataCache.Instance.CrtSceneIdx);
        }
    }
}