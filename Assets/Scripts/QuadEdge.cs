using System.Collections;
using System.Collections.Generic;

namespace MazeGen
{
    /// <summary>
    /// Class to represent the edges in the graph and its dual used to represent the maze.
    /// </summary>
    public class QuadEdge
    {
        Node m_node1;
        Node m_node2;
        Dnode m_dnode1;
        Dnode m_dnode2;

        public QuadEdge(){
            //Depends on how the maze is going to be instantiated
        }
    }

    /// <summary>
    /// Class to represent the nodes (vertices) in the graph.
    /// </summary>
    public class Node{
        //var info;
        bool m_visited;
        Node m_from;
        Node m_next;
        HashSet<QuadEdge> m_quadEdges;

        public Node(){
            m_visited = false;
        }

        /// <summary>
        /// Returns the set of quadedges a node is attached to. 
        /// </summary>
        public HashSet<QuadEdge> Edges{ get; }

        public string ToString(){
            return "";
        }
    }

    /// <summary>
    /// Class to represent the nodes (vertices) in the dual graph.
    /// </summary>
    public class Dnode
    {
        bool m_visited;
        Dnode m_from;
        Dnode m_next;
        HashSet<QuadEdge> m_quadEdges;

        public Dnode(){
            m_visited = false;
        }
    }
}
