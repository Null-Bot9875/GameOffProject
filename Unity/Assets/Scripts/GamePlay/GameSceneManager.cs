using UnityEngine.SceneManagement;

namespace Game
{
    public class GameSceneManager : SingletonWithMono<GameSceneManager>
    {
        public void Init()
        {
            
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