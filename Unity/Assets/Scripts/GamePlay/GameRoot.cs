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
            GameDataCache.Instance.Canvas = GameObject.FindObjectOfType<Canvas>();
        }
    }
}