using UnityEngine;

namespace Game
{
    public class LastDoorCtr : DoorCtr
    {
        protected override void OnTriggerAction()
        {
            AudioManager.Instance.PlayAudioOnce(GamePath.GamePassSFX);
            var pfb = Resources.Load<GameObject>(GamePath.UIPrefabPath + "GameUICreditsPanel");
            var go = GameObject.Instantiate(pfb, GameDataCache.Instance.Canvas.transform);
            go.GetComponent<GameUICreditsPanel>().isLast = true;
        }
    }
}