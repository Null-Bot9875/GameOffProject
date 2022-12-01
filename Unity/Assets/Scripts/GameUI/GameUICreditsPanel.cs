using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class GameUICreditsPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private float _speed = 10;
        [SerializeField] private Button _closeBtn;

        private float _targetY;

        public bool isLast;

        private void Awake()
        {
            var grid = _content.GetComponent<GridLayoutGroup>().cellSize.y;
            _targetY = (_content.childCount + 1) * grid;
            _closeBtn.onClick.AddListener(() =>
            {
                if (!isLast)
                {
                    GameObject.Destroy(gameObject);
                }
                else
                {
                    SceneManager.LoadScene("Main");
                }
            });
            MoveToTop();
            AudioManager.Instance.PlayMusicLoop(GamePath.MusicGameMain);
        }

        void MoveToTop()
        {
            _content.DOAnchorPosY(_targetY, _speed).SetSpeedBased().OnComplete(MoveEnd);
        }

        void MoveEnd()
        {
            _content.anchoredPosition = Vector2.zero;
            //MoveToTop();
        }
    }
}