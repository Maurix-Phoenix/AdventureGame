using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static AdventureGame;
using static MXUtilities;

public class Room : MonoBehaviour
{
    public Transform Parent { get; private set; }
    public string Tag { get; private set; }
    public Vector2Int Size { get; private set; }
    public Vector3 Position { get; private set; }
    public bool IsConnected { get; private set; }

    public Room[] ConnectedRooms = new Room[(int)Direction.Directions.ALL];

    public List<Tile> Tiles = new List<Tile>();

    public void Initialize()
    {
        Parent = null;
        Tag = "TMP Rooms";
        Position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
        Size = new Vector2Int(AGDungeons.Rooms.MIN_X,AGDungeons.Rooms.MAX_X);
        IsConnected = false;
    }

    public void CreateRoom(Vector3 position, Vector2Int size, Transform parent, string tag)
    {
        Parent = parent;
        Position = position; 
        Size = size;
        Tag = tag;

        transform.name = Tag;
        transform.position = Position;

        for(int i = 0; i < Size.x; i++)
        {
            for(int j  = 0; j < Size.y; j++)
            {
                Tile tile = new GameObject($"Tile {Position.x},{Position.z}").AddComponent<Tile>();
                tile.LocalPosition = new Vector2Int(i, j);
                tile.CreateTile(new Vector3(Position.x + i * AGDungeons.DUNGEON_UNIT , 0, Position.z + j * AGDungeons.DUNGEON_UNIT), transform);
                Tiles.Add(tile);
                Dungeon.Instance.Tiles.Add(tile);
            }
        }

        transform.SetParent(Parent);
    }


    private void OnDrawGizmosSelected()
    {
        #if UNITY_EDITOR
        //overlap debug
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(new Vector3(Position.x + (Size.x/2 * AGDungeons.DUNGEON_UNIT), 0, Position.z + (Size.y /2 * AGDungeons.DUNGEON_UNIT)),
                            new Vector3(Size.x * AGDungeons.DUNGEON_UNIT, 1, Size.y * AGDungeons.DUNGEON_UNIT));  
        #endif
    }

}
