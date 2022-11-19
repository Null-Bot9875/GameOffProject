using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Game
{
    public class CandleCrl : MonoBehaviour, IBulletTrigger, IExplosion
    {
        [SerializeField] private Light2D _light2D;

        private float time;
        private float num;

        private bool _isInvalid = true;

        private void Awake()
        {
            _light2D.intensity = 0;
        }

        private void Update()
        {
            if (_isInvalid)
                return;

            if (Time.time > time)
            {
                num = Random.Range(1, 1.2f);
                time += 0.2f;
            }

            _light2D.intensity = Mathf.Lerp(_light2D.intensity, num, 0.1f);
        }

        public void OnBulletTrigger(BulletCtr ctr)
        {
            if (ctr.IsGhost)
                return;

            _isInvalid = false;
        }

        public void OnExplosion()
        {
            _isInvalid = false;
        }
    }
}