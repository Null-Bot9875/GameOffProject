using Unity.VisualScripting;
using UnityEngine;

namespace GameUtil
{
    public static class UIDOTweenExtend
    {
        public static void PlayTween(this SingleTween[] tween, Transform tsf)
        {
            var uidoTween = tsf.GetOrAddComponent<UIDOTween>();
            uidoTween.PlayTween(tween);
        }
    }
}