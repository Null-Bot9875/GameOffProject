using UnityEngine;

namespace Game
{
    public class ExplotionBarrelCtr : MonoBehaviour, IExplosion, IBulletTrigger
    {
        [SerializeField, Header("爆炸特效")] private GameObject explosionClip;
        public bool isInvalided;
        [SerializeField, Header("爆炸半径")] private float explosionDistance;
        [SerializeField, Header("射线数")] private int rayCount = 5;

        public void OnBulletTrigger(BulletCtr ctr)
        {
            if (ctr.IsGhost)
                return;
            OnExplosion();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var perAngle = 180 / rayCount;
            for (int i = 0; i <= rayCount; i++)
            {
                var angleLeft = Quaternion.Euler(0, 0, -1 * perAngle * i);
                var angleRight = Quaternion.Euler(0, 0, 1 * perAngle * i);
                Gizmos.DrawRay(transform.position, angleLeft * transform.up.normalized * explosionDistance);
                Gizmos.DrawRay(transform.position, angleRight * transform.up.normalized * explosionDistance);
            }
        }

        public void OnExplosion()
        {
            //防止爆炸递归调用
            if (isInvalided)
                return;
            isInvalided = true;
            var clip = Instantiate(explosionClip, transform.position, transform.rotation);
            clip.GetComponent<Animator>().Play("Explosion3");
            GetRayCast();
        }

        private void GetRayCast()
        {
            var perAngle = 180 / rayCount;
            for (int i = 0; i <= rayCount; i++)
            {
                var angleLeft = Quaternion.Euler(0, 0, -1 * perAngle * i);
                var angleRight = Quaternion.Euler(0, 0, 1 * perAngle * i);
                if (Physics2D.Raycast(transform.position, angleLeft * transform.up.normalized * explosionDistance,explosionDistance)
                    .collider.gameObject.TryGetComponent(out IExplosion explosion))
                {
                    explosion.OnExplosion();
                }

                if (Physics2D.Raycast(transform.position, angleRight * transform.up.normalized * explosionDistance,explosionDistance)
                    .collider.gameObject.TryGetComponent(out IExplosion explosion1))
                {
                    explosion1.OnExplosion();
                }

                Destroy(gameObject);
            }

        }
    }
}
