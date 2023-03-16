public static class Constants
{
    public enum LayerMaskIds : int
    {
        Default = 1 << 0,
        IgnoreRaycast = 1 << 2,
        MainFloor = 1 << 7,
        Soldier = 1 << 8,
    }

    public enum GridTileColor
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow
    }

    public static class SoldierActionNames
    {
        public const string Move = "MOVE";
        public const string Spin = "SPIN";
        public const string Shoot = "SHOOT";
    }

    public enum SoldierActionTargetTypes
    {
        Self,
        Enemy,
        EmptyTile,
    }
}
