using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static AdventureGame;
using static UnityEditor.PlayerSettings;

public class Tile : MonoBehaviour
{
    public Vector2Int LocalPosition;
    public Transform Parent;
    public Vector3 Position;

    public void CreateTile(Vector3 position, Transform parent)
    {
        Parent = parent;
        Position = position;
        transform.position = Position;
        Instantiate(Dungeon.Instance.GroundPrefab, Position, Quaternion.identity, transform);

        transform.SetParent(Parent);
    }

    public void Identify()
    {
        for(int i = 0; i < Dungeon.Instance.Tiles.Count; i++)
        {
            Tile t = Dungeon.Instance.Tiles[i];
            if(t != null)
            {

            }
        }
    }
}
