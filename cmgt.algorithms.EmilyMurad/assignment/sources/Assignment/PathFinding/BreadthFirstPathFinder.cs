using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class BreadthFirstPathFinder : PathFinder {
  public BreadthFirstPathFinder(NodeGraph pGraph) : base(pGraph) { }

  Dictionary<Node, bool> checkedNode = new Dictionary<Node, bool>();
  Dictionary<Node, Node> previousNode = new Dictionary<Node, Node>();
  List<Node> currentQueuePath = new List<Node>();

  protected override List<Node> generate(Node pFrom, Node pTo) {
    checkedNode.Clear();
    previousNode.Clear();
    currentQueuePath.Clear();
    Console.WriteLine("clear previous generate");

    Console.WriteLine($"pFrom is {pFrom} and pTo is {pTo}");
    return FindPath(pFrom, pTo);
  }

  private List<Node> FindPath(Node startNode, Node targetNode) {
    currentQueuePath.Add(startNode);
    checkedNode[startNode] = true;
    Console.WriteLine($"add startNode {startNode} to currentQueuePath and mark it as checked");

    while (currentQueuePath.Count > 0) {
      //dequeue and check first node
      Node currentNode = currentQueuePath[0];
      currentQueuePath.RemoveAt(0);
      Console.WriteLine($"currentNode is {currentNode}");

      //build and return path
      if (currentNode == targetNode) {
        Console.WriteLine($"currentNode {currentNode} is equal to targetNode {targetNode}");
        return BuildPath(startNode, targetNode);
      }

      //check each neighbour and add it to queue when unchecked
      foreach (Node neighbour in currentNode.connections) {
        if (!checkedNode.ContainsKey(neighbour) || !checkedNode[neighbour]) {
          Console.WriteLine($"unchecked neighbour {neighbour} found, checking now");
          checkedNode[neighbour] = true;
          previousNode[neighbour] = currentNode;
          currentQueuePath.Add(neighbour);
          Console.WriteLine($"neighbour {neighbour} marked as checked and added to currentQueuePath");
        }
      }
    }

    Console.WriteLine("no path found");
    return null;
  }

  private List<Node> BuildPath(Node startNode, Node targetNode) {
    //initialize
    List<Node> path = new List<Node>();
    Node currentNode = targetNode;
    Console.WriteLine($"currentNode is {currentNode}");

    //check if the previousNode is connected to currentNode
    while (currentNode != null) { 
      path.Add(currentNode);
      Console.WriteLine($"added currentNode {currentNode} to path which is {string.Join(" -> ", path)}");
      if (!previousNode.ContainsKey(currentNode)) {
        Console.WriteLine($"previousNode is connected to currentNode {currentNode}");
        break;
      }
      currentNode = previousNode[currentNode]; 
    }

    //reverse list to get correct order
    path.Reverse();
    Console.WriteLine($"reversed path is now {string.Join(" -> ", path)}");

    //check path isn't empty and it starts correctly
    if (path.Count > 0 && path[0] == startNode) {
      Console.WriteLine($"Path found: {string.Join(" -> ", path)}");
      return path;
    }
    else {
      Console.WriteLine("bad path");
      return null;
    }
  }
}
