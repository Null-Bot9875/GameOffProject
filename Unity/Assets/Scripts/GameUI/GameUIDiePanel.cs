using DG.Tweening;
using Game.GameEvent;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameUIDiePanel : MonoBehaviour
    {
        [SerializeField] private Button _respawnBtn;
        [SerializeField] private Button _returnBtn;

        [SerializeField] private Image _cgImg;
        [SerializeField] private CanvasGroup _uiGroup;


        public void Init(DieReason evtDieReason)
        {
            GameDataCache.Instance.IsOver = true;
            foreach (var enemy in GameDataCache.Instance.EnemyList)
            {
                enemy.enabled = false;
            }

            _respawnBtn.onClick.AddListener(() => GameSceneManager.Instance.ReloadScene());
            _cgImg.DOFade(1, 1f).OnComplete(() => _uiGroup.DOFade(1, .2f));
            if (evtDieReason != DieReason.Enemy)
            {
                _cgImg.color = Color.black;
            }
        }

        private void OnDestroy()
        {
            GameDataCache.Instance.IsOver = false;
        }
    }
}