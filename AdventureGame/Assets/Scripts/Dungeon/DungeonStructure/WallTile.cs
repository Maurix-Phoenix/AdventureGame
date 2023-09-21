using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public void PlaceObjectOnWall(GameObject obj, WallPositions wallPos)
    {
        if (obj != null)
        {
            switch (wallPos)
            {
                case WallPositions.NONE: { break; }
                case WallPositions.Random:
                    {
                        int randomP = Random.Range(0, ContentPrefab.Length + 1);
                        PlaceObjectOnWall(obj, (WallPositions)randomP);
                        break;
                    }
                case WallPositions.ALL:
                    {
                        for (int i = 0; i < ContentPrefab.Length; i++)
                        {
                            if (ContentPrefab[i] == null)
                            {
                                ContentPrefab[i] = obj;
                            }
                        }
                        break;
                    }
                default:
                    {
                        if (ContentPrefab[(int)wallPos] == null)
                        {
                            ContentPrefab[(int)wallPos] = obj;
                        }

                        break;
                    }
            }
        }
    }

    private void UpdateWallObject(GameObject newOBJ, WallPositions wallPos)
    {
        if(wallPos == WallPositions.Left || wallPos == WallPositions.Center || wallPos == WallPositions.Right)
        {
            if (ContentPrefab[(int)wallPos] != null)
            {
                Destroy(ContentPrefab[(int)wallPos].gameObject);
                ContentPrefab[(int)wallPos] = newOBJ;
            }
            else
            {
                ContentPrefab[(int)wallPos] = newOBJ;
            }
            InstantiateWallObject(wallPos);
        }
    }

    public void DestroyAllObjectsOnWall()
    {
        for(int i = 0; i < ContentPrefab.Length;i++)
        { 
            if (ContentPrefab[i] != null)
            {
                Destroy(ContentPrefab[i].gameObject);
                ContentPrefab[i] = null;
            }
                
        }
    }

    private void InstantiateWallObject(WallPositions wallPos)
    {
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
                }

                GameObject wallObj = Instantiate(ContentPrefab[(int)wallPos]);
                wallObj.transform.rotation = transform.rotation;
                wallObj.transform.Rotate(0, 180, 0);

                wallObj.transform.SetParent(transform);
                wallObj.transform.localPosition = new Vector3(xOffset, -_WallCollider.bounds.extents.y, +0.15f);

                if(wallObj.TryGetComponent<IInteractable>(out IInteractable intObject))
                {
                    if(intObject.GetType() == typeof(Torch))
                    {
                        Torch t = (Torch)intObject;
                        Room r = _ParentTile.Parent.GetComponent<Room>();

                        
                        if(r != null)
                        {

                        }
                        r.TorchesList.Add(t);
                        t.Parent = r.transform;
                        MXUtilities.MXDebug.Log("Torch added");
                    }
                }
                wallObj.transform.SetParent(null);


            }
        }
    }



    private void InstantiateWallObjects()
    {
        for(int i = 0; i < ContentPrefab.Length; i++)
        {
            InstantiateWallObject((WallPositions) i);
        }
    }
}
