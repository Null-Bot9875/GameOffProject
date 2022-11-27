using UnityEngine;

namespace Game
{
    public class CandleShootCtr : CandleCtr, IBulletTrigger, IExplosion
    {
        [SerializeField] private GameObject _candleLight;

        private void OnDestroy()
        {
            AudioManager.Instance.StopAudioLoop(GamePath.CandleLoopVFX);
        }

        public void OnBulletTrigger(BulletCtr ctr)
        {
            if (ctr.IsGhost)
                return;

            _isInvalid = false;
            _candleLight.SetActive(true);
        }

        public void OnExplosion()
        {
            _isInvalid = false;
            _candleLight.SetActive(true);
            AudioManager.Instance.PlayAudioOnce(GamePath.CandleVFX);
            AudioManager.Instance.PlayAudioLoop(GamePath.CandleLoopVFX);
        }
    }
}