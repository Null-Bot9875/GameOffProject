using UnityEngine;

namespace Game
{
    public class GameUIFadePanel : MonoBehaviour
    {
        [SerializeField] private AnimationClip _inClip;
        [SerializeField] private AnimationClip _outClip;
        [SerializeField] private Animancer.AnimancerComponent _animancer;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            var state = _animancer.Play(_inClip);
            state.Events.OnEnd = () =>
            {
                var outState = _animancer.Play(_outClip);
                outState.Events.OnEnd = () => GameObject.Destroy(gameObject);
            };
        }
    }
}