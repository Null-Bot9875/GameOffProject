using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Game
{
    public class GameUICreditsPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private float _speed = 10;
        [SerializeField] private Button _closeBtn;

        private float _targetY;

        private void Awake()
        {
            var grid = _content.GetComponent<GridLayoutGroup>().cellSize.y;
            _targetY = (_content.childCount + 1) * grid;
            MoveToTop();
            _closeBtn.onClick.AddListener(() => GameObject.Destroy(gameObject));
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