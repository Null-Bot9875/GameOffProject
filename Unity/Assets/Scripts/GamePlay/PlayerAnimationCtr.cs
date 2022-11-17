using Animancer;
using UnityEngine;

namespace Game
{
    public class PlayerAnimationCtr : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent animancer;
        [SerializeField] private DirectionalAnimationSet8 moveSet;
        [SerializeField] private DirectionalAnimationSet8 idleSet;

        private PlayerController _player;

        void Start()
        {
            _player = GetComponent<PlayerController>();
        }

        void Update()
        {
            var dir = DirectionalAnimationSet8.SnapVectorToDirection(_player.GetMouseInfo());
            var clip = _player.GetPlayerMoveInfo().magnitude < 0.2f ? idleSet.GetClip(dir) : moveSet.GetClip(dir);
            animancer.Play(clip);
        }
    }
}