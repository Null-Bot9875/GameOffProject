using UnityEngine;

namespace Game
{
    public class ReboundWall : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                var bullet = collision.gameObject.GetComponent<BulletCtr>();
                if (!bullet.IsGhost)
                {
                    AudioManager.Instance.PlayAudioOnce(GamePath.ReboundVFX);
                }
            }
        }
    }
}