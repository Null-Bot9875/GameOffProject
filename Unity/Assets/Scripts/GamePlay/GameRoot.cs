using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameRoot : MonoBehaviour
    {
        private void Awake()
        {
            GameDataCache.Instance.EnemyList = GameObject.FindObjectsOfType<EnemyController>().ToList();
            GameDataCache.Instance.Player = GameObject.FindObjectOfType<PlayerController>();
            GameDataCache.Instance.CrtSceneIdx = SceneManager.GetActiveScene().buildIndex;
            GameDataCache.Instance.Canvas = GameObject.Find("UI").transform.Find("Canvas").GetComponent<Canvas>();
            GameDataCache.Instance.IsOver = false;
            GameDataCache.Instance.ShootCount = 0;

            Instantiate(Resources.Load<GameObject>(GamePath.FadePanelOutPfb), GameDataCache.Instance.Canvas.transform);
            AudioManager.Instance.PlayAudioLoop(GamePath.SFXTrainLoop);
            switch (GameDataCache.Instance.CrtSceneIdx - 1)
            {
                case 1:
                case 2:
                    AudioManager.Instance.PlayMusicLoop(GamePath.MusicGame1And2Level);
                    break;
                case 3:
                case 4:
                    AudioManager.Instance.PlayMusicLoop(GamePath.MusicGame3And4Level);
                    break;
                case 5:
                case 6:
                    AudioManager.Instance.PlayMusicLoop(GamePath.MusicGame5And6Level);
                    break;
                case 7:
                case 8:
                    AudioManager.Instance.PlayMusicLoop(GamePath.MusicGame7And8Level);
                    break;
                case 9:
                    AudioManager.Instance.PlayMusicLoop(GamePath.MusicGame9Level);
                    break;
                case 10:
                    AudioManager.Instance.PlayMusicLoop(GamePath.MusicGame10Level);
                    break;
            }
        }
    }
}