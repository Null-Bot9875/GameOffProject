using Game.GameEvent;
using UnityEngine;

namespace Game
{
    public class WallCtr : MonoBehaviour, IBulletTrigger
    {
        private GameObject bulletOnWallObj;
        private GameObject instanceOnWallObj;
        private GameObject bulletShootWallObj;
        private bool isInWall;

        private void Start()
        {
            bulletOnWallObj = Resources.Load<GameObject>(GamePath.BulletOnwallPfb);
            bulletShootWallObj = Resources.Load<GameObject>(GamePath.BulletShootPlacePfb);
            TypeEventSystem.Global.Register<GameRecycleBulletRequestEvt>(WallBulletShoot)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        void WallBulletShoot(GameRecycleBulletRequestEvt recycleBulletRequestEvt)
        {
            if (!isInWall)
                return;

            Destroy(instanceOnWallObj);
        }

        public void OnBulletTrigger(BulletCtr ctr)
        {
            if (ctr.IsGhost)
            {
                ctr.DestroyGo();
                return;
            }

            var go = ctr.gameObject;
            TypeEventSystem.Global.Send(new GameBulletShotOnPlaceEvt
            {
                bulletPos = go.transform.position
            });
            isInWall = true;
            Instantiate(bulletShootWallObj, go.transform.position, Quaternion.identity);
            instanceOnWallObj = Instantiate(bulletOnWallObj, go.transform.position, Quaternion.identity);
            ctr.DestroyGo();
            
            AudioManager.Instance.PlayAudioOnce(GamePath.WallSFX);
        }
    }
}