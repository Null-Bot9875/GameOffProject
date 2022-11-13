using Animancer;
using UnityEngine;

namespace Game
{
    public class PlayerAnimationCtr : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent animancer;
        [SerializeField] private DirectionalAnimationSet8 moveSet;
        [SerializeField] private DirectionalAnimationSet8 idleSet;

        private PlayerController _playeMove;


        // Start is called before the first frame update
        void Start()
        {
            _playeMove = GetComponent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_playeMove._canMove)
            {
                if (_playeMove.GetPlayerMoveInfo().magnitude < 0.2f)
                {
                    //静止
                    animancer.Play(
                        idleSet.GetClip(
                            DirectionalAnimationSet8.SnapVectorToDirection(
                                _playeMove.GetMouseInfo())));
                }
                else
                {
                    //移动
                    animancer.Play(
                        moveSet.GetClip(
                            DirectionalAnimationSet8.SnapVectorToDirection(
                                _playeMove.GetMouseInfo())));
                }
            }
            else
            {
                animancer.Play(
                    idleSet.GetClip(
                        DirectionalAnimationSet8.SnapVectorToDirection(
                            _playeMove.GetMouseInfo())));
            }
        }
    }
}