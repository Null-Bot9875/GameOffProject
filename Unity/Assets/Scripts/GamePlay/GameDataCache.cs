using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameDataCache : Singleton<GameDataCache>
    {
        public List<EnemyController> EnemyList { get; set; }
        public PlayerController Player { get; set; }
        public int CrtSceneIdx { get; set; }
        public Canvas Canvas { get; set; }
    }
}