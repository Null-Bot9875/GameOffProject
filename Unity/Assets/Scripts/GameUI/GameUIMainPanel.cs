using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class GameUIMainPanel : MonoBehaviour
    {
        [SerializeField] private Button _startBtn;
        [SerializeField] private Button _creditsBtn;
        [SerializeField] private Button _quitBtn;

        private void Awake()
        {
            
            _startBtn.onClick.AddListener(() =>
            {
                StartCoroutine(GetComponent<GameUIMainToNextScene>().GoToNextScene());
            });
            _creditsBtn.onClick.AddListener(() =>
            {
                var pfb = Resources.Load<GameObject>(GamePath.UIPrefabPath + "GameUICreditsPanel");
                GameObject.Instantiate(pfb, transform.parent);
            });
            _quitBtn.onClick.AddListener(Application.Quit);
        }
    }
}