// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

using System;
using UnityEngine;

namespace Animancer
{
    /// <summary>[Pro-Only] A <see cref="ControllerState"/> which manages two float parameters.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/animancer/docs/manual/animator-controllers">Animator Controllers</see>
    /// </remarks>
    /// <seealso cref="Float1ControllerState"/>
    /// <seealso cref="Float3ControllerState"/>
    /// https://kybernetik.com.au/animancer/api/Animancer/Float2ControllerState
    /// 
    public sealed class Float2ControllerState : ControllerState
    {
        /************************************************************************************************************************/

        /// <summary>An <see cref="ITransition{TState}"/> that creates a <see cref="Float2ControllerState"/>.</summary>
        public new interface ITransition : ITransition<Float2ControllerState> { }

        /************************************************************************************************************************/

        private ParameterID _ParameterXID;

        /// <summary>The identifier of the parameter which <see cref="ParameterX"/> will get and set.</summary>
        public ParameterID ParameterXID
        {
            get => _ParameterXID;
            set
            {
                _ParameterXID = value;
                _ParameterXID.ValidateHasParameter(Controller, AnimatorControllerParameterType.Float);
            }
        }

        /// <summary>
        /// Gets and sets a float parameter in the <see cref="ControllerState.Controller"/> using the
        /// <see cref="ParameterXID"/>.
        /// </summary>
        public float ParameterX
        {
            get => Playable.GetFloat(_ParameterXID.Hash);
            set => Playable.SetFloat(_ParameterXID.Hash, value);
        }

        /************************************************************************************************************************/

        private ParameterID _ParameterYID;

        /// <summary>The identifier of the parameter which <see cref="ParameterY"/> will get and set.</summary>
        public ParameterID ParameterYID
        {
            get => _ParameterYID;
            set
            {
                _ParameterYID = value;
                _ParameterYID.ValidateHasParameter(Controller, AnimatorControllerParameterType.Float);
            }
        }

        /// <summary>
        /// Gets and sets a float parameter in the <see cref="ControllerState.Controller"/> using the
        /// <see cref="ParameterYID"/>.
        /// </summary>
        public float ParameterY
        {
            get => Playable.GetFloat(_ParameterYID.Hash);
            set => Playable.SetFloat(_ParameterYID.Hash, value);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Gets and sets <see cref="ParameterX"/> and <see cref="ParameterY"/>.
        /// </summary>
        public Vector2 Parameter
        {
            get => new Vector2(ParameterX, ParameterY);
            set
            {
                ParameterX = value.x;
                ParameterY = value.y;
            }
        }

        /************************************************************************************************************************/

        /// <summary>Creates a new <see cref="Float2ControllerState"/> to play the `controller`.</summary>
        public Float2ControllerState(RuntimeAnimatorController controller,
            ParameterID parameterX, ParameterID parameterY, bool keepStateOnStop = false)
            : base(controller, keepStateOnStop)
        {
            _ParameterXID = parameterX;
            _ParameterXID.ValidateHasParameter(Controller, AnimatorControllerParameterType.Float);

            _ParameterYID = parameterY;
            _ParameterYID.ValidateHasParameter(Controller, AnimatorControllerParameterType.Float);
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override int ParameterCount => 2;

        /// <inheritdoc/>
        public override int GetParameterHash(int index)
        {
            switch (index)
            {
                case 0: return _ParameterXID;
                case 1: return _ParameterYID;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            };
        }

        /************************************************************************************************************************/
    }
}

