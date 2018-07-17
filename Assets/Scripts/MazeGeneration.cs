using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualGraph;
using System;

public class MazeGeneration : MonoBehaviour {

    Node[,] m_Nodes;            //All nodes in the maze
    HashSet<QuadEdge> m_Edges;  //All edges in the maze
    Dnode[,] m_Dnodes;          //All dual nodes in the maze
    int size;                   //Length of dimension in m_Nodes
    Dnode m_root;                 //Start of maze

    void Start() {
        CreateMaze();
        m_root = m_Dnodes[0, 0];
        Traverse();
        GenerateObjects();
    }
	
	void Update () {
		
	}

    void CreateMaze(){
        size = 5;               //Matrix size setting
        m_Nodes = new Node[size, size];  //Can replace all references to m_Nodes[]'s length by storing a variable instead.
        m_Dnodes = new Dnode[size - 1, size - 1];
        m_Edges = new HashSet<QuadEdge>();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                m_Nodes[i, j] = new Node();
                QuadEdge horz = new QuadEdge();         //Might need to be declared outside of loop - Temp. variable referencing the current quadedge between adjacent row nodes
                QuadEdge vert = new QuadEdge();
                if (j != 0)
                {           //Connect row nodes
                    horz = new QuadEdge(m_Nodes[i, j - 1], m_Nodes[i, j]);
                    if (i < size - 1) { m_Dnodes[i, j - 1] = new Dnode(); }   //Inserts all Dnodes into dual array - Should be implemented without the extra condition
                    horz.pos = new Vector3D(2 * j - 1, 0, 2 * i);             //Assigns the QuadEdge a position relative to the nodes
                    m_Edges.Add(horz);              
                    //GameObject edge = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    //edge.transform.position = new Vector3(2 * j - 1, 0, 2 * i);
                }
                if (i != 0)
                { //Connect column nodes
                    vert = new QuadEdge(m_Nodes[i - 1, j], m_Nodes[i, j]); //Might need to be declared outside of loop - Temp. variable referencing the current quadedge between adjacent column nodes
                    vert.pos = new Vector3D(2 * j, 0, 2 * i - 1);
                    m_Edges.Add(vert);
                    //GameObject edge = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    //edge.transform.position = new Vector3(2 * j, 0, 2 * i - 1);
                }
                if (i > 0 && j > 0)
                {
                    try
                    {
                        if(i < size - 1)
                        {
                            horz.SetDnodes(m_Dnodes[i - 1, j - 1], m_Dnodes[i, j - 1]); //Create connection between column dnodes
                            m_Dnodes[i - 1, j - 1].AddEdge(horz); 
                            m_Dnodes[i, j - 1].AddEdge(horz); 
                        }
                        if(j < size - 1 )
                        {
                            vert.SetDnodes(m_Dnodes[i - 1, j - 1], m_Dnodes[i - 1, j]); //Create connection between row dnodes
                            m_Dnodes[i - 1, j - 1].AddEdge(vert);
                            m_Dnodes[i - 1, j].AddEdge(vert);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.Log("Horz or Vert is null");
                        throw e;
                    }
                }
            }
        }
    }

    /*Method to check that the maze is correctly implemented by 
     * iterating through m_Nodes and m_Dnodes and generating
     * generic gameobjects to represent the nodes. */
    void GraphTest(){   //Can be used to generate final maze
        for(int i = 0; i < size; i++){ //Iterating through m_Nodes first
            for(int j = 0; j < size; j++){
                if(m_Nodes[i,j] != null){
                    GameObject node = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    node.transform.position = new Vector3(j * 2, 0, i * 2);
                }
                if(i < (size-1) && j < (size-1) && m_Dnodes[i,j] != null){
                    GameObject dnode = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    dnode.transform.position = new Vector3(2 * i + 1f, 0, 2 * j + 1f);
                }
            }
        }
    }

    void GenerateObjects(){
        for(int i = 0; i < size; i++){
            for(int j = 0; j < size; j++){
                if(m_Nodes[i,j] != null){   //Create walls from nodes
                    GameObject node = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    node.transform.position = new Vector3(j * 2, 0, i * 2);
                }
            }
        }
        foreach(QuadEdge q in m_Edges){ //Create walls from the edges
            if(q.Wall){
                GameObject edge = GameObject.CreatePrimitive(PrimitiveType.Cube);
                edge.transform.position = new Vector3(q.pos.x, q.pos.y, q.pos.z);
            }
        }
        //Create the floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.position = new Vector3(size - 1, -0.5f, size - 1);
        floor.transform.localScale = new Vector3(0.2f * size - 0.1f, 1, 0.2f * size - 0.1f);
        Material floorM = Resources.Load("/Assets/Materials/WallM.mat", typeof(Material)) as Material;
        floor.GetComponent<Renderer>().material = floorM;
    }

    void Traverse(){ //Starting with iterative implementation
        Stack<Dnode> stk = new Stack<Dnode>();
        stk.Push(m_root);
        while (stk.Count != 0) {
            Dnode currentNode = stk.Pop();
            if (!currentNode.IsVisited())
            {
                currentNode.SetVisited();
                if(currentNode.From != null){ currentNode.TurnOffWall(); }
                int size = currentNode.QuadEdges.Count;
                for(int i = 0; i < size; i++) 
                {
                    QuadEdge q = currentNode.GetRandomEdge();
                    if (!q.FarDnode(currentNode).IsVisited())
                    {
                        stk.Push(q.FarDnode(currentNode));           // Push neighbour onto stack
                        q.FarDnode(currentNode).From = currentNode;  // Set neighbour's m_from node to current node.
                    }
                }
            }
        }
    }

    void NeighbourCheck(){
        for(int i = 0; i < size-1; i++){
            for(int j = 0; j < size-1; j++){
                int count = m_Dnodes[i, j].QuadEdges.Count;
                Debug.Log("The [" + i + ", " + j + "] node has " + count + " friends.");
            }
        }
    }
}
