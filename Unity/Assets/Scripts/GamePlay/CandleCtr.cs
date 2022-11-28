using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Game
{
    public class CandleCtr : MonoBehaviour
    {
        [SerializeField] protected Light2D _light2D;
        private float time;
        private float num;
        protected bool _isInvalid;


        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            transform.Find("Fire").gameObject.SetActive(true);
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
    }
}