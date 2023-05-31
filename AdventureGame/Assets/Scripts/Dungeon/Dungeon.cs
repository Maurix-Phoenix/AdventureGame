using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static AdventureGame;
using static MXUtilities;

public class Dungeon : MonoBehaviour
{
    public static Dungeon Instance;


    [Header("Dungeon Structure")]
    public int RoomsNumber = 2;
    public Vector2Int RoomSizeXRange = new Vector2Int(3, 9);
    public Vector2Int RoomSizeYRange = new Vector2Int(3, 9);
    public Vector2Int RoomsDistanceInTile = new Vector2Int(0,3);
    public List<Room> Rooms = new List<Room>();
    public List<Tile> Tiles = new List<Tile>();
    public List<Tile> CorridorTiles = new List<Tile>();

    [Header("Dungeon Objects")]
    public GameObject GroundPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GenerateRooms(RoomsNumber);
    }

    private void GenerateRooms(int n)
    {
        string roomTag = "";
        Vector3 roomPosition = new Vector3();
        Vector2Int roomSize = new Vector2Int();

        for (int i = 0; i < n; i++)
        {
            //for each room create a blank object and randomize distance, also initialize the vars inside the room class
            int distanceInTiles = Random.Range(RoomsDistanceInTile.x, RoomsDistanceInTile.y);
            float distance = distanceInTiles * AGDungeons.DUNGEON_UNIT;
            MXDebug.Log($"Creating room n {i}");
            Room newRoom = new GameObject($"newRoom {i}").AddComponent<Room>();
            bool isValidRoom = false;
            newRoom.Initialize(); 

            while(!isValidRoom)
            {
                if (i == 0)
                {
                    roomTag = "Starting Room";
                    roomPosition = new Vector3(0, 0, 0);
                    roomSize = new Vector2Int(3, 3);
                    isValidRoom = true;
                    break;
                }
                else if (i == n - 1)
                {
                    roomTag = "Boss Room";
                }
                else
                {
                    roomTag = $"Normal Room{i}";
                }
                roomSize = new Vector2Int(Random.Range(RoomSizeXRange.x,RoomSizeXRange.y), 
                                          Random.Range(RoomSizeYRange.x, RoomSizeYRange.y));

                //searching other existing rooms to connect the new room
                List<Room> possibleConnectingRooms = GetPossibleConnectingRooms();
                if (possibleConnectingRooms.Count > 0)
                {
                    Room possibleRoom = possibleConnectingRooms[Random.Range(0, possibleConnectingRooms.Count)];
                    if (possibleRoom != null)
                    {
                        int countFreeConnectionPoints = 0;
                        List<int> connectionPositionList = new List<int>();
                        for (int fcp = 0; fcp < (int)Direction.Directions.ALL; fcp++)
                        {
                            if (possibleRoom.ConnectedRooms[fcp] == null)
                            {
                                connectionPositionList.Add(fcp);
                                countFreeConnectionPoints++;
                            }
                        }

                        while (connectionPositionList.Count > 0)
                        {
                            //basically this will try to create the room picking a random connection point of the possible "linked" room
                            int connectionPosition = connectionPositionList[Random.Range(0, connectionPositionList.Count)];
                            int newRoomConnectionPosition;
                            //detect the direction and the position based on direction of the room based on the possible "Linked" room
                            switch ((Direction.Directions)connectionPosition)
                            {
                                case Direction.Directions.North:
                                    {
                                        newRoomConnectionPosition = (int)Direction.Directions.South;
                                        roomPosition = new Vector3(possibleRoom.Position.x, 0, possibleRoom.Position.z + (possibleRoom.Size.y * AGDungeons.DUNGEON_UNIT) + distance);
                                        break;
                                    }
                                case Direction.Directions.South:
                                    {
                                        newRoomConnectionPosition = (int)Direction.Directions.North;

                                        roomPosition = new Vector3(possibleRoom.Position.x, 0, possibleRoom.Position.z - (roomSize.y * AGDungeons.DUNGEON_UNIT) - distance);
                                        break;
                                    }
                                case Direction.Directions.East:
                                    {
                                        newRoomConnectionPosition = (int)Direction.Directions.West;

                                        roomPosition = new Vector3(possibleRoom.Position.x + (possibleRoom.Size.x * AGDungeons.DUNGEON_UNIT) + distance, 0, possibleRoom.Position.z);
                                        break;
                                    }
                                case Direction.Directions.West:
                                    {
                                        newRoomConnectionPosition = (int)Direction.Directions.East;

                                        roomPosition = new Vector3(possibleRoom.Position.x - (roomSize.x * AGDungeons.DUNGEON_UNIT) - distance, 0, possibleRoom.Position.z);
                                        break;
                                    }
                                default: continue;
                            }

                            MXDebug.Log($"Room{i} - chosen position: {(Direction.Directions)connectionPosition} from room: {possibleRoom.name}");

                            //check if position is good
                            if (IsSpaceFree(roomPosition,roomSize)) 
                            { 
                                //position is valid, will create the room
                                MXDebug.Log($"Room{i} - Position VALID creating room!");
                                possibleRoom.ConnectedRooms[connectionPosition] = newRoom;
                                newRoom.ConnectedRooms[newRoomConnectionPosition] = possibleRoom;
                                isValidRoom = true;
                                break;
                            }
                            else
                            {
                                //position is invalid, will go back and search for other connections
                                MXDebug.Log($"Room{i} - Position Invalid");
                                possibleRoom.ConnectedRooms[connectionPosition] = null;
                                newRoom.ConnectedRooms[newRoomConnectionPosition] = null;
                                connectionPositionList.Remove(connectionPosition);
                                continue;
                            }
                         }
                        continue;
                    }
                    else continue;
                }
                else continue;
            }
             
            if(isValidRoom)
            {
                newRoom.CreateRoom(roomPosition, roomSize, transform, roomTag);
                CreateRoomCorridor(newRoom, distanceInTiles);
                Rooms.Add(newRoom);
            }
        }
    }

    private void CreateRoomCorridor(Room room, int distanceInTiles)
    {
        Room linkedRoom = null;
        int cardinalPoint = -1;
        float middlePoint = 0; ;
        Vector3 corridorPos = new Vector3(0,0,0);
        if(room != null && room.Tag != "Starting Room")
        {
            if (distanceInTiles > 0)
            {
                //getting the room connected (should be only one since the call of this method is right after the room creation).
                for (int dir = 0; dir < (int)Direction.Directions.ALL; dir++)
                {
                    if (room.ConnectedRooms[dir] != null)
                    {
                        linkedRoom = room.ConnectedRooms[dir];
                        cardinalPoint = dir;
                    }
                }


                float a, b;
                float unitC = 0;
                //placing the tiles
                for (int i = 0; i < distanceInTiles; i++)
                {
                    unitC += AGDungeons.DUNGEON_UNIT;
                    //centering the corridor based on the smallest room's sides
                    switch ((Direction.Directions)cardinalPoint)
                    {
                        case Direction.Directions.North:
                            {
                                b = linkedRoom.Size.x / 2 * AGDungeons.DUNGEON_UNIT;
                                a = room.Size.x / 2 * AGDungeons.DUNGEON_UNIT;
                                middlePoint = Mathf.Min(a, b);
                                corridorPos = new Vector3(room.Position.x + middlePoint, 0, linkedRoom.Position.z - unitC);
                                break;
                            }
                        case Direction.Directions.South:
                            {
                                b = linkedRoom.Size.x / 2 * AGDungeons.DUNGEON_UNIT;
                                a = room.Size.x / 2 * AGDungeons.DUNGEON_UNIT;
                                middlePoint = Mathf.Min(a, b);
                                corridorPos = new Vector3(room.Position.x + middlePoint, 0, room.Position.z - unitC);
                                break;
                            }
                        case Direction.Directions.East:
                            {
                                b = linkedRoom.Size.y / 2 * AGDungeons.DUNGEON_UNIT;
                                a = room.Size.y / 2 * AGDungeons.DUNGEON_UNIT;
                                middlePoint = Mathf.Min(a, b);
                                corridorPos = new Vector3(linkedRoom.Position.x - unitC, 0, room.Position.z + middlePoint);
                                break;
                            }
                        case Direction.Directions.West:
                            {
                                b = (linkedRoom.Size.y / 2) * AGDungeons.DUNGEON_UNIT;
                                a = room.Size.y / 2 * AGDungeons.DUNGEON_UNIT;
                                middlePoint = Mathf.Min(a, b);
                                corridorPos = new Vector3(room.Position.x - unitC, 0, room.Position.z + middlePoint);
                                break;
                            }
                    }
                    Tile corridorTile = new GameObject($"CorridorTile").AddComponent<Tile>();
                    corridorTile.CreateTile(corridorPos, transform);
                    Tiles.Add(corridorTile);
                    CorridorTiles.Add(corridorTile);
                    corridorTile.name = $"Corridor {CorridorTiles.IndexOf(corridorTile)}";
                }
            }
        }
    }

    private List<Room>GetPossibleConnectingRooms()
    {
        //possibles room are determined if they have one of the four cardinal point null. (connectedrooms)
        List<Room> possiblesRooms = new List<Room>();
        foreach (Room room in Rooms)
        {
            int count = 0;
            for(int i = 0; i< (int)Direction.Directions.ALL; i++)
            {
                if (room.ConnectedRooms[i] == null)
                {
                    count++;
                }
            }
            if(count > 0)
            {
                possiblesRooms.Add(room);
            }
        }
        return possiblesRooms;
    }

    private bool IsSpaceFree(Vector3 position, Vector2Int size)
    {
        //"virtual area" the room occupy 
        Vector3 minPosition = new Vector3(position.x, 0, position.z);
        Vector3 maxPosition = new Vector3(position.x + (size.x * AGDungeons.DUNGEON_UNIT), 0, position.z + (size.y * AGDungeons.DUNGEON_UNIT));

        //if only one of the dungeon tiles is already inside that area will return false
        foreach (Tile tile in Tiles)
        {
            if(tile.Position.x >= minPosition.x && tile.Position.x <= maxPosition.x &&
               tile.Position.z >= minPosition.z && tile.Position.z <= maxPosition.z)
            {
                return false;
            }
        }
        return true;
    }

}
