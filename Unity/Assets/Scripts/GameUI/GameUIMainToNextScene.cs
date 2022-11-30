using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameUIMainToNextScene: MonoBehaviour
    {
        [SerializeField] private GameObject FadeGo;
        public IEnumerator GoToNextScene()
        {
            var pfb = Instantiate(FadeGo,transform);
            var async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            async.allowSceneActivation = false;
            while (!pfb.GetComponent<GameUIFadePanel>().animcerDone)
            {
                yield return null;
            }
            async.allowSceneActivation = true;
        }
    }
}