public static class Constants
{
    public enum LayerMaskIds : int
    {
        Default = 1 << 0,
        IgnoreRaycast = 1 << 2,
        Obstacle = 1 << 6,
        MainFloor = 1 << 7,
        Soldier = 1 << 8,
    }

    public enum GridTileColor
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow,
        Orange,
    }

    public static class SoldierActionNames
    {
        public const string Move = "MOVE";
        public const string Spin = "SPIN";
        public const string Shoot = "SHOOT";
        public const string Grenade = "GRENADE";
        public const string Sword = "SWORD";
        public const string Interact = "INERACT";
    }

    public enum SoldierActionTargetTypes
    {
        Self,
        Enemy,
        EmptyTile,
        Any,
        Door,
    }
}
