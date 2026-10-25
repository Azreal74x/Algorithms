using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

class OnGraphWayPointAgent : NodeGraphAgent {

  //Current target to move towards
  private Node _target = null;

  //keep track of where morc is at right now
  private Node currentNode;

  //keep track of the newly clicked nodes
  private Node newNode; 

  private List<Node> nodeQueue = new List<Node>();

  public OnGraphWayPointAgent(NodeGraph pNodeGraph) : base(pNodeGraph) {
    SetOrigin(width / 2, height / 2);

    //position ourselves on a random node
    if (pNodeGraph.nodes.Count > 0) {
      Node randomNode = pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)];
      jumpToNode(randomNode);
      currentNode = randomNode;
    }

    //listen to nodeclicks
    pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
  }

  protected virtual void onNodeClickHandler(Node pNode) { 
    newNode = pNode;
    Console.WriteLine($"new node: {newNode}");

    if (newNode != currentNode) { 
      //Console.WriteLine("new node is not the same as current node");
      if (currentNode.connections.Contains(newNode)) {
        nodeQueue.Add(newNode);
        currentNode = newNode; 
        //Console.WriteLine("currentNode has a connection to newNode");
        Console.WriteLine($"node added to queue, current queue: {nodeQueue.Count}");
      }
    }
  }

  protected override void Update() {
    //set target to first in nodeQueue
    if (nodeQueue.Count > 0) { 
      _target = nodeQueue[0]; 
    }

    //no target? Don't walk
    if (_target == null) {
      return;
    }

    //Move towards the target node, if we reached it, clear the target
    if (moveTowardsNode(_target)) {
      _target = null;

      nodeQueue.RemoveAt(0); 
      //Console.WriteLine("morc reached target");
      Console.WriteLine($"node removed from queue, current queue: {nodeQueue.Count}");
    }
  }
}
