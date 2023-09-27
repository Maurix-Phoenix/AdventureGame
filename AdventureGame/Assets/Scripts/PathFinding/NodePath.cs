using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MXNodes
{
    public class NodePath : MonoBehaviour
    {
        public NodeMap NodeMap;

        public List<Node> FinalPath = new List<Node>();
        public Vector3 StartingPosition = Vector3.zero;
        public Vector3 EndingPosition = Vector3.zero;

        public float PathTickTime = 1.0f;
        public float MoveSpeed = 1f;

        public bool EDITOR_ShowPath = false;
        #region Heuristic
        public enum HeuristicMode
        {
            Manhattan,
            Euclidean,
            Chebyshev,
        }
        public HeuristicMode Heuristic = HeuristicMode.Manhattan;


        public float CalculateHeuristic(HeuristicMode mode, Node nodeA, Node nodeB)
        {
            float x = Mathf.Abs(nodeA.GridPosition.x - nodeB.GridPosition.x);
            float y = Mathf.Abs(nodeA.GridPosition.y - nodeB.GridPosition.y);

            float heuristc = x + y;
            switch (mode)
            {
                case HeuristicMode.Manhattan:
                    heuristc = (float)x + y;
                    break;
                case HeuristicMode.Euclidean:
                    heuristc = (float)Mathf.Sqrt(x * x + y * y);
                    break;
                case HeuristicMode.Chebyshev:
                    heuristc = (float)Mathf.Max(x, y);
                    break;
            }
            return heuristc;
        }
        #endregion


        private void Start()
        {
            NodeMap = FindAnyObjectByType<NodeMap>();
           // InvokeRepeating("PathTick", 0, PathTickTime);
        }

        //private void PathTick()
        //{
        //    if(NodeMap.NodeMapGrid != null)
        //    {
        //        FindPath(StartingPosition, EndingPosition);

        //    }
        //}

        private void Update()
        {
            StartingPosition = transform.position;
            FindPath(StartingPosition, EndingPosition);
        }

        //A* (A-Star) alghoritm
        private void FindPath(Vector3 startPos, Vector3 endPos)
        {
            Node startNode = NodeMap.GetNodeFromWorldPoint(NodeMap.ActiveNodes, startPos);
            Node endNode = NodeMap.GetNodeFromWorldPoint (NodeMap.ActiveNodes, endPos);

            List<Node> openList = new List<Node>();
            HashSet<Node> closedList = new HashSet<Node>();

            openList.Add(startNode);
            while(openList.Count > 0)
            {
                Node currenNode = openList[0];
                for(int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].TotalCost < currenNode.TotalCost ||
                        openList[i].TotalCost == currenNode.TotalCost && openList[i].ExitCost < currenNode.ExitCost)
                    {
                        currenNode = openList[i];
                    }
                }

                openList.Remove(currenNode);
                closedList.Add(currenNode);

                if(currenNode == endNode)
                {
                    GetFinalPath(startNode, endNode);
                }

                //Check neighbours
                foreach(Node neighbourNode in NodeMap.GetLinkedNodes(currenNode, NodeMap.DirectionMode))
                {
                    //if is not valid or is  already been checked 
                    if (!neighbourNode.IsActive || closedList.Contains(neighbourNode)) 
                    {
                        continue;
                    }
                    //total cost (F cost in A*)
                    float moveCost = (currenNode.EntryCost + CalculateHeuristic(Heuristic, currenNode, neighbourNode));

                    if(moveCost < neighbourNode.EntryCost || !openList.Contains(neighbourNode))
                    {
                        neighbourNode.EntryCost = moveCost;
                        neighbourNode.ExitCost = CalculateHeuristic(Heuristic, neighbourNode, endNode);
                        neighbourNode.ParentNode = currenNode;

                        if(!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }


        }

        private void GetFinalPath(Node startNode, Node endNode)
        {
            List<Node> fp = new List<Node>();
            Node currenNode = endNode;

            while (currenNode != startNode)
            {
                fp.Add(currenNode);
                currenNode = currenNode.ParentNode;
            }
            fp.Reverse();
            FinalPath = fp;
        }

        public Vector3 SetDestination(Vector3 destination)
        {
            FinalPath.Clear();
            EndingPosition = destination;
            return destination;
        }

        public void MoveToDestination(float speed)
        {

            Vector3 correctDirection = Vector3.zero;
            if (FinalPath.Count > 0)
            {
                correctDirection = new Vector3(FinalPath[0].WorldPosition.x - transform.position.x, 0, FinalPath[0].WorldPosition.z - transform.position.z).normalized;
                transform.position += correctDirection * Time.deltaTime * speed;
                Vector3 lookNode = new Vector3(FinalPath[0].WorldPosition.x, transform.position.y, FinalPath[0].WorldPosition.z);
                transform.LookAt(lookNode);
            }
            else
            {
                correctDirection = new Vector3(EndingPosition.x - transform.position.x, 0, EndingPosition.z - transform.position.z).normalized;
                transform.position += correctDirection * Time.deltaTime * speed;
                transform.LookAt(new Vector3(EndingPosition.x, transform.position.y, EndingPosition.z));
            }
            
        }


        private void OnDrawGizmos()
        {
            if (EDITOR_ShowPath)
            {


                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(StartingPosition, EndingPosition);

                Gizmos.color = Color.blue;
                if (FinalPath.Count > 0)
                {
                    foreach (Node node in FinalPath)
                    {
                        Gizmos.DrawLine(node.WorldPosition, node.ParentNode.WorldPosition);
                    }
                }
            }
        }





    }
}

