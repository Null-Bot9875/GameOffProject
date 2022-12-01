using System;
using UnityEngine;

namespace Game
{
    public class ReboundWall : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D col)
        {
            var bullet = col.gameObject.GetComponent<BulletCtr>();
            if (col.gameObject.CompareTag("Bullet") && !bullet.IsGhost)
            {
                AudioManager.Instance.PlayAudioOnce(GamePath.SFXRebound);
            }
        }
    }
}