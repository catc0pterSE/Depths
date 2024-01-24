using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

namespace PathFindingSystem
{
    public class Pathfinder : MonoBehaviour, IPathFinding
    {
        public struct Node
        {
            public int2 coord;
            public int2 parent;
            public int gScore;
            public int hScore;
        }

        Hashtable obstacles;
        int safeGuard = 1000;
        private NativeHashMap<int2, bool> isObstacle;
        private NativeHashMap<int2, Node> nodes;
        private NativeArray<int2> offsets;
        private NativeHashMap<int2, Node> openSet;

        private void Start()
        {
            obstacles = new Hashtable();

            openSet =
                new NativeHashMap<int2, Node>(safeGuard, Allocator.TempJob);

            isObstacle =
                new NativeHashMap<int2, bool>(obstacles.Count, Allocator.TempJob);
            nodes =
                new NativeHashMap<int2, Node>(safeGuard, Allocator.TempJob);

            offsets =
                new NativeArray<int2>(8, Allocator.TempJob);
        }

        // Update is called once per frame
        public List<Vector3> FindPath(Vector3Int startPosition, Vector3Int finishPosition)
        {
            var start = new Node
            {
                coord = new int2(startPosition.x, startPosition.y), parent = int2.zero, gScore = int.MaxValue,
                hScore = int.MaxValue
            };
            var end = new Node
            {
                coord = new int2(finishPosition.x, finishPosition.y), parent = int2.zero, gScore = int.MaxValue,
                hScore = int.MaxValue
            };

            // foreach (int2 o in obstacles.Keys)
            // {
            //     isObstacle.Add(o, true);
            // }
            // NativeHashMap<int2, Node> openSet =
            //     new NativeHashMap<int2, Node>(safeGuard, Allocator.TempJob);

            offsets[0] = new int2(0, 1);
            offsets[1] = new int2(1, 1);
            offsets[2] = new int2(1, 0);
            offsets[3] = new int2(1, -1);
            offsets[4] = new int2(0, -1);
            offsets[5] = new int2(-1, -1);
            offsets[6] = new int2(-1, 0);
            offsets[7] = new int2(-1, 1);


            AStar aStar = new AStar
            {
                isObstacle = isObstacle,
                nodes = nodes,
                start = start,
                openSet = openSet,
                offsets = offsets,
                end = end,
                safeGuard = safeGuard
            };

            JobHandle handle = aStar.Schedule();
            handle.Complete();

            //NativeArray<Node> nodeArray = nodes.GetValueArray(Allocator.TempJob);

            List<Vector3> path = ListPool<Vector3>.Get();

            if (nodes.ContainsKey(end.coord))
            {
                path.Add(new Vector3(end.coord.x, end.coord.y, 0));

                int2 currentCoord = end.coord;

                while (!currentCoord.Equals(start.coord))
                {
                    currentCoord = nodes[currentCoord].parent;
                    path.Add(new Vector3(currentCoord.x, currentCoord.y, 0));
                }
            }


            nodes.Clear();
            isObstacle.Clear();

            openSet.Clear();
            //odeArray.Dispose();


            if (path.Count == 0)
            {
                path.Add(startPosition);
            }

            return path;
        }

        private void OnDestroy()
        {
            openSet.Dispose();
            offsets.Dispose();
            nodes.Dispose();
            isObstacle.Dispose();
        }

        [BurstCompile(CompileSynchronously = true)]
        private struct AStar : IJob
        {
            public NativeHashMap<int2, bool> isObstacle;
            public NativeHashMap<int2, Node> nodes;
            public NativeHashMap<int2, Node> openSet;
            public NativeArray<int2> offsets;
            public Node start;
            public Node end;

            public int safeGuard;

            [BurstCompile]
            public void Execute()
            {
                Node current = start;
                current.gScore = 0;
                current.hScore = SquaredDistance(current.coord, end.coord);

                openSet.TryAdd(current.coord, current);

                int counter = 0;

                do
                {
                    current = openSet[ClosestNode(openSet)];
                    nodes.TryAdd(current.coord, current);

                    for (int i = 0; i < offsets.Length; i++)
                    {
                        if (!nodes.ContainsKey(current.coord + offsets[i]) &&
                            !isObstacle.ContainsKey(current.coord + offsets[i]))
                        {
                            Node neighbour = new Node
                            {
                                coord = current.coord + offsets[i],
                                parent = current.coord,
                                gScore = current.gScore +
                                         SquaredDistance(current.coord, current.coord + offsets[i]),
                                hScore = SquaredDistance(current.coord + offsets[i], end.coord)
                            };

                            if (openSet.ContainsKey(neighbour.coord) && neighbour.gScore <
                                openSet[neighbour.coord].gScore)
                            {
                                openSet[neighbour.coord] = neighbour;
                            }
                            else if (!openSet.ContainsKey(neighbour.coord))
                            {
                                openSet.TryAdd(neighbour.coord, neighbour);
                            }
                        }
                    }

                    openSet.Remove(current.coord);
                    counter++;

                    if (counter > safeGuard)
                        break;
                } while (openSet.Count() != 0 && !current.coord.Equals(end.coord));
            }

            private int SquaredDistance(int2 coordA, int2 coordB)
            {
                int a = coordB.x - coordA.x;
                int b = coordB.y - coordA.y;
                return a * a + b * b;
            }

            private int2 ClosestNode(NativeHashMap<int2, Node> openSet)
            {
                Node result = new Node();
                int fScore = int.MaxValue;

                NativeArray<Node> nodeArray = openSet.GetValueArray(Allocator.Temp);

                for (int i = 0; i < nodeArray.Length; i++)
                {
                    if (nodeArray[i].gScore + nodeArray[i].hScore < fScore)
                    {
                        result = nodeArray[i];
                        fScore = nodeArray[i].gScore + nodeArray[i].hScore;
                    }
                }

                nodeArray.Dispose();
                return result.coord;
            }
        }
    }

    public interface IPathFinding
    {
        List<Vector3> FindPath(Vector3Int startPosition, Vector3Int finishPosition);
    }
}