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
            _startBtn.onClick.AddListener(() => SceneManager.LoadScene("GameLevel1"));
            _quitBtn.onClick.AddListener(Application.Quit);
        }
    }
}