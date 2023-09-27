using UnityEngine;
using static AdventureGame;

public class Tile : MonoBehaviour
{

    public enum Types
    {
        None = -1,
        Room,
        Corridor,
    }
    public Types Type;

    public Vector2Int LocalPosition;
    public Transform Parent { get; private set; }
    public Vector3 WorldPosition;

    public Tile[] ConnectedTiles = new Tile[(int)Direction.Directions.ALL];
    public GameObject[] Boundaries =new GameObject[(int)Direction.Directions.ALL];
    public void CreateTile(Vector3 position, Transform parent, Types typ)
    {
        Parent = parent;
        WorldPosition = position;
        transform.position = WorldPosition;
        Type = typ;
        if(IsSpaceFree(position, new Vector2Int(1,1)))
        {
            Instantiate(Dungeon.Instance.GroundPrefab, WorldPosition, Quaternion.identity, transform);
        }
        else
        {
            Destroy(this);
        }

        transform.SetParent(Parent);
    }


    private bool IsSpaceFree(Vector3 position, Vector2Int size)
    {
        //"virtual area" the room occupy 
        Vector3 minPosition = new Vector3(position.x - (size.x/2 * AGDungeons.DUNGEON_UNIT), 0, position.z-(size.y/2 * AGDungeons.DUNGEON_UNIT));
        Vector3 maxPosition = new Vector3(position.x + (size.x/2 * AGDungeons.DUNGEON_UNIT), 0, position.z + (size.y/2 * AGDungeons.DUNGEON_UNIT));

        //if only one of the dungeon tiles is already inside that area will return false
        foreach (Tile tile in Dungeon.Instance.Tiles)
        {
            if (tile.WorldPosition.x >= minPosition.x && tile.WorldPosition.x <= maxPosition.x &&
               tile.WorldPosition.z >= minPosition.z && tile.WorldPosition.z <= maxPosition.z)
            {
                return false;
            }
        }
        return true;
    }

    public void FindConnectedTiled()
    {
        //Find the tiles connected to the current one

        Vector3 searchPosition = new Vector3(0, 0, 0);
        for(int i = 0; i < ConnectedTiles.Length; i++)
        {
            if (ConnectedTiles[i] == null) 
            {
                switch ((Direction.Directions)i)
                {
                    case Direction.Directions.North: 
                        {
                            searchPosition = new Vector3(WorldPosition.x, 0, WorldPosition.z + AGDungeons.DUNGEON_UNIT);
                            break;
                        }
                    case Direction.Directions.South: 
                        {
                            searchPosition = new Vector3(WorldPosition.x, 0, WorldPosition.z - AGDungeons.DUNGEON_UNIT);
                            break;
                        }
                    case Direction.Directions.East: 
                        {
                            searchPosition = new Vector3(WorldPosition.x + AGDungeons.DUNGEON_UNIT, 0, WorldPosition.z);
                            break;
                        }
                    case Direction.Directions.West: 
                        {
                            searchPosition = new Vector3(WorldPosition.x - AGDungeons.DUNGEON_UNIT, 0, WorldPosition.z);
                            break; 
                        }
                }

                for(int n = 0; n < Dungeon.Instance.Tiles.Count; n++)
                {
                    Tile tile = Dungeon.Instance.Tiles[n];
                    if(tile != null && tile != this)
                    {
                        if(tile.WorldPosition == searchPosition) 
                        {
                            ConnectedTiles[i] = tile;
                            int oppositeD = (int)Direction.GetOppositeDirection((Direction.Directions)i);
                            tile.ConnectedTiles[oppositeD] = this;
                        }
                    }
                }

            }
        }
    }

    public void BuildBoundaries() 
    {
        //identify the current tile if and where has walls


        for(int dir = 0; dir < (int)Direction.Directions.ALL; dir++)
        {
            Tile tile = ConnectedTiles[dir];
            if (tile != null)
            {
                RemoveBoundary((Direction.Directions)dir);
                if (tile.Type == Types.Corridor && Type != Types.Corridor)
                {
                    PlaceBoundaries(Dungeon.Instance.DoorwayPrefab, (Direction.Directions)dir);
                }
            }
            else
            {
                PlaceBoundaries(Dungeon.Instance.WallPrefab, (Direction.Directions)dir);
            }
        }
    }

    private void PlaceBoundaries(GameObject prefab, Direction.Directions dir)
    {
        Vector3 pos = new Vector3(0, 0, 0);
        Vector3 rot = new Vector3(0, 0, 0);
        switch (dir)
        {
            case Direction.Directions.North:
                {
                    pos = new Vector3(WorldPosition.x, AGDungeons.DUNGEON_UNIT/2, WorldPosition.z + AGDungeons.DUNGEON_UNIT/2);
                    rot = new Vector3(0, 180, 0);
                    break;
                }
            case Direction.Directions.South:
                {
                    pos = new Vector3(WorldPosition.x, AGDungeons.DUNGEON_UNIT / 2, WorldPosition.z - AGDungeons.DUNGEON_UNIT/2);
                    rot = new Vector3(0, 0, 0);
                    break;
                }
            case Direction.Directions.East:
                {
                    pos = new Vector3(WorldPosition.x + AGDungeons.DUNGEON_UNIT/2, AGDungeons.DUNGEON_UNIT / 2, WorldPosition.z);
                    rot = new Vector3(0, 270, 0);
                    break;
                }
            case Direction.Directions.West:
                {
                    pos = new Vector3(WorldPosition.x - AGDungeons.DUNGEON_UNIT/2, AGDungeons.DUNGEON_UNIT / 2, WorldPosition.z);
                    rot = new Vector3(0, 90, 0);
                    break;
                }
            default: return;
        }

        GameObject boundary = Instantiate(prefab, pos, Quaternion.Euler(rot), transform);
        Room parentRoom = Parent.GetComponent<Room>();

        Boundaries[(int)dir] = boundary;
        if (parentRoom != null)
        {
          
            parentRoom.Boundaries.Add(boundary);
        }
        Dungeon.Instance.Boundaries.Add(boundary);
        

        

    }

    private void RemoveBoundary(Direction.Directions dir)
    {
        Room parentRoom = Parent.GetComponent<Room>();
        GameObject bound = Boundaries[(int)dir];

        Boundaries[(int)dir] = null;
        if (parentRoom != null)
        {
            parentRoom.Boundaries.Remove(bound);
        }
        Dungeon.Instance.Boundaries.Remove(bound);

        Destroy(bound);
    }
}
