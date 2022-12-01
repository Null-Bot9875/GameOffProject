using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [DefaultExecutionOrder(-100)]
    public class Init : MonoBehaviour
    {
        [SerializeField] private Texture2D _cursor;

        private GameObject _fadePanel;
        private void Awake()
        {
            Application.targetFrameRate = 60;
            AudioManager.Instance.Init();
            GameSceneManager.Instance.Init();

            SceneManager.LoadScene("Main");
            Cursor.SetCursor(_cursor, new Vector2(_cursor.width / 2f, _cursor.height / 2f), CursorMode.Auto);
        }
    }
}