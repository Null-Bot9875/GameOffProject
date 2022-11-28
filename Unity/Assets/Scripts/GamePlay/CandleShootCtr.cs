using UnityEngine;

namespace Game
{
    public class CandleShootCtr : CandleCtr, IBulletTrigger, IExplosion
    {
        [SerializeField] private GameObject _candleLight;

        protected override void OnAwake()
        {
            _isInvalid = true;
            _light2D.intensity = 0;
        }

        private void OnDestroy()
        {
            AudioManager.Instance.StopAudioLoop(GamePath.CandleLoopVFX);
        }

        public void OnBulletTrigger(BulletCtr ctr)
        {
            if (ctr.IsGhost)
                return;

            OnSetLightTrue();
        }

        public void OnExplosion()
        {
            OnSetLightTrue();
        }

        private void OnSetLightTrue()
        {
            _isInvalid = false;
            _candleLight.SetActive(true);
            transform.Find("Fire").gameObject.SetActive(true);
            AudioManager.Instance.PlayAudioOnce(GamePath.CandleVFX);
            AudioManager.Instance.PlayAudioLoop(GamePath.CandleLoopVFX);
        }
    }
}