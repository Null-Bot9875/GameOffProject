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
        public static string FireEffectPfb = ItemPrefabPath + "FireEffect";
        public static string RecycleEffectPfb = ItemPrefabPath + "RecycleEffect";
        public static string DiePanelPfb = UIPrefabPath + "GameUIDiePanel";
        public static string FinishPanelPfb = UIPrefabPath + "GameUIFinishPanel";
        public static string FadePanelPfb = UIPrefabPath + "GameUIFadePanel";


        //Animation
        public static string GunClip = AnimationPath + "Gun";
        public static string EnemyClip = AnimationPath + "Enemy";

        //Audio
        public static string CandleVFX = AudioPath + "CandleVFX";
        public static string CandleLoopVFX = AudioPath + "CandleLoopVFX";
        public static string HoverLoopVFX = AudioPath + "HoverLoopVFX";
        public static string BarrelVFX = AudioPath + "BarrelVFX";
        public static string ReboundVFX = AudioPath + "ReboundVFX";
        public static string ForwardShootVFX = AudioPath + "ForwardShootVFX";
        public static string RecycleShootVFX = AudioPath + "RecycleShootVFX";
        public static string WallVFX = AudioPath + "WallVFX";
        public static string BulletVFX = AudioPath + "BulletVFX";
        public static string TrainLoopVFX = AudioPath + "TrainLoopVFX";
        public static string EnemyAttackVFX = AudioPath + "EnemyAttackVFX";
        public static string EnemyDieVFX = AudioPath + "EnemyDieVFX";
    }
}