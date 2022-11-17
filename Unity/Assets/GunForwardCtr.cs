using Animancer;
using UnityEngine;

namespace Game
{
    public class GunForwardCtr : MonoBehaviour
    {
        [SerializeField] private AnimationClip[] _animationClips;
        private AnimancerComponent animancer;
        private DirectionalAnimationSet _set;
        private Vector3 _vector3;

        
        private void FixedUpdate()
        {
            _vector3 = (transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized;
            Debug.Log(_vector3);
        }
    }
}
