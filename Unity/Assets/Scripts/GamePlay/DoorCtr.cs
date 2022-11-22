using UnityEngine;

namespace Game
{
    public class DoorCtr : MonoBehaviour
    {
        [SerializeField] private Sprite _doorOpen;
        [SerializeField] private Sprite _doorClose;
        private bool isOpen;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player") && isOpen)
            {
                GameSceneManager.Instance.LoadNextScene();
            }
        }
    }
}