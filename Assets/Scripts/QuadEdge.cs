using System.Collections;
using System.Collections.Generic;
using System;

namespace DualGraph
{
    /// <summary>
    /// Class to represent the edges in the graph and its dual used to represent the maze.
    /// </summary>
    public class QuadEdge
    {
        Node  m_node1;
        Node  m_node2;
        Dnode m_dnode1;
        Dnode m_dnode2;
        bool  m_wall = true;        //Not instantiating as true when implemented in the constructor for some reason.
        bool  m_visited;
        
        public Vector3D pos;

        public bool Visited
        {
            get
            {
                return m_visited;
            }

            set
            {
                m_visited = value;
            }
        }

        public bool Wall
        {
            get
            {
                return m_wall;
            }
        }

        public bool isWall(){
            return m_wall;
        }

        public QuadEdge() { 
            //this.Wall = true;
            m_visited = false;
        }
        public QuadEdge(Node n1, Node n2){ m_node1 = n1; m_node2 = n2;}

        public void SetDnodes(Dnode n1, Dnode n2){ m_dnode1 = n1; m_dnode2 = n2;}

        public Node GetNode1()   { return m_node1; }    //Necessary?
        public Node GetNode2()   { return m_node2; }    //Necessary?
        public Dnode GetDnode1() { return m_dnode1; }   //Necessary?
        public Dnode GetDnode2() { return m_dnode2; }   //Necessary?

        /// <summary>
        /// Sets the quadedge such that it is no longer a wall.
        /// </summary>
        public void TurnOffWall(){ m_wall = false; }

        /* public Node FarNode(Node from){
            if (from == m_node1) { return m_node2; }
            else{ return m_node1; }
        } */

        /// <summary>
        /// Returns the dual node attached to the quadedge that's not "from".
        /// </summary>
        /// <param name="from">The Dnode from which this function is called from.</param>
        /// <returns></returns>
        public Dnode FarDnode(Dnode from){
            if(from == m_dnode1){ return m_dnode2; }
            else{ return m_dnode1; }
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

        public Node From
        {
            get {   return m_from;    }
            set {   m_from = value;   }
        }

        public Node Next //Possibly not necessary
        {   get {   return m_next;    }
            set {   m_next = value;   }
        }

        public bool IsVisited()  { return m_visited;       }
        public void SetVisited() { m_visited = !m_visited; }

        /* public override string ToString(){
            return "";
        } */
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

        public Dnode() {
            m_visited = false;
            m_quadEdges = new HashSet<QuadEdge>();
        }
        
        public Dnode From
        {
            get { return m_from; }
            set { m_from = value; }
        }

        public Dnode Next //Possibly not necessary
        {
            get { return m_next; }
            set { m_next = value; }
        }

        public HashSet<QuadEdge> QuadEdges //Might need cleaning up
        {
            get
            {
                return m_quadEdges;
            }

            set
            {
                m_quadEdges = value;
            }
        }

        public void AddEdge(QuadEdge e){ m_quadEdges.Add(e); }

        public bool IsVisited()  { return m_visited;       }
        public void SetVisited() { m_visited = !m_visited; }

        /// <summary>
        /// Turns off the wall of Dnode and its m_from node.
        /// </summary>
        public void TurnOffWall(){
            if(m_from != null && m_quadEdges != null){
                foreach(QuadEdge q in m_quadEdges){
                    if(q.FarDnode(this) == m_from){ q.TurnOffWall(); }
                }
            }
        }

        public QuadEdge GetRandomEdge(){
            if(m_quadEdges != null){
                int size = m_quadEdges.Count;
                int rnd = new Random().Next(size);
                int i = 0;
                foreach (QuadEdge q in m_quadEdges) {
                    if (i == rnd) { return q; }
                    i++;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Struct to store three values. Not to be used as a replacement for Unity's Vector3 if using Unity.
    /// </summary>
    public struct Vector3D{

        public float x, y, z;

        public Vector3D(float x, float y, float z){
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static bool operator ==(Vector3D lhs, Vector3D rhs){
            /* if(
                 lhs.x == rhs.x &&
                 lhs.y == rhs.y &&
                 lhs.z == rhs.z
             ){ return true; }
             return false; */
            return lhs.Equals(rhs);
        }
        public static bool operator !=(Vector3D lhs, Vector3D rhs){
            /*if (
                lhs.x != rhs.x ||
                lhs.y != rhs.y ||
                lhs.z != rhs.z
            ) { return true; }
            return false; */
            return !lhs.Equals(rhs);
        }
    }
}
