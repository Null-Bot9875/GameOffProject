using System.Linq;
using Game.GameEvent;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameRoot : MonoBehaviour
    {
        private void Awake()
        {
            GameDataCache.Instance.EnemyList = GameObject.FindObjectsOfType<EnemyController>().ToList();
            GameDataCache.Instance.Player = GameObject.FindObjectsOfType<PlayerController>().ToList();

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


        private void ReloadScene()
        {
            SceneManager.LoadScene(GameDataCache.Instance.CrtSceneIdx);
        }
    }
}