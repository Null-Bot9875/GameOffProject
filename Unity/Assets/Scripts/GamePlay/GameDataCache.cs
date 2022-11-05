using System.Collections.Generic;

namespace Game
{
    public class GameDataCache : Singleton<GameDataCache>
    {
        public List<EnemyController> EnemyList { get; set; }
        public List<PlayerController> Player { get; set; }
        public int CrtSceneIdx { get; set; }
    }
}