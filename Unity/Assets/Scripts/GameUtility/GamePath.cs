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

        //Animation
        public static string GunClip = AnimationPath + "Gun";
        public static string EnemyClip = AnimationPath + "Enemy";

        //Audio
    }
}