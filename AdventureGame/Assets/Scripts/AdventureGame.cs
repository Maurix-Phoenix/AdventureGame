using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public static class AdventureGame
{
    public enum TriggerType
    {
        NONE = -1,
        LoadScene,
    }

    public static class AGDungeons
    {
        public const float DUNGEON_UNIT = 0.8f;
        public const float TILE_OFFSET = 0.4f;
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

        public static Directions GetOppositeDirection(Directions d)
        {
            Directions oppositeD = Directions.NONE;
            switch (d)
            {
                case Directions.North:
                {
                    oppositeD = Directions.South;
                    break;
                }
               case Directions.South: 
               {
                    oppositeD = Directions.North;
                    break;
               }
               case Directions.East:
               {
                    oppositeD = Directions.West;
                    break;
               }
                case Directions.West:
                {
                    oppositeD = Directions.East;
                    break;
                }
                default: { oppositeD = Directions.NONE; break; }
            }
            return oppositeD;
        }

        public static Vector3 North = new Vector3 (0, 0, 1);
        public static Vector3 South = new Vector3(0, 0, -1);
        public static Vector3 East = new Vector3(1, 0, 0);
        public static Vector3 West = new Vector3(-1, 0, 0);
    }

    
}
