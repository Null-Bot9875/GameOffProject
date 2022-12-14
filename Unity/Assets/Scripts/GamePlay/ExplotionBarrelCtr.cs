using System;
using UnityEngine;

namespace Game
{
    public class ExplotionBarrelCtr : MonoBehaviour, IExplosion, IBulletTrigger
    {
        [SerializeField, Header("爆炸特效")] private GameObject explosionClip;
        public bool isInvalided;
        [SerializeField, Header("爆炸半径")] private float explosionDistance;

        // [SerializeField, Header("射线数")] private int rayCount = 5;

        private void Start()
        {
            GameDataCache.Instance.Player = GameObject.FindObjectOfType<PlayerController>();
        }

        public void OnBulletTrigger(BulletCtr ctr)
        {
            if (ctr.IsGhost)
                return;
            OnExplosion();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionDistance);
            // for (int i = 0; i <= rayCount; i++)
            // {
            //     var angleLeft = Quaternion.Euler(0, 0, -1 * perAngle * i);
            //     var angleRight = Quaternion.Euler(0, 0, 1 * perAngle * i);
            //     Gizmos.DrawRay(transform.position, angleLeft * transform.up.normalized * explosionDistance);
            //     Gizmos.DrawRay(transform.position, angleRight * transform.up.normalized * explosionDistance);
            // }
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
            AudioManager.Instance.PlayAudioSingle(GamePath.SFXBarrel);
        }

        private void GetRayCast()
        {
            foreach (var item in Physics2D.OverlapCircleAll(transform.position, explosionDistance))
            {
                if (item.gameObject.TryGetComponent(out IExplosion explosion))
                {
                    if (item.gameObject.CompareTag("Player"))
                    {
                        var hit2d = Physics2D.Raycast(transform.position, (transform.position - item.transform.position).normalized, explosionDistance, LayerMask.GetMask("Wall"));
                        if (hit2d.collider == null)
                        {
                            explosion.OnExplosion();
                        }
                        continue;
                    }

                    explosion.OnExplosion();
                }
            }


            // var perAngle = 180 / rayCount;
            // for (int i = 0; i <= rayCount; i++)
            // {
            //     
            //     var angleLeft = Quaternion.Euler(0, 0, -1 * perAngle * i);
            //     var angleRight = Quaternion.Euler(0, 0, 1 * perAngle * i);
            //     var hit2d1 = Physics2D.Raycast(transform.position, angleLeft * transform.up.normalized,
            //         explosionDistance);
            //     var hit2d2 = Physics2D.Raycast(transform.position, angleRight * transform.up.normalized,
            //         explosionDistance);
            //
            //     if (hit2d1.collider.gameObject.TryGetComponent(out IExplosion explosion))
            //     {
            //         explosion.OnExplosion();
            //     }
            //     
            //     if (hit2d2.collider.gameObject.TryGetComponent(out IExplosion explosion1))
            //     {
            //         explosion1.OnExplosion();
            //     }
            //     
            // }
            Destroy(gameObject);
        }
    }
}