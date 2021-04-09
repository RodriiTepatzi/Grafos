using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafos.Resources
{
    class _Dijkstra
    {
        public double[] dist { get; private set; }
        public int[] path { get; private set; }

        private List<int> queue = new List<int>();
        //private int[][] m_adjMatrix;
        //private int v;

        /* Sets up initial settings */
        private void Initialize(int s, int len)
        {
            dist = new double[len];
            path = new int[len];

            /* Set distance to all nodes to infinity - alternatively use Int.MaxValue for use of Int type instead */
            for (int i = 0; i < len; i++)
            {
                dist[i] = Double.PositiveInfinity;

                queue.Add(i);
            }

            /* Set distance to 0 for starting point and the previous node to null (-1) */
            dist[s] = 0;
            path[s] = -1;
        }

        /* Retrives next node to evaluate from the queue */
        private int GetNextVertex()
        {
            double min = Double.PositiveInfinity;
            int Vertex = -1;

            /* Search through queue to find the next node having the smallest distance */
            foreach (int j in queue)
            {
                if (dist[j] <= min)
                {
                    min = dist[j];
                    Vertex = j;
                }
            }

            queue.Remove(Vertex);

            return Vertex;

        }

        /* Takes a graph as input an adjacency matrix (see top for details) and a starting node */
        public _Dijkstra(int[,] G, int s)
        {
            /* Check graph format and that the graph actually contains something */
            //if (G.GetLength(0) < 1 || G.GetLength(0) != G.GetLength(1))
            //{
            //    throw new ArgumentException("Graph error, wrong format or no nodes to compute");
            //}

            int len = G.GetLength(0);

            Initialize(s, len);

            while (queue.Count > 0)
            {
                int u = GetNextVertex();

                /* Find the nodes that u connects to and perform relax */
                for (int v = 0; v < len; v++)
                {
                    if (G[u,v] < 0)
                    {
                        throw new ArgumentException("Graph contains negative edge(s)");
                    }

                    if (G[u,v] > 0)
                    {
                        if (dist[v] > dist[u] + G[u, v])
                        {
                            dist[v] = dist[u] + G[u, v];

                            path[v] = u;
                            Console.WriteLine(dist[v]);
                            Console.WriteLine(path[v]);

                        }
                    }
                }
            }
        }

    }
}
