using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class WallTile : MonoBehaviour
{
    public GameObject[] ContentPrefab = new GameObject[3];
    private Collider _WallCollider;
    private Tile _ParentTile;

    public enum WallPositions
    {
        NONE = -1,

        Left,
        Right,
        Center,

        Random,
        ALL,
    }

    private void Awake()
    {
        _WallCollider = GetComponent<Collider>();
        _ParentTile = transform.parent.GetComponent<Tile>();
        MXUtilities.MXDebug.Log($"WallTile Prent = {_ParentTile.name}");
    }
    private void Start()
    {
        InstantiateWallObjects();
    }

    public GameObject InstantiateWallObject(GameObject prefab = null, WallPositions wallPos = WallPositions.NONE)
    {
        if(prefab != null)
        {
            ContentPrefab[(int)wallPos] = prefab;
        }

        if (wallPos == WallPositions.Left || wallPos == WallPositions.Center || wallPos == WallPositions.Right)
        {
            if (ContentPrefab[(int)wallPos] != null)
            {
                float xOffset = 0;

                switch (wallPos)
                {
                    case WallPositions.Left:  //left
                        {
                            xOffset = -(_WallCollider.bounds.size.x / 3);
                            break;
                        }
                    case WallPositions.Center: //center
                        {
                            xOffset = 0;
                            break;
                        }
                    case WallPositions.Right: //right
                        {
                            xOffset = _WallCollider.bounds.size.x / 3;
                            break;
                        }
                    default: { return null; }
                }

                GameObject wallObj = Instantiate(ContentPrefab[(int)wallPos]);
                wallObj.transform.rotation = transform.rotation;
                wallObj.transform.Rotate(0, 180, 0);

                wallObj.transform.SetParent(transform);
                wallObj.transform.localPosition = new Vector3(xOffset, -_WallCollider.bounds.extents.y, +0.15f);

                if(wallObj.TryGetComponent<IInteractable>(out IInteractable intObject))
                {
                //    if(intObject.GetType() == typeof(Torch))
                //    {
                //        Torch t = (Torch)intObject;
                //        Room r = _ParentTile.Parent.GetComponent<Room>();

                //        if(r.TorchesList.Count < r.MaxRoomTorches)
                //        {
                //            r.TorchesList.Add(t);
                //            t.Parent = r.transform;
                //        }

                //        MXUtilities.MXDebug.Log("Torch added");
                //    }
                wallObj.transform.SetParent(null);
                }
                return wallObj;

            }
        }
        return null;
    }



    private void InstantiateWallObjects()
    {
        for(int i = 0; i < ContentPrefab.Length; i++)
        {
            InstantiateWallObject(null,(WallPositions) i);
        }
    }
}
