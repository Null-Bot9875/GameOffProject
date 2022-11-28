using System;
using Animancer;
using UnityEngine;

namespace Game
{
    public class PlayerAnimationCtr : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent animancer;
        [SerializeField] private DirectionalAnimationSet8 moveSet;
        [SerializeField] private DirectionalAnimationSet8 idleSet;
        [SerializeField] private AnimationClip dieClip;

        private PlayerController _player;

        void Start()
        {
            _player = GetComponent<PlayerController>();
        }

        void Update()
        {
            if (animancer.IsPlayingClip(dieClip))
                return;

            var dir = DirectionalAnimationSet8.SnapVectorToDirection(_player.GetDirection_MouseToPlayer());
            if (!_player.IsMove)
            {
                var idleClip = idleSet.GetClip(dir);
                animancer.Play(idleClip);
                return;
            }

            var clip = GetPlayerMoveInfo(_player).magnitude < 0.2f ? idleSet.GetClip(dir) : moveSet.GetClip(dir);
            animancer.Play(clip);
        }

        private static Vector2 GetPlayerMoveInfo(PlayerController player)
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            return new Vector2(x * player.moveSpeed, y * player.moveSpeed);
        }

        public void Die(Action action)
        {
            transform.Find("Gun").gameObject.SetActive(false);
            if (!animancer.IsPlayingClip(dieClip))
            {
                animancer.Play(dieClip).Events.OnEnd += action;
            }
        }
    }
}