namespace Game
{
    public static class GamePath
    {
        //Path
        public static string PrefabPath = "Prefabs/";
        public static string ItemPrefabPath = PrefabPath + "Item/";
        public static string UIPrefabPath = PrefabPath + "UI/";
        public static string AnimationPath = "Animation/";
        public static string AudioPath = "Audio/";

        //Prefab
        public static string BulletPfb = ItemPrefabPath + "Bullet";
        public static string BulletOnwallPfb = ItemPrefabPath + "BulletOnwall";
        public static string BulletShootPlacePfb = ItemPrefabPath + "BulletShootPlace";
        public static string FireEffectPfb = ItemPrefabPath + "FireEffect";
        public static string RecycleEffectPfb = ItemPrefabPath + "RecycleEffect";
        public static string DiePanelPfb = UIPrefabPath + "GameUIDiePanel";
        public static string FinishPanelPfb = UIPrefabPath + "GameUIFinishPanel";
        public static string FadePanelOutPfb = UIPrefabPath + "GameUIFadeOutPanel";
        
        //Animation
        public static string GunClip = AnimationPath + "Gun";
        public static string EnemyClip = AnimationPath + "Enemy";

        //Audio
        public static string CandleSFX = AudioPath + "CandleSFX";
        public static string CandleLoopSFX = AudioPath + "CandleLoopSFX";
        public static string HoverLoopSFX = AudioPath + "HoverLoopSFX";
        public static string BarrelSFX = AudioPath + "BarrelSFX";
        public static string ReboundSFX = AudioPath + "ReboundSFX";
        public static string ForwardShootSFX = AudioPath + "ForwardShootSFX";
        public static string RecycleShootSFX = AudioPath + "RecycleShootSFX";
        public static string WallSFX = AudioPath + "WallSFX";
        public static string BulletSFX = AudioPath + "BulletSFX";
        public static string TrainLoopSFX = AudioPath + "TrainLoopSFX";
        public static string EnemyAttackSFX = AudioPath + "EnemyAttackSFX";
        public static string EnemyDieSFX = AudioPath + "EnemyDieSFX";

        public static string BulletHitSFX = AudioPath + "BulletHitSFX";
        public static string UIClickSFX = AudioPath + "UIClickSFX";
        public static string UIGameStartClickSFX = AudioPath + "UIGameStartClickSFX";
        public static string GameDieByEnemySFX = AudioPath + "GameDieByEnemySFX";
        public static string GameDieByOtherSFX = AudioPath + "GameDieByOtherSFX";
        public static string GamePassSFX = AudioPath + "GamePassSFX";
    }
}