using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Init : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
            AudioManager.Instance.Init();
            GameSceneManager.Instance.Init();
        }
    }
}