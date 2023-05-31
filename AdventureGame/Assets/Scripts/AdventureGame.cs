using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public static class AdventureGame
{
    public enum TriggerType
    {
        NONE = -1,
        Shop,
        AdventureGuild,
        Dungeon,
        All = 5,
    }

    public static class AGDungeons
    {
        public const float DUNGEON_UNIT = 0.4f;
        public const float TILE_OFFSET = 0.2f;
        public const int MIN_X = 30;
        public const int MAX_X = 180;
        public const int MIN_Y = 30;
        public const int MAX_Y = 180;

        public const int MIN_ROOMS = 1;
        public const int MAX_ROOMS = 100;

        public const float MIN_DISTANCE_BEETWEENROOMS = DUNGEON_UNIT;
        public const float MAX_DISTANCE_BEETWEENROOMS = DUNGEON_UNIT * 10;


        public static class Rooms
        {
            public const int MIN_X = 3;
            public const int MAX_X = 9;
            public const int MIN_Y = 3;
            public const int MAX_Y = 9;
        }

    }

    public static class Direction
    {
        public enum Directions
        {
            NONE = -1,
            North = 0,
            South = 1,
            East = 2,
            West = 3,
            ALL = 4,
        }

        public static Vector3 North = new Vector3 (0, 0, 1);
        public static Vector3 South = new Vector3(0, 0, -1);
        public static Vector3 East = new Vector3(1, 0, 0);
        public static Vector3 West = new Vector3(-1, 0, 0);
    }

    
}
