using UnityEngine;

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

            Cursor.SetCursor(_cursor, new Vector3(_cursor.width / 2f, _cursor.height / 2f, 0), CursorMode.Auto);
        }
    }
}