namespace Game.GameEvent
{
    //玩家回收到模拟子弹
    public class GameRecycleBulletGhost
    {
        public GameRecycleBulletGhost(bool isAimSelf)
        {
            IsAimSelf = isAimSelf;
        }
        public bool IsAimSelf { get; set; }
    }
}