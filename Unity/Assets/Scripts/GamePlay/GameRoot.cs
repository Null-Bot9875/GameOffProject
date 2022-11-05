using UnityEngine;

namespace Game
{
    public class GameRoot : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}