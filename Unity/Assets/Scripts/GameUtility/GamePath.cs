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
        public static string SFXCandle = AudioPath + "SFXCandle";
        public static string SFXCandleLoop = AudioPath + "SFXCandleLoop";
        public static string SFXHoverLoop = AudioPath + "SFXHoverLoop";
        public static string SFXBarrel = AudioPath + "SFXBarrel";
        public static string SFXRebound = AudioPath + "SFXRebound";
        public static string SFXForwardShoot = AudioPath + "SFXForwardShoot";
        public static string SFXRecycleShoot = AudioPath + "SFXRecycleShoot";
        public static string SFXWall = AudioPath + "SFXWall";
        public static string SFXBullet = AudioPath + "SFXBullet";
        public static string SFXTrainLoop = AudioPath + "SFXTrainLoop";
        public static string SFXEnemyAttack = AudioPath + "SFXEnemyAttack";
        public static string SFXEnemyDie = AudioPath + "SFXEnemyDie";

        public static string SFXBulletHit = AudioPath + "SFXBulletHit";
        public static string SFXUIClick = AudioPath + "SFXUIClick";
        public static string SFXUIGameStartClick = AudioPath + "SFXUIGameStartClick";
        public static string SFXGameDieByEnemy = AudioPath + "SFXGameDieByEnemy";
        public static string SFXGameDieByOther = AudioPath + "SFXGameDieByOther";
        public static string SFXGamePass = AudioPath + "SFXGamePass";
        
        public static string MusicGameMain =  AudioPath + "MusicGameMain";
        public static string MusicGameDie =  AudioPath + "MusicGameDie";
        public static string MusicGame1And2Level =  AudioPath + "MusicGame1And2Level";
        public static string MusicGame3And4Level =  AudioPath + "MusicGame3And4Level";
        public static string MusicGame5And6Level =  AudioPath + "MusicGame5And6Level";
        public static string MusicGame7And8Level =  AudioPath + "MusicGame7And8Level";
        public static string MusicGame9Level =  AudioPath + "MusicGame9Level";
        public static string MusicGame10Level =  AudioPath + "MusicGame10Level";
    }
}