using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

namespace MXNodes
{
    public class NodeMap : MonoBehaviour
    {
        public Node[,] NodeMapGrid;
        public List<Node> ActiveNodes = new List<Node>();
        public List<Node> DisplacingNodes = new List<Node>();
        public Vector3 NodeMapGridOrigin { get{ return transform.position;  } }
        public Vector2Int NodeMapGridSize = Vector2Int.one;
        public Vector3 NodeMapGridCenter { get {return new Vector3(NodeMapGridOrigin.x + (NodeMapGridSize.x/2), NodeMapGridOrigin.y, NodeMapGridOrigin.z + (NodeMapGridSize.y /2)); } }

        public LayerMask ValidNodeMask;
        public LayerMask InvalidNodeMask;

        public float NodeSearchRadius { get { return Mathf.Sqrt(Mathf.Pow((DistanceBetweenNodes + NodeSize) * 2, 2)); } }
        public float NodeSize = 1f;
        public float DistanceBetweenNodes = 1f;


        public bool EDITOR_ShowNodes = false;
        public bool EDITOR_ShowInvalidNodes = false;
        public enum DirectionsMode
        {
            FourWays = 4,
            EightWays = 8,
        }
        public DirectionsMode DirectionMode = DirectionsMode.FourWays;

        private void Start()
        {
            CreateNodeGrid(NodeMapGridOrigin, NodeMapGridSize, NodeSize, DistanceBetweenNodes);
            
            GetNodeFromWorldPoint(ActiveNodes, new Vector3(0,0,0)); //Test
        }

        public void CreateNodeGrid(Vector3 origin, Vector2Int size, float nodeSize, float nodesDistance)
        {
            NodeMapGrid = new Node[size.x, size.y];

            Vector3 gridCenter = new Vector3(origin.x + (size.x - 1) * (nodeSize + nodesDistance) *  0.5f,
                                             origin.y,
                                             origin.z + (size.y - 1) * (nodeSize + nodesDistance) * 0.5f);

            if (size.x > 0 && size.y > 0)
            {
                for (int x = 0; x < size.x; x++)
                {
                    for (int y = 0; y < size.y; y++)
                    {
                        // node position consider the center
                        Vector3 position = new Vector3(x * (nodeSize + nodesDistance) - gridCenter.x,
                                                      origin.y,
                                                      y * (nodeSize + nodesDistance) - gridCenter.z);

                        bool isActive = false;

                        // cast to check the validity of the node
                        if (Physics.CheckBox(position, new Vector3(nodeSize / 2, nodeSize / 2, nodeSize / 2), Quaternion.identity, ValidNodeMask) &&
                            !Physics.CheckBox(position, new Vector3(nodeSize/2, nodeSize/2, nodeSize/2), Quaternion.identity, InvalidNodeMask))
                        {
                            
                            isActive = true;
                        }

                        NodeMapGrid[x, y] = new Node(this, position, new Vector2Int(x, y), isActive);
                        NodeMapGrid[x, y].NodeID = $"N[{x}][{y}]";

                        if (NodeMapGrid[x, y].IsActive)
                        {
                            ActiveNodes.Add(NodeMapGrid[x, y]);
                        }
                    }
                }
            }
        }

        public Node CreateNode(Vector3 position, float nodeSize)
        {
            Vector2Int gridPosition = Vector2Int.zero;
            bool isActive = false;

            if (Physics.CheckBox(position, new Vector3(nodeSize / 2, nodeSize / 2, nodeSize / 2), Quaternion.identity, ValidNodeMask) &&
                          !Physics.CheckBox(position, new Vector3(nodeSize / 2, nodeSize / 2, nodeSize / 2), Quaternion.identity, InvalidNodeMask))
            {

                isActive = true;
            }

            Node node = new Node(this, position, gridPosition, isActive);
            DisplacingNodes.Add(node);

            if (NodeMapGrid != null)
            {
                //ricalcolare la posizione al'interno della griglia
                ReOrderGrid();
            }
            else
            {
                NodeMapGrid[0, 0] = node;
            }


            return node;


        }

        public void ReOrderGrid()
        {
            foreach(Node node in DisplacingNodes)
            {
                for (int i = 0; i < NodeMapGrid.Length; i++)
                {

                }
            }

        }

        public Node GetNodeFromWorldPoint( List<Node>nodeCollection, Vector3 worldP, bool onlyActiveNodes = true)
        {
            if (nodeCollection.Count > 0)
            {
                List<Node> nodes = new List<Node>(nodeCollection); //needed for orderby
                if (onlyActiveNodes)
                {
                    if (nodes.Count > 0)
                    {
                        Node nearNode = nodes.OrderBy(node => Vector3.Distance(node.WorldPosition, worldP)).First();
                        return nearNode;
                    }
                    return null;
                }
            }
            return null;
        }

        public List<Node>GetLinkedNodes(Node node, DirectionsMode dm)
        {
            if (NodeMapGrid != null)
            {
                List<Node> linkedNodes = new List<Node>((int)dm);

                Vector2Int northNode = new Vector2Int(node.GridPosition.x, node.GridPosition.y + 1);
                Vector2Int southNode = new Vector2Int(node.GridPosition.x, node.GridPosition.y - 1);
                Vector2Int eastNode = new Vector2Int(node.GridPosition.x + 1, node.GridPosition.y);
                Vector2Int westNode = new Vector2Int(node.GridPosition.x - 1, node.GridPosition.y);

                //4ways movement
                if (IsInGridRange(northNode)) { linkedNodes.Add(NodeMapGrid[northNode.x, northNode.y]); }
                if (IsInGridRange(southNode)) { linkedNodes.Add(NodeMapGrid[southNode.x, southNode.y]); }
                if (IsInGridRange(eastNode)) { linkedNodes.Add(NodeMapGrid[eastNode.x, eastNode.y]); }
                if (IsInGridRange(westNode)) { linkedNodes.Add(NodeMapGrid[westNode.x, westNode.y]); }

                if (dm == DirectionsMode.EightWays)
                {
                    Vector2Int northWestNode = new Vector2Int(node.GridPosition.x - 1, node.GridPosition.y + 1);
                    Vector2Int northEastNode = new Vector2Int(node.GridPosition.x + 1, node.GridPosition.y + 1);
                    Vector2Int southWestNode = new Vector2Int(node.GridPosition.x - 1, node.GridPosition.y - 1);
                    Vector2Int southEastNode = new Vector2Int(node.GridPosition.x + 1, node.GridPosition.y - 1);
                    //8 ways movement
                    if (IsInGridRange(northWestNode)) { linkedNodes.Add(NodeMapGrid[northWestNode.x, northWestNode.y]); }
                    if (IsInGridRange(northEastNode)) { linkedNodes.Add(NodeMapGrid[northEastNode.x, northEastNode.y]); }
                    if (IsInGridRange(southWestNode)) { linkedNodes.Add(NodeMapGrid[southWestNode.x, southWestNode.y]); }
                    if (IsInGridRange(southEastNode)) { linkedNodes.Add(NodeMapGrid[southEastNode.x, southEastNode.y]); }
                }


                return linkedNodes;
            }
            return null;
        }

        public bool IsInGridRange(Vector2 id)
        {
            if (id.x >= 0 && id.x < NodeMapGridSize.x &&
                id.y >= 0 && id.y < NodeMapGridSize.y)
            {
                return true;
            }
            else return false;
        }


        private void OnDrawGizmos()
        {
            if (EDITOR_ShowNodes)
            {

                if (NodeMapGrid != null)
                {
                    for (int x = 0; x < NodeMapGrid.GetLength(0); x++)
                    {
                        for (int y = 0; y < NodeMapGrid.GetLength(1); y++)
                        {
                            if (EDITOR_ShowInvalidNodes)
                            {
                                Gizmos.color = NodeMapGrid[x, y].IsActive ? Color.white : Color.black;
                                Gizmos.DrawWireCube(NodeMapGrid[x, y].WorldPosition, new Vector3(NodeSize, NodeSize, NodeSize));
                            }
                            else
                            {
                                if (NodeMapGrid[x, y].IsActive)
                                {
                                    Gizmos.color = NodeMapGrid[x, y].IsActive ? Color.white : Color.black;
                                    Gizmos.DrawWireCube(NodeMapGrid[x, y].WorldPosition, new Vector3(NodeSize, NodeSize, NodeSize));
                                }
                            }
                        }
                    }
                }
            }
        }

    }

}


