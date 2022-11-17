using UnityEngine;

namespace Game
{
    public class ExplotionBarrelCtr : MonoBehaviour, IExplosion, IBulletTrigger
    {
        [SerializeField, Header("爆炸特效")] private GameObject explosionClip;
        public bool isInvalided;
        [SerializeField, Header("爆炸半径")] private float explosionRadius;

        public void OnBulletTrigger(BulletCtr ctr)
        {
            if (ctr.IsGhost)
                return;
            OnExplosion();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

        public void OnExplosion()
        {
            //防止爆炸递归调用
            if (isInvalided)
                return;
            isInvalided = true;
            var go = Instantiate(explosionClip, transform.position, transform.rotation);
            foreach (var item in Physics2D.OverlapCircleAll(transform.position, explosionRadius))
            {
                if (item.gameObject.TryGetComponent(out IExplosion explosion))
                {
                    explosion.OnExplosion();
                }
            }

            Destroy(gameObject);
        }
    }
}