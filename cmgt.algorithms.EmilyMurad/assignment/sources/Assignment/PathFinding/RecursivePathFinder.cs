using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class RecursivePathFinder : PathFinder {
  public RecursivePathFinder(NodeGraph pGraph) : base(pGraph) { }

  Dictionary<Node, bool> checkedNode = new Dictionary<Node, bool>();
  List<Node> currentPath = new List<Node>();
  List<Node> shortestPath = new List<Node>();

  protected override List<Node> generate(Node pFrom, Node pTo) {
    checkedNode.Clear();
    currentPath.Clear();
    shortestPath.Clear();

    //call recursive method
    Console.WriteLine($"pFrom is {pFrom} and pTo is {pTo}");
    FindPath(pFrom, pTo, checkedNode, currentPath);

    //return path
    if (shortestPath.Count == 0) {
      Console.WriteLine("pFrom is equal to pTo");
      return null;
    }
    else {
      return shortestPath;
    }
  }

  private bool FindPath(Node currentNode, Node targetNode, Dictionary<Node, bool> pCheckedNode, List<Node> currentPath) {
    //base case
    if (currentNode == targetNode) {
      Console.WriteLine($"pFrom {currentNode} is equal to pTo {targetNode}");
      currentPath.Add(currentNode);
      //update shortest path if a shorter one was found
      if (shortestPath.Count == 0 || currentPath.Count < shortestPath.Count) { 
        Console.WriteLine($"shorter path found, shortestPath count is {currentPath.Count} and currentPath count is {shortestPath.Count}");
        shortestPath.Clear();
        shortestPath.AddRange(currentPath);
        Console.WriteLine($"new shortestPath is {string.Join(" -> ", shortestPath)}");
      }
      currentPath.RemoveAt(currentPath.Count - 1);
      Console.WriteLine("removing currentNode from currentPath");
      return true;
    }

    //mark node as checked
    pCheckedNode[currentNode] = true;
    currentPath.Add(currentNode);
    Console.WriteLine($"currentNode {currentNode} checked (true) and added to currentPath {string.Join(" -> ", currentPath)}");

    //repeat for all neighbours of currentNode
    foreach (Node neighbour in currentNode.connections) {
      if (!pCheckedNode.ContainsKey(neighbour) || !pCheckedNode[neighbour]) {
        Console.WriteLine($"neighbour {neighbour} hasn't been visited recursive call FindPath on neighbour");
        FindPath(neighbour, targetNode, pCheckedNode, currentPath);
      }
    }

    //backtrack, remove node from path
    currentPath.RemoveAt(currentPath.Count - 1);
    Console.WriteLine("removing currentNode from currentPath");
    pCheckedNode[currentNode] = false;
    Console.WriteLine($"currentNode {currentNode} checked (false) ");
    return false;
  }
}
