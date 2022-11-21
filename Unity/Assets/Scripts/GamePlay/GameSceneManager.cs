using Game.GameEvent;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameSceneManager : SingletonWithMono<GameSceneManager>
    {
        public void Init()
        {
            Application.targetFrameRate = 60;
            TypeEventSystem.Global.Register<GameOverEvt>(OnGameOverEvt);
        }

        private void OnGameOverEvt(GameOverEvt obj)
        {
            ReloadScene();
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(GameDataCache.Instance.CrtSceneIdx);
        }

        public void LoadNextScene()
        {
            SceneManager.LoadScene(GameDataCache.Instance.CrtSceneIdx + 1);
        }
    }
}