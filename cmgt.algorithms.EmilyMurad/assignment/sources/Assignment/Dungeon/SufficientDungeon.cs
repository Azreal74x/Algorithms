using System;
using GXPEngine;
using System.Drawing;
using System.Collections.Generic;

class SufficientDungeon : Dungeon {
  public SufficientDungeon(Size pSize) : base(pSize) { }

  static private Random random = new Random();

  List<Room> roomsToSplit = new List<Room>();

  private int doorPadding = 2;
  private int minWallSize = 4;

  protected override void generate(int pMinimumRoomSize) {
    CreateAndSplitRooms(pMinimumRoomSize);
    FindIntersectionAndCreateDoors();
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

      //draw all the rooms
      drawRooms(rooms, wallPen); 
    }

    foreach (Room room in rooms) {
      Console.WriteLine($"new room area: {room.area}");
    }
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
          drawDoors(doors, doorPen); //draw all the doors
        }
      }
    }
  }
}
