namespace Game.GameEvent
{
    public class GameShootBulletRequestEvt
    {
        public int Count { get; }

        public GameShootBulletRequestEvt()
        {
            Count = GameDataCache.Instance.ShootCount;
        }
    }
}