using UnityEngine;

namespace Game
{
    public class ExplotionBarrelCtr : MonoBehaviour, IExplosion
    {
        [SerializeField, Header("爆炸特效")] private GameObject explosionClip;
        public bool isInvalided;
        [SerializeField, Header("爆炸半径")] private float explosionRadius;

        private void OnTriggerEnter2D(Collider2D col)
        {
            var go = col.gameObject;
            if (go.CompareTag("Bullet") && !go.GetComponent<BulletCtr>().QueryGhost())
            {
                OnExplosion();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

        public void OnExplosion()
        {
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