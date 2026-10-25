using System;
using GXPEngine;
using System.Drawing;
using System.Collections.Generic;

class GoodDungeon : Dungeon {
  public GoodDungeon(Size pSize) : base(pSize) { }

  static private Random random = new Random();

  List<Room> roomsToSplit = new List<Room>();

  private int doorPadding = 2;
  private int minWallSize = 4;

  protected override void generate(int pMinimumRoomSize) {
    CreateAndSplitRooms(pMinimumRoomSize);
    RemoveSmallestAndBiggestRooms();
    FindIntersectionAndCreateDoors();
    FillRoomsAndDoors();
  }

  private void CreateAndSplitRooms(int pMinRoomSize) {
    Room firstRoom = new Room(new Rectangle(0, 0, size.Width, size.Height));
    Console.WriteLine($"first room area: {firstRoom}firstRoom.area");
    roomsToSplit.Add(firstRoom);

    while (roomsToSplit.Count > 0) {
      Room unsplitRoom = roomsToSplit[0];
      //vertical split
      if (unsplitRoom.area.Width / 2 > pMinRoomSize && unsplitRoom.area.Width > unsplitRoom.area.Height) { 
        //split into 2 rooms 
        Room room1 = new Room(new Rectangle(unsplitRoom.area.X, unsplitRoom.area.Y, random.Next(pMinRoomSize, unsplitRoom.area.Width - pMinRoomSize), unsplitRoom.area.Height));
        Room room2 = new Room(new Rectangle(unsplitRoom.area.X + room1.area.Width - 1, unsplitRoom.area.Y, unsplitRoom.area.Width - room1.area.Width + 1, unsplitRoom.area.Height));

        //add new rooms so they get split
        roomsToSplit.Add(room1);
        roomsToSplit.Add(room2);

        Console.WriteLine($"vertical split room1 = {room1.area}");
        Console.WriteLine($"room2 = {room2.area}");
      }
      //horizontal split
      else if (unsplitRoom.area.Height / 2 > pMinRoomSize && unsplitRoom.area.Height >= unsplitRoom.area.Width) { 
        //split into 2 rooms
        Room room1 = new Room(new Rectangle(unsplitRoom.area.X, unsplitRoom.area.Y, unsplitRoom.area.Width, random.Next(pMinRoomSize, unsplitRoom.area.Height - pMinRoomSize)));
        Room room2 = new Room(new Rectangle(unsplitRoom.area.X, unsplitRoom.area.Y + room1.area.Height - 1, unsplitRoom.area.Width, unsplitRoom.area.Height - room1.area.Height + 1));

        //add new rooms so they get split
        roomsToSplit.Add(room1);
        roomsToSplit.Add(room2);

        Console.WriteLine($"horizontal split room1 = {room1.area}");
        Console.WriteLine($"horizontal split room2 = {room2.area}");
      }
      else {
        //if room cant be spllit add to rooms
        rooms.Add(unsplitRoom); 
        Console.WriteLine($"added {unsplitRoom.area} to rooms");
      }

      //for debug
      //drawRoom(roomsToSplit[0], wallPen);

      //remove the unsplit room from the list once its done so the next one can get split
      roomsToSplit.RemoveAt(0);

      //for debug
      //Console.ReadLine(); 
    }

    foreach (Room room in rooms) {
      Console.WriteLine($"new room area: {room.area}");
    }
  }

  private void RemoveSmallestAndBiggestRooms() {
    //return if rooms is empty
    if (rooms.Count == 0) return; 

    Room smallestRoom = rooms[0];
    Room biggestRoom = rooms[0];

    //find smallest and biggest rooms
    foreach (Room room in rooms) {
      int roomArea = room.area.Width * room.area.Height;
      if (roomArea < smallestRoom.area.Width * smallestRoom.area.Height) {
        smallestRoom = room;
        Console.WriteLine($"smallest room found: {smallestRoom}");
      }
      if (roomArea > biggestRoom.area.Width * biggestRoom.area.Height) {
        biggestRoom = room;
        Console.WriteLine($"biggest room found: {biggestRoom}");
      }
    }

    rooms.Remove(smallestRoom);
    rooms.Remove(biggestRoom);

    Console.WriteLine($"removed smallest room: {smallestRoom.area}");
    Console.WriteLine($"removed largest room: {biggestRoom.area}");
  }

  private void FindIntersectionAndCreateDoors() {
    for (int i = 0; i < rooms.Count; i++) {
      for (int j = i + 1; j < rooms.Count; j++) {
        //store overlapping rectangles area
        Rectangle intersection = Rectangle.Intersect(rooms[i].area, rooms[j].area);
        if (!intersection.IsEmpty) {
          Console.WriteLine($"intersection of {rooms[i].area} & {rooms[j].area} is {intersection}");
          //vertical door 
          if (intersection.Width > minWallSize && intersection.Height == 1) { 
            try {
              //create door at random point on intersection
              Door door1 = new Door(new Point(random.Next(intersection.X + doorPadding, intersection.X + intersection.Width - doorPadding), intersection.Y));
              doors.Add(door1);
              Console.WriteLine($"new door1 location: {door1.location}");
            }
            catch {
              Console.WriteLine("min is bigger than max, skipping door");
            }
          }
          //horizontal door 
          else if (intersection.Width == 1 && intersection.Height > minWallSize) { 
            try {
              //create door at random point on intersection
              Door door1 = new Door(new Point(intersection.X, random.Next(intersection.Y + doorPadding, intersection.Y + intersection.Height - doorPadding)));
              doors.Add(door1);
              Console.WriteLine($"new door1 location: {door1.location}");
            }
            catch {
              Console.WriteLine("min is bigger than max, skipping door");
            }
          }
        }
      }
    }
  }

  private void FillRoomsAndDoors() {
    Brush fillColor = new SolidBrush(Color.White);

    //check all doors in rooms to know which color to fill
    foreach (Room room in rooms) {
      int doorCount = 0;
      foreach (Door door in doors) {
        if (room.area.Contains(door.location)) {
          doorCount += 1;
          Console.WriteLine($"room {room} doorCount is {doorCount}");
        }
      }
      switch (doorCount) {
        case 0:
          fillColor = new SolidBrush(Color.Red);
          break;
        case 1:
          fillColor = new SolidBrush(Color.Orange);
          break;
        case 2:
          fillColor = new SolidBrush(Color.Yellow);
          break;
        case 3:
          fillColor = new SolidBrush(Color.Green);
          break;
        default:
          fillColor = new SolidBrush(Color.Purple);
          break;
      }
      Console.WriteLine($"room {room} will be filled with the color {fillColor}");
      drawRoom(room, wallPen, fillColor);
    }
    drawDoors(doors, doorPen); //draw all the doors
  }

}
