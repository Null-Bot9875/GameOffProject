using UnityEngine;

namespace Game
{
    public class ReboundWall : MonoBehaviour , IBulletTrigger
    {
        public void OnBulletTrigger(BulletCtr ctr)
        {
            var bullet = ctr.gameObject.GetComponent<BulletCtr>();
            if (!bullet.IsGhost)
            {
                AudioManager.Instance.PlayAudioOnce(GamePath.ReboundVFX);
            }
        }
    }
}