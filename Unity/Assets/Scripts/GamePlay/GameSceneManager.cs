using Game.GameEvent;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameSceneManager : SingletonWithMono<GameSceneManager>
    {
        private GameObject _diePanel;
        private GameObject _finishPanel;

        public void Init()
        {
            _diePanel = Resources.Load<GameObject>(GamePath.DiePanelPfb);
            _finishPanel = Resources.Load<GameObject>(GamePath.FinishPanelPfb);
            TypeEventSystem.Global.Register<GameOverEvt>(OnGameOverEvt);
            TypeEventSystem.Global.Register<GameFinishEvt>(OnGameFinishEvt);
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

            _diePanel.GetComponent<GameUIDiePanel>().DieReason = evt.DieReason;
            GameObject.Instantiate(_diePanel, GameDataCache.Instance.Canvas.transform);
        }

        private void OnGameFinishEvt(GameFinishEvt evt)
        {
            GameObject.Instantiate(_finishPanel, GameDataCache.Instance.Canvas.transform);
        }
    }
}