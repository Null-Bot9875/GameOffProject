namespace Game.GameEvent
{
    public class GameEnemyAlertEvt
    {
        public int EnemyId { get; set; }
        public int CurrentAlertValue { get; set; }

        public GameEnemyAlertEvt(int enemyId, int currentAlertValue)
        {
            EnemyId = enemyId;
            CurrentAlertValue = currentAlertValue;
        }
    }
}