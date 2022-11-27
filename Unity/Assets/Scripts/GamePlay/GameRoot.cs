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

            Instantiate(Resources.Load<GameObject>(GamePath.FadePanelOutPfb),GameDataCache.Instance.Canvas.transform);
            AudioManager.Instance.PlayAudioLoop(GamePath.TrainLoopVFX);
        }

        private void OnDestroy()
        {
            AudioManager.Instance.StopAudioLoop(GamePath.TrainLoopVFX);
        }
    }
}