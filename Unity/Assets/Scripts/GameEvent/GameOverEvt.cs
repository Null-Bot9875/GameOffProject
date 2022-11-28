namespace Game.GameEvent
{
    public enum DieReason
    {
        Bullet,
        Enemy,
        Explosion,
    }
    public class GameOverEvt
    {
        public GameOverEvt(DieReason dieReason)
        {
            DieReason = dieReason;
        }
        public DieReason DieReason { get; set; }
    }
}