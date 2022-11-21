namespace Game
{
    public static class GamePath
    {
        //Prefab
        public static string PrefabPath = "Prefabs/";
        public static string ItemPrefabPath = "Item/";
        public static string BulletPfb = PrefabPath + ItemPrefabPath + "Bullet";
        public static string BulletOnwallPfb = PrefabPath + ItemPrefabPath + "BulletOnwall";
        public static string FireEffectPfb = ItemPrefabPath + "FireEffect";
        public static string RecycleEffectPfb = ItemPrefabPath + "RecycleEffect";

        //Animation
        public static string AnimationPath = "Animation/";
        public static string GunClip = AnimationPath + "Gun";
        public static string EnemyClip = AnimationPath + "Enemy";
    }
}