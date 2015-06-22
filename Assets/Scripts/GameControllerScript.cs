using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Node
{
	public Node()
	{
	}
	public Vector3 pos;
	public override bool Equals (object obj)
	{
		Node test = (Node)obj;
		bool value = false;
		if (pos.x == test.pos.x &&pos.y == test.pos.y &&pos.z == test.pos.z)
		{
			value = true;
		}
		return value;
	}
}
public class AstarHolder{

	//List<Node> emptySet;
	List<Node> openSet = new List<Node>();
	List<Node> closedSet = new List<Node>();
	Dictionary<Node,Node> cameFrom = new Dictionary<Node, Node>(); //new List<Node>();
	Dictionary<Node,float> gScore = new Dictionary<Node, float>();
	Dictionary<Node,float> fScore = new Dictionary<Node, float>();
	public Dictionary<Node,List<Node>> neighborNodes = new Dictionary<Node, List<Node>>();
	public AstarHolder()
	{
	}

	public bool Holds(List<Node> list,Node searchNode)
	{
		foreach(Node node in list)
		{
			if (node.Equals(searchNode))
				return true;
		}
		return false;
	}
	public bool Holds(Dictionary<Node,Node> list,Node searchNode)
	{
		foreach(Node node in list.Values)
		//foreach()
		{
			if (node.Equals(searchNode))
				return true;
		}
		return false;
	}
	public float Holds(Dictionary<Node,float> list,Node searchNode)
	{
		foreach(Node node in list.Keys)
		{
			if (node.Equals(searchNode))
				return list[node];
		}
		return 1000;
	}

	float heurCost(Node start, Node goal)
	{
		return Vector3.Distance(start.pos,goal.pos)/4;
	}
	public List<Node> astar(Node start,Node goal)
	{
		//(list)
		closedSet.Clear(); //the set of nodes already evauluated.
		openSet.Add(start); //the set of tentative nodes to be checked
		cameFrom.Clear(); // the map of navigated nodes
		
		gScore[start] = 0;
		fScore[start] = gScore[start] + heurCost(start,goal);
		int it = -1;
		while (openSet.Count > 0 && it < 10000)
		{
			it++;
			//current = //the node in the openset having the lowest fscore[] value
			Node current = new Node();
			foreach (Node newCurrent in cameFrom.Values)
			{
				current = newCurrent;
			}
			float lowestScore =1000;
			foreach (float fcheck in fScore.Values)
			{
				if (fcheck < lowestScore)
				{
					//lowestNode = fScore.;
					lowestScore = fcheck;
				}
			}
			foreach (Node testNode in fScore.Keys)
			{
				if (lowestScore == fScore[testNode] && !Holds(closedSet,testNode))
					current = testNode;
			}

			if (current == goal || Holds(cameFrom,goal) /*|| cameFrom.Count >= 2*/)
			{
				return redoPath(cameFrom,goal);
			}
			openSet.Remove(current);
			if (!Holds(closedSet,current))
				closedSet.Add(current);
			else
			{
				int bobbob = 5;
			}
			List<Node> tempList = new List<Node>();// = neighborNodes[current];

			foreach(Node finderNode in neighborNodes.Keys)
			{
				if (finderNode.Equals(current))
				{
					tempList = neighborNodes[finderNode];
				}
			}
			if (current.pos.x == 0 && current.pos.y == 0)
			{
				it = 10000;
				int bob = 5;
			}
			else
			{
				int bob = 5;
			}
			foreach(Node neighbor in tempList)
			{
				if (Holds(closedSet,neighbor))// closedSet.Contains(neighbor))
				{
					continue;
				}
				//float ngScore = gScore[current] + (Vector3.Distance(current.pos,neighbor.pos)/4);//distBetween(current,neighbor);
				float ngScore = Holds(gScore,current) + (Vector3.Distance(current.pos,neighbor.pos)/4);//distBetween(current,neighbor);
				
				if (!Holds(openSet,neighbor) || ngScore < gScore[neighbor])
				{
					cameFrom[neighbor] = current;
					gScore[neighbor] = ngScore;
					fScore[neighbor] = gScore[neighbor] + heurCost(neighbor,goal);
					//if (!openSet.Contains(neighbor))
					if (!Holds(openSet,neighbor))
						openSet.Add(neighbor);
				}
			}
		}
		return null;
	}

	public List<Node> redoPath(Dictionary<Node,Node> cameFrom,Node current)
	{
		List<Node> totalPath = new List<Node>(cameFrom.Values);
		//totalPath.Add(current);
		//List<Node> tempList = cameFrom;
		//while (cameFrom.ContainsKey(current))
		//{
		//	current = cameFrom[current];
		//	totalPath.Add(current);
		///}
		return totalPath;
	}
	/*public List<Node> redoPath(Dictionary<Node,Node> cameFrom,Node current)
	{
		List<Node> totalPath = new List<Node>();
		totalPath.Add(current);
		//List<Node> tempList = cameFrom;
		while (cameFrom.ContainsKey(current))
		{
			current = cameFrom[current];
			totalPath.Add(current);
		}
		return totalPath;
	}*/
}
[RequireComponent (typeof (MovementControl))]
public class GameControllerScript : MonoBehaviour {	
	public GameObject grass;
	public GameObject stone;
	public GameObject tree;
	public GameObject wall;

	public GameObject masterPlayer;
	public GameObject masterNpc;

	public GameObject blueSpawn;
	public GameObject redSpawn;

	public bool loadIt = true;
	public GameObject[] players;
	public GameObject[] npc;

	public AstarHolder astar = new AstarHolder();
	//public AStar tilesHolder = new Astar();
	//public AStarPathFinder pathing = new AStarPathFinder();
	public GameObject[,] tiles = new GameObject[9,6];
	//public GameObject[,] tiles;
	public int turn = 0;
	public int aiTurn = 0;
	public float npcTimeLoop = 0;
	public int maxTurn = 5;
	public int maxAITurn = 5;
//	private List<Node> nodePath = new List<Node>();

	void Start () {
		Spawn();
	}

	public void Spawn()
	{

		for(int i = 0; i != 9; i++)
		{
			//tiles[i] = new GameObject[6];
			for (int ii = 0; ii != 6; ii++)
			{
				Vector3 placement = new Vector3(0 + (i*4),0,0 + (ii*4));
				if (ii == 3 || i == 3)
				{
					tiles[i,ii] = (GameObject)Instantiate(stone,placement,Quaternion.identity);
				}
				else if (ii == 3)
				{
					tiles[i,ii] = (GameObject)Instantiate(wall,placement,Quaternion.identity);
					tiles[i,ii].name = "Wall";
				}
				else if (ii == 3 || i == 3)
				{
					tiles[i,ii] = (GameObject)Instantiate(tree,placement,Quaternion.identity);
				}
				else
				{
					tiles[i,ii] = (GameObject)Instantiate(grass,placement,Quaternion.identity);
				}
			}
		}
		for (int i = 0; i != 9; i++)
		{
			for (int ii = 0; ii != 6; ii++)
			{
				
				List<Node> tempList = new List<Node>();
				Node tempNode = new Node();
				if (i - 1 != -1)
				{
					tempNode = new Node();
					tempNode.pos.x = i-1;
					tempNode.pos.z = ii;
					tempList.Add(tempNode);
				}
				if (i + 1 != 10)
				{
					tempNode = new Node();
					tempNode.pos.x = i+1;
					tempNode.pos.z = ii;
					tempList.Add(tempNode);
				}
				if (ii - 1 != -1)
				{
					tempNode = new Node();
					tempNode.pos.x = i;
					tempNode.pos.z = ii - 1;
					tempList.Add(tempNode);
				}
				if (ii + 1 != 7)
				{
					tempNode = new Node();
					tempNode.pos.x = i;
					tempNode.pos.z = ii + 1;
					tempList.Add(tempNode);
				}
				tempNode.pos.x = i;
				tempNode.pos.z = ii;
				astar.neighborNodes.Add(tempNode,tempList);
			}
		}
		/*foreach(int tile in tiles)
		{
		}*/
		players = new GameObject[5];
		for (int i = 0; i != 5; i ++)
		{
			players[i] = (GameObject)Instantiate(masterPlayer, blueSpawn.transform.position, Quaternion.identity);
			players[i].GetComponent<MovementControl>().enabled = false;
			Transform[] tchildren = players[i].GetComponentsInChildren<Transform>();
			tchildren[1].camera.enabled = false;
			tchildren[2].SendMessage("changeNumber",i,SendMessageOptions.RequireReceiver);
		}
		npc = new GameObject[5];
		for (int i = 0; i != 5; i ++)
		{
			Vector3 newRedSpawn = redSpawn.transform.position;
			newRedSpawn.y += (4*i);
			npc[i] = (GameObject)Instantiate(masterNpc, newRedSpawn, Quaternion.identity);
			//npc[i].GetComponent<MovementControl>().enabled = false;
			//Transform[] tchildren = npc[i].GetComponentsInChildren<Transform>();
			//tchildren[1].camera.enabled = false;
			//tchildren[2].SendMessage("changeNumber",i,SendMessageOptions.RequireReceiver);
		}
		Turn();
	}
	void Turn()
	{
		int loops = 0;
		foreach(GameObject player in players)
		{
			if (loops == turn)
			{
				player.GetComponent<MovementControl>().enabled = true;
				Transform[] tchildren = player.GetComponentsInChildren<Transform>();
				tchildren[1].camera.enabled = true;
				return;
			}
			loops++;
		}
	}
	void TurnNPC()
	{
		int loops = 0;
		foreach(GameObject player in npc)
		{
			if (loops == turn - maxAITurn)
			{
				player.GetComponent<AIControl>().enabled = true;
				player.GetComponent<AIControl>().step = 0;
				List<Vector3> newList = new List<Vector3>();
				
				//PathFinding newPath = new PathFinding();
				//newPath.FindPath(player.transform.position/4,players[0].transform.position/4); 
				Node start = new Node();
				start.pos.x = player.transform.position.x/4;
				start.pos.z = player.transform.position.z/4;

				Node end = new Node();
				end.pos.x = players[0].transform.position.x/4;
				end.pos.z = players[0].transform.position.z/4;

				List<Node> tempList = astar.astar(start,end);
				Vector3 tempPos = Vector3.zero;
				tempPos.y = player.transform.position.y;
				//Node tempCell = new Node();// = new SearchCell();
				int maxTurns = 0;
				foreach (Node tempCell in tempList)
				{
					//tempCell = tempList[0];
					tempPos.x = tempCell.pos.x * 4;
					tempPos.z = tempCell.pos.z * 4;
					if (tempPos != player.transform.position)
						newList.Add(tempPos);
					maxTurns++;

				}
				/*tempCell = tempList[0];
				tempPos.x = tempCell.pos.x * 4;
				tempPos.z = tempCell.pos.z * 4;
				newList.Add(tempPos);
				tempCell = tempList[1];
				tempPos.x = tempCell.pos.x * 4;
				tempPos.z = tempCell.pos.z * 4;
				newList.Add(tempPos);
				tempCell = tempList[2];
				tempPos.x = tempCell.pos.x * 4;
				tempPos.z = tempCell.pos.z * 4;
				newList.Add(tempPos);
				tempCell = tempList[3];
				tempPos.x = tempCell.pos.x * 4;
				tempPos.z = tempCell.pos.z * 4;
				newList.Add(tempPos);
				tempCell = tempList[4];
				tempPos.x = tempCell.pos.x * 4;
				tempPos.z = tempCell.pos.z * 4;
				newList.Add(tempPos);
				tempCell = tempList[5];
				tempPos.x = tempCell.pos.x * 4;
				tempPos.z = tempCell.pos.z * 4;*/
				//newList.Add(player.transform.position + new Vector3(4,0,0));
	            //newList.Add(player.transform.position + new Vector3(8,0,0));
	            //newList.Add(player.transform.position + new Vector3(12,0,0));
	            //newList.Add(player.transform.position + new Vector3(16,0,0));
	            //newList.Add(player.transform.position + new Vector3(20,0,0));
				player.GetComponent<AIControl>().walkOrder = newList;
				player.GetComponent<AIControl>().checkWalk.Clear();
				player.GetComponent<AIControl>().checkWalk.Add(0);
				player.GetComponent<AIControl>().checkWalk.Add(0);
				player.GetComponent<AIControl>().checkWalk.Add(0);
				player.GetComponent<AIControl>().checkWalk.Add(0);
				player.GetComponent<AIControl>().checkWalk.Add(0);
				//Transform[] tchildren = player.GetComponentsInChildren<Transform>();
				//tchildren[1].camera.enabled = true;
				return;
			}
			loops++;
		}
	}
	void unTurn()
	{
		int loops = 0;
		foreach(GameObject player in players)
		{
			if (loops == turn)
			{
				
				player.GetComponent<MovementControl>().enabled = false;
				player.GetComponent<MovementControl>().currentSteps = 0;
				Transform[] tchildren = player.GetComponentsInChildren<Transform>();
				tchildren[1].camera.enabled = false;
				return;
			}
			loops++;
		}
	}
	
	void unTurnNPC()
	{
		int loops = 0;
		foreach(GameObject player in npc)
		{
			if (loops == turn - maxTurn)
			{
				//player.GetComponent<MovementControl>().enabled = false;
				player.GetComponent<AIControl>().currentSteps = 0;
				player.GetComponent<AIControl>().enabled = false;
				List<Vector3> newList = new List<Vector3>();
				newList.Add(player.transform.position);
				newList.Add(player.transform.position);
				newList.Add(player.transform.position);
				newList.Add(player.transform.position);
				newList.Add(player.transform.position);
				
				player.GetComponent<AIControl>().walkOrder = newList;
				player.GetComponent<AIControl>().checkWalk.Clear();
				player.GetComponent<AIControl>().checkWalk.Add(0);
				player.GetComponent<AIControl>().checkWalk.Add(0);
				player.GetComponent<AIControl>().checkWalk.Add(0);
				player.GetComponent<AIControl>().checkWalk.Add(0);
				player.GetComponent<AIControl>().checkWalk.Add(0);
				//Transform[] tchildren = player.GetComponentsInChildren<Transform>();
				//tchildren[1].camera.enabled = false;
				return;
			}
			loops++;
		}
	}

	void Update()
	{
		int loops = 0;
		if (turn >= maxTurn && turn < maxAITurn + maxTurn)
		{
			foreach(GameObject ai in npc)
			{
				if (loops + maxTurn == turn)
				{
					if (ai.GetComponent<AIControl>().currentSteps >=
					    ai.GetComponent<AIControl>().maxSteps)
					{
						unTurnNPC();
						turn++;

						TurnNPC();
						
						break;
					}
				}
				loops++;
			}
		}
		/*
		if (turn >= maxTurn && npc[aiTurn] != null && aiTurn <= maxAITurn)
		{
 			if (npc[aiTurn].GetComponent<AIControl>().currentSteps <=
			        npc[aiTurn].GetComponent<AIControl>().maxSteps)
			    {
				//Vector2 targetPos = 
				Node targetPos = UpdateNPC(npc[aiTurn])[0];
				npc[aiTurn].GetComponent<AIControl>().targetPos = new Vector3(targetPos.x,0,targetPos.y);
			}
				//UpdateNPC(npc[aiTurn]);
				
		}
		else if (turn >= maxTurn)
			turn = 0;
		else*/
		else if (turn >= maxTurn)
		{
			turn = 0;
			Turn();
		}
		else
		foreach(GameObject player in players)
		{
			if (loops == turn)
			{
				if (player.GetComponent<MovementControl>().currentSteps >=
				    player.GetComponent<MovementControl>().maxSteps)
				{
					unTurn();
					turn++;
					if (turn == maxTurn)
					{
						TurnNPC();
						//turn++;
						//bool aiTurn = true;
					}
					Turn();

					break;
				}
			}
			loops++;
		}

	}
	/*List<Node> UpdateNPC(GameObject npc)
	{
		Vector2 targetPoint = Vector3.zero;
		Vector2 thisPoint = Vector2.zero;
		Vector3 shortestDistance = new Vector3(100,100,100);
		foreach(GameObject player in players)
		{
			if (Vector3.Distance(player.transform.position,npc.transform.position) 
			    < Vector3.Distance(shortestDistance,npc.transform.position))
			{
				shortestDistance = player.transform.position;
			}
		}
		
		thisPoint.x = npc.transform.position.x/4;
		thisPoint.y = npc.transform.position.z/4;
		targetPoint.x = shortestDistance.x/4;
		targetPoint.y = shortestDistance.z/4;

		return tilesHolder.FindPath(thisPoint,targetPoint);
		*/



		/*while (true)
		{
			bool madeProgress = false;
			int distance = 0;
			
			// Look at each square on the board.
			foreach (GameObject tile in tiles)
			{
				int x = tile.transform.position.x/4;
				int z = tile.transform.position.z/4;
				
				// If the square is open, look through valid moves given
				// the coordinates of that square.
				if (tiles[x][z].name != "Wall")
				{
					int passHere = distance;
					
					foreach (Point movePoint in ValidMoves(x, y))
					{
						int newX = movePoint.X;
						int newY = movePoint.Y;
						int newPass = passHere + 1;
						
						if (_squares[newX, newY].DistanceSteps > newPass)
						{
							_squares[newX, newY].DistanceSteps = newPass;
							madeProgress = true;
						}
					}
				}
			}
			if (!madeProgress)
			{
				break;
			}
		}
	//}
	*/









	//}
}
//[RequireComponent (typeof (Astar))]
/*public class Mover{}
public class Node {
	public int x;
	public int y;
	public float cost;
	public Node parent;
	public float heuristic;
	public int depth;
	public bool blocked = false;
	
	public Node(int x, int y) {
		this.x = x;
		this.y = y;
	}
	public int setParent(Node parent) {
		depth = parent.depth + 1;
		this.parent = parent;
		
		return depth;
	}
	public int compareTo(Node other) {
		Node o = (Node) other;
		
		float f = heuristic + cost;
		float of = o.heuristic + o.cost;
		
		if (f < of) {
			return -1;
		} else if (f > of) {
			return 1;
		} else {
			return 0;
		}
	}
}
public class Step {
	public int x;
	public int y;

	public Step(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public int getX() {
		return x;
	}

	public int getY() {
		return y;
	}

	public int hashCode() {
		return x*y;
	}

	/*public bool equals(Object other) {
		if (other instanceof Step) {
			Step o = (Step) other;
			
			return (o.x == x) && (o.y == y);
		}
		
		return false;
	}*/
//}
/*public class Path {
	private List<Step> steps = new List<Step>();

	public Path() {
		
	}
	public int getLength() {
		return steps.Count;
	}

	public Step getStep(int index) {
		return (Step) steps[index];
	}
	public int getX(int index) {
		return getStep(index).x;
	}
	public int getY(int index) {
		return getStep(index).y;
	}
	public void appendStep(int x, int y) {
		steps.Add(new Step(x,y));
	}
	public void prependStep(int x, int y) {
		steps.Insert(0, new Step(x, y));
	}
	public bool contains(int x, int y) {
		Step finderStep = new Step(x,y);
		return steps.Contains(finderStep);
	}


}
public class TileBasedMap {
	public int getWidthInTiles(){ return 6;}
	public int getHeightInTiles(){return 9;}	
	public float getCost(Mover mover, int sx, int sy, int tx, int ty){ return 1;}
	public void pathFinderVisited(int x, int y){}
	public Path findPath(Mover mover, int sx, int sy, int tx, int ty){ return new Path();}
}
public class AStarPathFinder {
	private List<Node> closed = new List<Node>();
	private SortedList open = new SortedList();
	private TileBasedMap map;
	private int maxSearchDistance;
	private Node[][] nodes;
	private bool allowDiagMovement;
	//private AStarHeuristic heuristic;

	public AStarPathFinder(TileBasedMap map, int maxSearchDistance, bool allowDiagMovement) {
		//this(map, maxSearchDistance, allowDiagMovement);
		this.map = map;
		this.maxSearchDistance = maxSearchDistance;
		this.allowDiagMovement = allowDiagMovement;
	}

	public AStarPathFinder(TileBasedMap map, int maxSearchDistance, 
	                       bool allowDiagMovement, int heuristic) {
		//this.heuristic = 1;
		this.map = map;
		this.maxSearchDistance = maxSearchDistance;
		this.allowDiagMovement = allowDiagMovement;
		
		nodes = new Node[9][];//[7];
		for (int x=0;x<map.getWidthInTiles();x++) {
			nodes[x] = new Node[6];
			for (int y=0;y<map.getHeightInTiles();y++) {
				nodes[x][y] = new Node(x,y);
			}
		}
	}

	public Path findPath(Mover mover, int sx, int sy, int tx, int ty) {
		// easy first check, if the destination is blocked, we can't get there
		//if (map.blocked(mover, tx, ty)) {
		//	return null;
		//}
		if (nodes[tx][ty].blocked)
		{
			return null;
		}
		
		// initial state for A*. The closed group is empty. Only the starting
		// tile is in the open list and it's cost is zero, i.e. we're already there
		nodes[sx][sy].cost = 0;
		nodes[sx][sy].depth = 0;
		closed.Clear();
		open.clear();
		open.add(nodes[sx][sy]);
		
		nodes[tx][ty].parent = null;
		
		// while we haven't found the goal and haven't exceeded our max search depth
		int maxDepth = 0;
		while ((maxDepth < maxSearchDistance) && (open.size() != 0)) {
			// pull out the first node in our open list, this is determined to 
			// be the most likely to be the next step based on our heuristic
			Node current = getFirstInOpen();
			if (current == nodes[tx][ty]) {
				break;
			}
			
			removeFromOpen(current);
			addToClosed(current);
			
			// search through all the neighbours of the current node evaluating
			// them as next steps
			for (int x=-1;x<2;x++) {
				for (int y=-1;y<2;y++) {
					// not a neighbour, its the current tile
					if ((x == 0) && (y == 0)) {
						continue;
					}
					// if we're not allowing diaganol movement then only 
					// one of x or y can be set
					if (!allowDiagMovement) {
						if ((x != 0) && (y != 0)) {
							continue;
						}
					}
					// determine the location of the neighbour and evaluate it
					int xp = x + current.x;
					int yp = y + current.y;
					
					if (isValidLocation(mover,sx,sy,xp,yp)) {
						// the cost to get to this node is cost the current plus the movement
						// cost to reach this node. Note that the heursitic value is only used
						// in the sorted open list
						float nextStepCost = current.cost + getMovementCost(mover, current.x, current.y, xp, yp);
						Node neighbour = nodes[xp][yp];
						map.pathFinderVisited(xp, yp);
						
						// if the new cost we've determined for this node is lower than 
						// it has been previously makes sure the node hasn't been discarded. We've
						// determined that there might have been a better path to get to
						// this node so it needs to be re-evaluated
						if (nextStepCost < neighbour.cost) {
							if (inOpenList(neighbour)) {
								removeFromOpen(neighbour);
							}
							if (inClosedList(neighbour)) {
								removeFromClosed(neighbour);
							}
						}
						
						// if the node hasn't already been processed and discarded then
						// reset it's cost to our current cost and add it as a next possible
						// step (i.e. to the open list)
						if (!inOpenList(neighbour) && !(inClosedList(neighbour))) {
							neighbour.cost = nextStepCost;
							neighbour.heuristic = getHeuristicCost(mover, xp, yp, tx, ty);
							maxDepth = Mathf.Max(maxDepth, neighbour.setParent(current));
							addToOpen(neighbour);
						}
					}
				}
			}
		}
		
		// since we've got an empty open list or we've run out of search 
		// there was no path. Just return null
		if (nodes[tx][ty].parent == null) {
			return null;
		}
		
		// At this point we've definitely found a path so we can uses the parent
		// references of the nodes to find out way from the target location back
		// to the start recording the nodes on the way.
		Path path = new Path();
		Node target = nodes[tx][ty];
		while (target != nodes[sx][sy]) {
			path.prependStep(target.x, target.y);
			target = target.parent;
		}
		path.prependStep(sx,sy);
		
		// thats it, we have our path 
		return path;
	}
	protected Node getFirstInOpen() {
		return (Node) open.first();
	}
	protected void addToOpen(Node node) {
		open.add(node);
	}

	protected bool inOpenList(Node node) {
		return open.contains(node);
	}
	protected void removeFromOpen(Node node) {
		open.remove(node);
	}

	protected void addToClosed(Node node) {
		closed.Add(node);
	}

	protected bool inClosedList(Node node) {
		return closed.Contains(node);
	}
	protected void removeFromClosed(Node node) {
		closed.Remove(node);
	}

	protected bool isValidLocation(Mover mover, int sx, int sy, int x, int y) {
		bool invalid = (x < 0) || (y < 0) || (x >= map.getWidthInTiles()) || (y >= map.getHeightInTiles());
		
		if ((!invalid) && ((sx != x) || (sy != y))) {
			invalid = nodes[x][y].blocked;
		}
		
		return !invalid;
	}

	public float getMovementCost(Mover mover, int sx, int sy, int tx, int ty) {
		return map.getCost(mover, sx, sy, tx, ty);
	}

	public float getHeuristicCost(Mover mover, int x, int y, int tx, int ty) {
		return heuristic.getCost(map, mover, x, y, tx, ty);
	}

	private class SortedList {
		private List<Node> list = new List<Node>();
		public Node first() {
			return list.get(0);
		}

		public void clear() {
			list.Clear();
		}

		public void add(Node o) {
			list.Add(o);
			list.Sort();
		}

		public void remove(Node o) {
			list.Remove(o);
		}

		public int size() {
			return list.Count();
		}

		public bool contains(Node o) {
			return list.Contains(o);
		}
	}
}
/*
/*public class AStar{
	
	public List<Node> closed = new List<Node> ();
	public List<Node> open = new List<Node> ();
	public TileBasedMap map;
	private int maxSearchDistance;
	private Node[][] nodes;
	private AStarHeuristic heuristic;
	public float getCost(TileBasedMap map, Mover mover, int x, int y, int tx, int ty){}

	public void init(){
		if (map.blocked(mover, tx, ty)) {
			return null;
		}
		
		// initial state for A*. The closed group is empty. Only the starting
		// tile is in the open list and it's cost is zero, i.e. we're already there
		nodes[sx][sy].cost = 0;
		nodes[sx][sy].depth = 0;
		closed.clear();
		open.clear();
		open.add(nodes[sx][sy]);
		
		// set the parent of the target location to null to mark that 
		// we haven't found a route yet
		nodes[tx][ty].parent = null;
	}
}*/
/*
struct MapHolder{
	public int[,] Map;
	public void setMap() {
		int[,] newMap = {
			{ 1,-1, 1, 1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 1, 1, 1 },
			{ 1,-1, 1,-1, 1,-1, 1, 2, 1, 1 },
			{ 1, 1, 1,-1, 1, 1, 2, 3, 2, 1 }
		};
		Map = newMap;
	}
	public int getMap(int x,int y)
	{
		if((x < 0) || (x > 9))
			return(-1);
		if((y < 0) || (y > 9))
			return(-1);
		return(Map[y,x]);
	}
}
public class AStarNode2D : AStarNode
{
	public int X 
	{
		get 
		{
			return FX;
		}
	}
	private int FX;

	public int Y
	{
		get
		{
			return FY;
		}
	}
	private int FY;

	public AStarNode2D(AStarNode AParent,AStarNode AGoalNode,double ACost,int AX, int AY) : base(AParent,AGoalNode,ACost)
	{
		FX = AX;
		FY = AY;
	}

	private void AddSuccessor(ArrayList ASuccessors,int AX,int AY) 
	{
		//int CurrentCost = mapGetMap(AX,AY);
		MapHolder tempMap = new MapHolder();
		tempMap.setMap();
		int CurrentCost = tempMap.getMap(AX,AY);
		if(CurrentCost == -1) 
		{
			return;
		}
		AStarNode2D NewNode = new AStarNode2D(this,GoalNode,Cost + CurrentCost,AX,AY);
		if(NewNode.IsSameState(Parent)) 
		{
			return;
		}
		ASuccessors.Add(NewNode);
	}

	public override bool IsSameState(AStarNode ANode)
	{
		if(ANode == null) 
		{
			return false;
		}
		return ((((AStarNode2D)ANode).X == FX) &&
		        (((AStarNode2D)ANode).Y == FY));
	}

	public override void Calculate()
	{
		if(GoalNode != null) 
		{
			float xd = FX - ((AStarNode2D)GoalNode).X;
			float yd = FY - ((AStarNode2D)GoalNode).Y;
			GoalEstimate = Mathf.Max(Mathf.Abs(xd),Mathf.Abs(yd));
		}
		else
		{
			GoalEstimate = 0;
		}
	}

	public override void GetSuccessors(ArrayList ASuccessors)
	{
		ASuccessors.Clear();
		AddSuccessor(ASuccessors,FX-1,FY  );
		AddSuccessor(ASuccessors,FX-1,FY-1);
		AddSuccessor(ASuccessors,FX  ,FY-1);
		AddSuccessor(ASuccessors,FX+1,FY-1);
		AddSuccessor(ASuccessors,FX+1,FY  );
		AddSuccessor(ASuccessors,FX+1,FY+1);
		AddSuccessor(ASuccessors,FX  ,FY+1);
		AddSuccessor(ASuccessors,FX-1,FY+1);
	}	

	public override void PrintNodeInfo()
	{
		//WriteLine("X:\t{0}\tY:\t{1}\tCost:\t{2}\tEst:\t{3}\tTotal:\t{4}",FX,FY,Cost,GoalEstimate,TotalCost);
	}
}
public class Heap
{
	
	private ArrayList FList;
	private IComparer FComparer = null;
	private bool FUseObjectsComparison;

	public Heap()
	{ 
		InitProperties(null, 0); 
	}

	public Heap(int Capacity)
	{ 
		InitProperties(null, Capacity); 
	}

	public Heap(IComparer Comparer)
	{ 
		InitProperties(Comparer, 0); 
	}

	public Heap(IComparer Comparer, int Capacity)
	{ 
		InitProperties(Comparer, Capacity); 
	}

	public bool AddDuplicates 
	{ 
		set 
		{ 
			FAddDuplicates = value; 
		} 
		get 
		{ 
			return FAddDuplicates; 
		} 
	}
	private bool FAddDuplicates;
	

	public int Capacity 
	{ 
		get 
		{
			return FList.Capacity; 
		} 
		set 
		{ 
			FList.Capacity = value; 
		} 
	}
	
	public object this[int Index]
	{
		get
		{
			//if(Index >= FList.Count || Index < 0) 
				//throw new ArgumentOutOfRangeException("Index is less than zero or Index is greater than Count.");
			return FList[Index];
		}
		set
		{
			//throw new InvalidOperationException("[] operator cannot be used to set a value in a Heap.");
		}
	}

	public int Add(object O)
	{
		int Return = -1;
		if (ObjectIsCompliant(O))
		{
			int Index = IndexOf(O);
			int NewIndex = Index>=0 ? Index : -Index-1;
			if (NewIndex>=Count) FList.Add(O);
			else FList.Insert(NewIndex, O);
			Return = NewIndex;
		}
		return Return;
	}

	public bool Contains(object O)
	{
		return FList.BinarySearch(O, FComparer)>=0;
	}

	public int IndexOf(object O)
	{
		int Result = -1;
		Result = FList.BinarySearch(O, FComparer);
		while(Result>0 && FList[Result-1].Equals(O))
			Result--;
		return Result;
	}

	public bool IsFixedSize 
	{ 
		get 
		{ 
			return FList.IsFixedSize ; 
		} 
	}

	public bool IsReadOnly 
	{ 
		get 
		{ 
			return FList.IsReadOnly; 
		} 
	}

	public void Clear() 
	{ 
		FList.Clear(); 
	}

	public void Insert(int Index, object O)
	{
		//throw new InvalidOperationException("Insert method cannot be called on a Heap.");
	}

	public void Remove(object Value) 
	{ 
		FList.Remove(Value); 
	}

	public void RemoveAt(int Index) 
	{ 
		FList.RemoveAt(Index); 
	}

	//public void CopyTo(Array array, int arrayIndex) { FList.CopyTo(array, arrayIndex); }

	public int Count { get { return FList.Count; } }

	public bool IsSynchronized { get { return FList.IsSynchronized; } }

	public object SyncRoot { get { return FList.SyncRoot; } }

	public IEnumerator GetEnumerator()
	{ 
		return FList.GetEnumerator(); 
	}

	public object Clone()
	{
		Heap Clone = new Heap(FComparer, FList.Capacity);
		Clone.FList = (ArrayList)FList.Clone();
		Clone.FAddDuplicates = FAddDuplicates;
		return Clone;
	}

	public delegate bool Equality(object Object1, object Object2);

	public override string ToString()
	{
		string OutString = "{";
		for (int i=0; i<FList.Count; i++)
			OutString += FList[i].ToString() + (i != FList.Count-1 ? "; " : "}");
		return OutString;
	}

	public override bool Equals(object Object)
	{
		Heap SL = (Heap)Object;
		if ( SL.Count!=Count ) 
			return false;
		for (int i=0; i<Count; i++)
			if ( !SL[i].Equals(this[i]) ) 
				return false;
		return true;
	}

	public override int GetHashCode() 
	{ 
		return FList.GetHashCode(); 
	}
	

	public int IndexOf(object Object, int Start)
	{
		int Result = -1;
		Result = FList.BinarySearch(Start, FList.Count-Start, Object, FComparer);
		while(Result > Start && FList[Result-1].Equals(Object))
			Result--;
		return Result;
	}

	public int IndexOf(object Object, Equality AreEqual)
	{
		for (int i=0; i<FList.Count; i++)
			if ( AreEqual(FList[i], Object) ) return i;
		return -1;
	}

	public int IndexOf(object Object, int Start, Equality AreEqual)
	{
		//if ( Start<0 || Start>=FList.Count ) throw new ArgumentException("Start index must belong to [0; Count-1].");
		for (int i=Start; i<FList.Count; i++)
			if ( AreEqual(FList[i], Object) ) return i;
		return -1;
	}

	public void AddRange(ICollection C)
	{
		foreach(object Object in C) 
			Add(Object);
	}

	public void InsertRange(int Index, ICollection C)
	{
		//throw new InvalidOperationException("Insert cannot be called on a Heap.");
	}

	public void LimitOccurrences(object Value, int NumberToKeep)
	{
		//if(Value == null) 
			//throw new ArgumentNullException("Value");
		int Pos = 0;
		while((Pos = IndexOf(Value, Pos)) >= 0)
		{
			if(NumberToKeep <= 0)
				FList.RemoveAt(Pos);
			else
			{
				Pos++; 
				NumberToKeep--; 
			}
			if(FComparer.Compare(FList[Pos],Value) > 0 ) 
				break; 
		}
	}

	public void RemoveDuplicates()
	{
		int PosIt;
		PosIt = 0;
		while(PosIt < Count-1)
		{
			if(FComparer.Compare(this[PosIt],this[PosIt+1]) == 0 ) 
				RemoveAt(PosIt);
			else 
				PosIt++;
		}
	}

	public int IndexOfMin()
	{
		int RetInt = -1;
		if (FList.Count > 0)
		{
			RetInt = 0;
			object RetObj = FList[0];
		}
		return RetInt;
	}

	public int IndexOfMax()
	{
		int RetInt = -1;
		if(FList.Count > 0)
		{
			RetInt = FList.Count-1;
			object RetObj = FList[FList.Count-1];
		}
		return RetInt;
	}

	public object Pop()
	{
		//if(FList.Count == 0)
			//throw new InvalidOperationException("The heap is empty.");
		object Object = FList[Count-1];
		FList.RemoveAt(Count-1);
		return(Object);
	}

	public int Push(object Object)
	{
		return(Add(Object));
	}

	private bool ObjectIsCompliant(object Object)
	{
		//if(FUseObjectsComparison && !(Object is IComparable)) 
			//throw new ArgumentException("The Heap is set to use the IComparable interface of objects, and the object to add does not implement the IComparable interface.");
		if(!FAddDuplicates && Contains(Object)) 
			return false;
		return true;
	}
	
	private class Comparison
	{
		public int Compare(object Object1, object Object2)
		{
			IComparable C = Object1 as IComparable;
			return C.CompareTo(Object2);
		}
	}
	
	private void InitProperties(IComparer Comparer, int Capacity)
	{
		if(Comparer != null)
		{
			FComparer = Comparer;
			FUseObjectsComparison = false;
		}
		else
		{
			//FComparer = new Comparison();
			FUseObjectsComparison = true;
		}
		FList = Capacity > 0 ? new ArrayList(Capacity) : new ArrayList();
		FAddDuplicates = true;
	}
}
public class AStarNode
{
	
	private AStarNode FParent;
	public AStarNode Parent
	{
		get
		{
			return FParent;
		}
		set
		{
			FParent = value;
		}
	}

	public double Cost 
	{
		set
		{
			FCost = value;
		}
		get
		{
			return FCost;
		}
	}
	private double FCost;

	public double GoalEstimate 
	{
		set
		{
			FGoalEstimate = value;
		}
		get 
		{
			Calculate();
			return(FGoalEstimate);
		}
	}
	private double FGoalEstimate;

	public double TotalCost
	{
		get 
		{
			return(Cost + GoalEstimate);
		}
	}

	public AStarNode GoalNode 
	{
		set 
		{
			FGoalNode = value;
			Calculate();
		}
		get
		{
			return FGoalNode;
		}
	}
	private AStarNode FGoalNode;

	public AStarNode(AStarNode AParent,AStarNode AGoalNode,double ACost)
	{
		FParent = AParent;
		FCost = ACost;
		GoalNode = AGoalNode;
	}
	public bool IsGoal()
	{
		return IsSameState(FGoalNode);
	}
	public virtual bool IsSameState(AStarNode ANode)
	{
		return false;
	}

	public virtual void Calculate()
	{
		FGoalEstimate = 0.0f;
	}

	public virtual void GetSuccessors(ArrayList ASuccessors)
	{
	}

	public virtual void PrintNodeInfo()
	{
	}
	
	public override bool Equals(object obj)
	{
		return IsSameState((AStarNode)obj);
	}
	
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
	
	public int CompareTo(object obj)
	{
		return(-TotalCost.CompareTo(((AStarNode)obj).TotalCost));
	}

}


public class AStar
{
	
	private AStarNode FStartNode;
	private AStarNode FGoalNode;
	private Heap FOpenList;
	private Heap FClosedList;
	private ArrayList FSuccessors;

	public ArrayList Solution
	{
		get 
		{
			return FSolution;
		}
	}
	private ArrayList FSolution;
	
	public AStar()
	{
		FOpenList = new Heap();
		FClosedList = new Heap();
		FSuccessors = new ArrayList();
		FSolution = new ArrayList();
	}

	private void PrintNodeList(object ANodeList)
	{
		Console.WriteLine("Node list:");
		foreach(AStarNode n in (ANodeList as IEnumerable)) 
		{
			n.PrintNodeInfo();
		}
		Console.WriteLine("=====");
	}

	public void FindPath(AStarNode AStartNode,AStarNode AGoalNode)
	{
		FStartNode = AStartNode;
		FGoalNode = AGoalNode;
		
		FOpenList.Add(FStartNode);
		while(FOpenList.Count > 0) 
		{
			// Get the node with the lowest TotalCost
			AStarNode NodeCurrent = (AStarNode)FOpenList.Pop();
			
			// If the node is the goal copy the path to the solution array
			if(NodeCurrent.IsGoal()) {
				while(NodeCurrent != null) {
					FSolution.Insert(0,NodeCurrent);
					NodeCurrent = NodeCurrent.Parent;
				}
				break;					
			}
			
			// Get successors to the current node
			NodeCurrent.GetSuccessors(FSuccessors);
			foreach(AStarNode NodeSuccessor in FSuccessors) 
			{
				// Test if the currect successor node is on the open list, if it is and
				// the TotalCost is higher, we will throw away the current successor.
				AStarNode NodeOpen = null;
				if(FOpenList.Contains(NodeSuccessor))
					NodeOpen = (AStarNode)FOpenList[FOpenList.IndexOf(NodeSuccessor)];
				if((NodeOpen != null) && (NodeSuccessor.TotalCost > NodeOpen.TotalCost)) 
					continue;
				
				// Test if the currect successor node is on the closed list, if it is and
				// the TotalCost is higher, we will throw away the current successor.
				AStarNode NodeClosed = null;
				if(FClosedList.Contains(NodeSuccessor))
					NodeClosed = (AStarNode)FClosedList[FClosedList.IndexOf(NodeSuccessor)];
				if((NodeClosed != null) && (NodeSuccessor.TotalCost > NodeClosed.TotalCost)) 
					continue;
				
				// Remove the old successor from the open list
				FOpenList.Remove(NodeOpen);
				
				// Remove the old successor from the closed list
				FClosedList.Remove(NodeClosed);
				
				// Add the current successor to the open list
				FOpenList.Push(NodeSuccessor);
			}
			// Add the current node to the closed list
			FClosedList.Add(NodeCurrent);
		}
	}

}
*/
