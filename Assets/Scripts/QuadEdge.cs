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

        //Constructor
        public QuadEdge() { }
        public QuadEdge(Node n1, Node n2){ m_node1 = n1; m_node2 = n2;}

        public void setDnodes(Dnode n1, Dnode n2){ m_dnode1 = n1; m_dnode2 = n2;}

        public Node GetNode1()   { return m_node1; }
        public Node GetNode2()   { return m_node2; }
        public Dnode GetDnode1() { return m_dnode1; }
        public Dnode GetDnode2() { return m_dnode2; }
    }

    /// <summary>
    /// Class to represent the nodes (vertices) in the graph.
    /// </summary>
    public class Node{ //TODO: Implement getter functions for quadedges and corresponding nodes
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

        public override string ToString(){
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
