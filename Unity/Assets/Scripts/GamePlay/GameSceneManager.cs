using Game.GameEvent;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameSceneManager : SingletonWithMono<GameSceneManager>
    {
        private GameObject _diePanel;

        public void Init()
        {
            _diePanel = Resources.Load<GameObject>(GamePath.DiePanelPfb);
            TypeEventSystem.Global.Register<GameOverEvt>(OnGameOverEvt);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(GameDataCache.Instance.CrtSceneIdx);
        }

        public void LoadNextScene()
        {
            SceneManager.LoadScene(GameDataCache.Instance.CrtSceneIdx + 1);
        }

        private void OnGameOverEvt(GameOverEvt evt)
        {
            if (GameDataCache.Instance.IsOver)
                return;

            GameObject.Instantiate(_diePanel, GameDataCache.Instance.Canvas.transform);
        }
    }
}