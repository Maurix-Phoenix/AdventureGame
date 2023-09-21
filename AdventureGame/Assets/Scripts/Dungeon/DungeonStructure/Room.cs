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

    public Room[] ConnectedRooms = new Room[(int)Direction.Directions.ALL];

    public List<Tile> Tiles = new List<Tile>();

    public List<GameObject>Boundaries = new List<GameObject>();
    public MobSpawner MobSpawner;

    public Vector2Int RoomTorches = new Vector2Int(4,6);
    public List<Torch> TorchesList = new List<Torch>();

    public void Initialize()
    {
        Parent = null;
        Tag = "TMP Rooms";
        Position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
        Size = new Vector2Int(AGDungeons.Rooms.MIN_X,AGDungeons.Rooms.MAX_X);
    }

    private void OnEnable()
    {
        if(TorchesList.Count > 0)
        {
            foreach (Torch t in TorchesList)
            {
                t.gameObject.SetActive(true);
            }
        }
        
    }
    private void OnDisable()
    {
        if (TorchesList.Count > 0)
        {
            foreach (Torch t in TorchesList)
            {
                t.gameObject.SetActive(false);
            }
        }
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
                tile.CreateTile(new Vector3(Position.x + i * AGDungeons.DUNGEON_UNIT , 0, Position.z + j * AGDungeons.DUNGEON_UNIT), transform, Tile.Types.Room);
                Tiles.Add(tile);
                Dungeon.Instance.Tiles.Add(tile);
            }
        }
        transform.SetParent(Parent);
    }

    public void PlaceRoomTorches(int torchN)
    {
        MXDebug.Log("Placing Torches...");
        if(torchN > RoomTorches.y)
        {
            torchN = RoomTorches.y;
        }
        else if (torchN < RoomTorches.x)
        {
            torchN = RoomTorches.x;
        }

        List<WallTile> wtl = new List<WallTile>();
        for(int i = 0; i < Boundaries.Count;  i++)
        {
            if(Boundaries[i] != null)
            {
                if(Boundaries[i].GetComponent<WallTile>() != null)
                {
                    wtl.Add(Boundaries[i].GetComponent<WallTile>());
                }

                //else is a doorway
            }
        }


        for(int i = 0; i < torchN; i++)
        {
            //select a random wall
            if (wtl.Count > 0)
            {
                WallTile selectedWT = wtl[Random.Range(0, wtl.Count - 1)];
                wtl.Remove(selectedWT);


                //instance the torch
                
                Torch torch = selectedWT.InstantiateWallObject(Dungeon.Instance.TorchPrefab, WallTile.WallPositions.Center).GetComponent<Torch>();
                TorchesList.Add(torch);
            }
        }


    }
}
