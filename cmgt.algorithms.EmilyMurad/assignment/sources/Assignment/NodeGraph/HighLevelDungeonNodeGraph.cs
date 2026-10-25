using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

class HighLevelDungeonNodeGraph : SampleDungeonNodeGraph {
  public HighLevelDungeonNodeGraph(Dungeon pDungeon) : base(pDungeon) { }

  Dictionary<Room, Node> roomNodes = new Dictionary<Room, Node>(); 
  Dictionary<Door, Node> doorNodes = new Dictionary<Door, Node>(); 

  protected override void generate() {
    CreateAndMapRoomNodes();
    CreateAndMapDoorNodes();
    CreateNodeConnections();
  }

  //create node for each room and map it to corresponding room in dictionary
  protected void CreateAndMapRoomNodes() { 
    foreach (Room room in _dungeon.rooms) {
      Node roomNode = new Node(getRoomCenter(room));
      nodes.Add(roomNode);
      roomNodes[room] = roomNode;
      Console.WriteLine(room + " roomNode: " + roomNode.id);
    }
  }

  //create node for each door and map it to corresponding door in dictionary
  protected void CreateAndMapDoorNodes() { 
    foreach (Door door in _dungeon.doors) { 
      Node doorNode = new Node(getDoorCenter(door)); 
      nodes.Add(doorNode); 
      doorNodes[door] = doorNode; 
      Console.WriteLine(door + " doorNode: " + doorNode.id);
    }
  }

  //check which doors are in each room and create connections between them
  protected void CreateNodeConnections() { 
    for (int i = 0; i < _dungeon.doors.Count; i++) {
      Door door = _dungeon.doors[i];
      //Console.WriteLine("door location: " + door.location);

      for (int j = 0; j < _dungeon.rooms.Count; j++) {
        Room room = _dungeon.rooms[j];
        //Console.WriteLine("room area: " + room.area);

        if (_dungeon.rooms[j].area.Contains(door.location)) {
          AddConnection(doorNodes[door], roomNodes[room]); 
          //Console.WriteLine("Connection added between " + doorNodes[door] + " and " + roomNodes[room]);
        }
      }
    }
  }
}
