using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DualGraph;
using System;


public class MazeGeneration : MonoBehaviour {

    Node[,] m_Nodes;            //All nodes in the maze
    HashSet<QuadEdge> m_Edges;  //All edges in the maze
    Dnode[,] m_Dnodes;          //All dual nodes in the maze
    int m_size;                   //Length of dimension in m_Nodes
    Dnode m_root;               //Start of maze
    Dnode m_end;                //End of maze
    GameObject m_player;        //The ball
    GameObject m_maze;          //The whole maze

    void Start() {
        LoadGame();
    }
	
	void Update () {
        if(Input.GetKey(KeyCode.R)){ SceneManager.LoadScene("MazeGen"); }
	}

    /// <summary>
    /// Creates a new game
    /// </summary>
    void LoadGame(){    //As opposed to SceneManager.LoadScene()
        CreateGraphs();
        SetRoot();
        Traverse();
        SpawnPlayer();
        GenerateObjects();
    }

    /// <summary>
    /// Creates the initial graph and dual graph needed for generating a maze (via a pair of matrices)
    /// </summary>
    void CreateGraphs(){
        m_size = 7;               //Matrix size setting
        m_Nodes = new Node[m_size, m_size];  //Can replace all references to m_Nodes[]'s length by storing a variable instead.
        m_Dnodes = new Dnode[m_size - 1, m_size - 1];
        m_Edges = new HashSet<QuadEdge>();
        for (int i = 0; i < m_size; i++)
        {
            for (int j = 0; j < m_size; j++)
            {
                m_Nodes[i, j] = new Node();
                QuadEdge horz = new QuadEdge();         //Might need to be declared outside of loop - Temp. variable referencing the current quadedge between adjacent row nodes
                QuadEdge vert = new QuadEdge();
                if (j != 0)
                {           //Connect row nodes
                    horz = new QuadEdge(m_Nodes[i, j - 1], m_Nodes[i, j]);
                        if (i < m_size - 1) { m_Dnodes[i, j - 1] = new Dnode(); }   //Inserts all Dnodes into dual array - Should be implemented without the extra condition
                    horz.pos = new Vector3D(2 * j - 1, 0, 2 * i);             //Assigns the QuadEdge a position relative to the nodes
                    m_Edges.Add(horz);              
                }
                if (i != 0)
                { //Connect column nodes
                    vert = new QuadEdge(m_Nodes[i - 1, j], m_Nodes[i, j]); //Might need to be declared outside of loop - Temp. variable referencing the current quadedge between adjacent column nodes
                    vert.pos = new Vector3D(2 * j, 0, 2 * i - 1);
                    m_Edges.Add(vert);
                }
                if (i > 0 && j > 0)
                {
                    try
                    {
                        if(i < m_size - 1)
                        {
                            horz.SetDnodes(m_Dnodes[i - 1, j - 1], m_Dnodes[i, j - 1]); //Create connection between column dnodes
                            m_Dnodes[i - 1, j - 1].AddEdge(horz); 
                            m_Dnodes[i, j - 1].AddEdge(horz); 
                        }
                        if(j < m_size - 1 )
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

    /*Function to check that the maze is correctly implemented by 
     * iterating through m_Nodes and m_Dnodes and generating
     * generic gameobjects to represent the nodes. */
    void GraphTest(){   //Can be used to generate final maze
        for(int i = 0; i < m_size; i++){ //Iterating through m_Nodes first
            for(int j = 0; j < m_size; j++){
                if(m_Nodes[i,j] != null){
                    GameObject node = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    node.transform.position = new Vector3(j * 2, 0, i * 2);
                }
                if(i < (m_size-1) && j < (m_size-1) && m_Dnodes[i,j] != null){
                    GameObject dnode = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    dnode.transform.position = new Vector3(2 * i + 1f, 0, 2 * j + 1f);
                }
            }
        }
    }

    /// <summary>
    /// Goes through the modified graphs and instantiates objects based on the information the nodes carry.
    /// </summary>
    void GenerateObjects(){
        m_maze = new GameObject("MAZE");                //Instantiates the maze parent object
        m_maze.AddComponent<RotationScript>();          //and let's it function (rotate)
        m_maze.transform.position = new Vector3(m_size - 1, 0f, m_size - 1);        //Moves the maze to the centre of the objects
        for(int i = 0; i < m_size; i++){
            for(int j = 0; j < m_size; j++){
                if(m_Nodes[i,j] != null){   //Create walls from nodes and adds it the maze parent (as a child)
                    GameObject node = GameObject.CreatePrimitive(PrimitiveType.Cube);           //Alternatively: Can load a prefab and instantiate that instead.
                               node.GetComponent<Renderer>().material = Resources.Load("Materials/WallM") as Material;
                    Transform transNode = node.transform;
                              transNode.position = new Vector3(j * 2, 0, i * 2);
                              transNode.SetParent(m_maze.transform);
                }
                if(i < m_size - 1 && j < m_size - 1){   //Create start and end points and adds them the maze parent (as a child)
                    if(m_Dnodes[i,j] != null && m_Dnodes[i,j] == m_root){       //Create the starting plate and set its position
                        GameObject root = GameObject.CreatePrimitive(PrimitiveType.Plane);
                                   root.name = "Start Plate";
                                   root.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f);
                        Transform transRoot = root.GetComponent<Transform>();
                                  transRoot.position = new Vector3(j * 2 + 1, -0.49f, i * 2 + 1);
                                  transRoot.localScale = new Vector3(0.1f, 1, 0.1f);
                                  transRoot.SetParent(m_maze.transform);
                        m_player.transform.position = new Vector3(j * 2 + 1, 0.5f, i * 2 + 1);      //Sets the player's starting position
                    }
                    if (m_Dnodes[i, j] != null && m_Dnodes[i, j] == m_end){     //Create the goal plate and set its position
                        GameObject end = GameObject.CreatePrimitive(PrimitiveType.Plane);
                                   end.name = "Goal Plate";
                                   end.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f);
                        Transform transEnd = end.GetComponent<Transform>();
                                  transEnd.position = new Vector3(j * 2 + 1, -0.49f, i * 2 + 1);
                                  transEnd.localScale = new Vector3(0.1f, 1, 0.1f);
                                  transEnd.SetParent(m_maze.transform);
                        SetGoal(end);
                    }
                }
            }
        }
        foreach(QuadEdge q in m_Edges){ //Create walls from the edges and colours them
            if(q.Wall){
                GameObject edge = GameObject.CreatePrimitive(PrimitiveType.Cube);
                           edge.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/WallM");
                Transform transEdge = edge.transform;
                          transEdge.position = new Vector3(q.pos.x, q.pos.y, q.pos.z);
                          transEdge.SetParent(m_maze.transform);
            }
        }
        //Create the floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane); 
        Transform transFloor = floor.transform;
                  transFloor.position = new Vector3(m_size - 1, -0.5f, m_size - 1);
                  transFloor.localScale = new Vector3(0.2f * m_size - 0.1f, 1, 0.2f * m_size - 0.1f);
                  transFloor.SetParent(m_maze.transform);
    }

    /// <summary>
    /// Maze-generation algorithm that traverses the dual graph (graph of Dnodes) and creates the maze information required for instantiation
    /// </summary>
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
                m_end = currentNode;           //Assigns the end of the maze to the current every time until the end of the loop
            }
        }
    }
    
    /// <summary>
    /// Selects a Dnode on the edge of the maze to be the starting point of traversal for maze generation. Only works for array mazes.
    /// </summary>
    void SetRoot(){
        System.Random rng = new System.Random();
        int row = rng.Next(0, m_size-1);                      //Row index of root
        int col;                                            //Column index of root
        if (row == 0 || row == (m_size - 2))
        {
            col = rng.Next(0, m_size - 1);
        }
        else{                                               //if the row selected isn't the top or bottom row then the first or last column will be selected
            int[] opt = { 0, m_size - 2 };
            col = opt[rng.Next(0, 1)];
        }
        m_root = m_Dnodes[row, col];
    }

    /// <summary>
    /// Creates a new object and stores a trigger script for victory at the location of the goal. 
    /// </summary>
    /// <param name="end"></param>
    void SetGoal(GameObject end){
        Type[] comps = new Type[] {typeof(BoxCollider)};
        GameObject empty = new GameObject("Goal Trigger", comps);        //Alternatively: Use empty.AddComponent<Collider>()
        Collider col = empty.GetComponent<Collider>();
        col.isTrigger = true;
        empty.AddComponent<GoalTrigger>();                          //Attaches victory script
        empty.transform.position = end.transform.position;          //Moves the collider to the goal plate
        empty.transform.SetParent(end.transform);                   //Makes the goal plate the parent of the collider
    }

    /// <summary>
    /// Spawns the ball from prefabs
    /// </summary>
    public void SpawnPlayer(){
        GameObject ball = (GameObject)Resources.Load("Prefabs/Ball");
        m_player = GameObject.Instantiate(ball);
    }

    /// <summary>
    /// Function to check the number of neighbours of each Dnode during development
    /// </summary>
    void NeighbourCheck(){
        for(int i = 0; i < m_size-1; i++){
            for(int j = 0; j < m_size-1; j++){
                int count = m_Dnodes[i, j].QuadEdges.Count;
                Debug.Log("The [" + i + ", " + j + "] node has " + count + " friends.");
            }
        }
    }
}
