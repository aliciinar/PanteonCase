namespace Modules.MainModule.Constants
{
    public static class MainConstants
    {
        public const string BOOT_SET = "Boot";
        public const string SCREENS_STEP = "Screens";
        public const string POOLS_STEP = "Pools";

        /// <summary>
        /// The pool groups the boot fills before the main screen opens; an empty list makes the Pools
        /// step skip.
        /// </summary>
        public static readonly string[] BootPoolGroups = { "buildings", "board_buildings", "units", "building_info" };
    }
}
