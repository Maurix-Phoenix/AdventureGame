using System;
using System.Collections.Generic;
using UnityEngine;


namespace MXNodes
{
    [Serializable]
    public class Node
    {
        public string NodeID;
        public Node ParentNode;

        public bool IsActive = false;

        public float EntryCost; //distance from the starting point (g cost in A*)
        public float ExitCost; //distance from the ending point (h cost in A*)
        public float TotalCost {get { return EntryCost + ExitCost; } } //(f cost in A*)


        public Vector2Int GridPosition;
        public Vector3 WorldPosition;

        public Node()
        {
            //do nothing
        }
        public Node(NodeMap nodeMap, Vector3 position, Vector2Int gridpos, bool isActive)
        {
            IsActive = isActive;
            WorldPosition = position;
            GridPosition = gridpos;
        }

    }

}
