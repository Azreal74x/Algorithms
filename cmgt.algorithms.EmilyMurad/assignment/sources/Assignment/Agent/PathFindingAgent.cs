using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class PathFindingAgent : NodeGraphAgent {

  //Current target to move towards
  private Node _target = null;

  private Node currentNode; //keep track of where morc is at right now
  private Node newNode; //keep track of the newly clicked nodes

  private List<Node> nodeQueue = new List<Node>();

  PathFinder pathFinder; //to store pathFinder

  public PathFindingAgent(NodeGraph pNodeGraph, PathFinder pPathFinder) : base(pNodeGraph) {
    pathFinder = pPathFinder;
    SetOrigin(width / 2, height / 2);

    //position ourselves on a random node
    if (pNodeGraph.nodes.Count > 0) {
      Node randomNode = pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)];
      jumpToNode(randomNode);
      currentNode = randomNode;
      Console.WriteLine($"random currentNode is {currentNode}");
    }

    //listen to nodeclicks
    pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
  }

  protected virtual void onNodeClickHandler(Node pNode) {
    newNode = pNode;
    Console.WriteLine($"mouse clicked, new node is {newNode}");

    //if we're not moving, generate a path
    if (nodeQueue.Count == 0) {
      nodeQueue = pathFinder.Generate(currentNode, newNode);
      Console.WriteLine($"generating path from currentNode {currentNode} to newNode {newNode}");
    }
  }

  protected override void Update() {
    //set target to first in nodeQueue
     if (nodeQueue.Count > 0) {
      _target = nodeQueue[0];
      //Console.WriteLine($"current target is {_target}");
    }

    //no target? Don't walk
    if (_target == null) {
      //Console.WriteLine("target is null");
      return;
    }

    //Move towards the target node, if we reached it, clear the target
    if (moveTowardsNode(_target)) {
      currentNode = _target;
      _target = null;

      nodeQueue.RemoveAt(0);
      Console.WriteLine($"target reached, node removed from queue, current queue is {nodeQueue.Count} long");
    }
  }
}
