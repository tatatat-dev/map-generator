
int length = 7;

int countSpawnedRooms = 0;
int countTargetRooms = 50;

int attempts = 0;
int maxAttempts = 1000;

Random rng = new Random();

char[,] mapArray = new char[length, length];
List<(int y, int x)> activeRooms = new List<(int, int)>();



(int y, int x)[] directions =
{
    (-1,0), //0: вверх
    (1,0),  //1: вниз
    (0,-1), //2: влево
    (0,1)   //3: вправо
};
(int vert, int horiz)[] corners =
{
    (0,2), //0: вверх-влево
    (0,3), //1: вверх-вправо
    (1,2), //2: вниз-влево
    (1,3)  //3: вниз-вправо
};

CreateStartMap();
CreateFirstRoom();
NewRooms();
Neighbours();
DrawMap();

void Neighbours()
{
    for (int y = 0; y < length; y++)
    {
        for (int x = 0; x < length; x++)
        {
            if (mapArray[y, x] == '0') continue;
            mapArray[y, x] = (char)('0' + CountNeighbours(y, x));
        }
    }
}

int CountNeighbours(int roomY,int roomX)
{
    int count = 0;
    for (int i = 0; i < directions.Length; i++)
    {
        int targetY = roomY + directions[i].y;
        int targetX = roomX + directions[i].x;
        if (OutOfBounds(targetY, targetX)) continue;
        if (mapArray[targetY, targetX] == '0') continue;
        count++;
    }
    return count;
}
void NewRooms()
{


    while (countSpawnedRooms < countTargetRooms)
    {
        attempts++;
        int roomNow = rng.Next(0, activeRooms.Count);

        if (!CheckIsRoomActive(activeRooms[roomNow].y, activeRooms[roomNow].x)) continue;

        int nextDirection = rng.Next(0, 4);

        int targetY = activeRooms[roomNow].y + directions[nextDirection].y;
        int targetX = activeRooms[roomNow].x + directions[nextDirection].x;

        if (attempts == maxAttempts) break;
        if (OutOfBounds(targetY, targetX) || !CanPlaceRoom(targetY, targetX) || IsRoomCreateSquare(targetY, targetX)) continue;

        AddRoom(targetY, targetX);
        attempts = 0;
    }
}

bool CanPlaceRoom(int roomY, int roomX)
{
    if (mapArray[roomY, roomX] != '0') return false;
    return true;
}

bool IsRoomCreateSquare(int roomY, int roomX)
{
    for (int i = 0; i < directions.Length; i++)
    {
        int vertMove = roomY + directions[corners[i].vert].y;
        int horizMove = roomX + directions[corners[i].horiz].x;
        if (OutOfBounds(vertMove, horizMove)) continue;
        if (mapArray[vertMove, roomX] != '0' && mapArray[roomY, horizMove] != '0' && mapArray[vertMove, horizMove] != '0') return true;
    }
    return false;
}
bool CheckIsRoomActive(int roomY, int roomX)
{
    for (int i = 0; i < directions.Length; i++)
    {
        int targetY = roomY + directions[i].y;
        int targetX = roomX + directions[i].x;
        if (OutOfBounds(targetY, targetX) || mapArray[targetY, targetX] != '0') continue;
        return true;
    }



    activeRooms.Remove((roomY, roomX)); //можно перенести но какбудто нахуй надо
    return false;
}
void MakeRoomActive(int roomY, int roomX)
{
    if (!CheckIsRoomActive(roomY, roomX) || activeRooms.Contains((roomY, roomX))) return;
    activeRooms.Add((roomY, roomX));

}
void AddRoom(int roomY, int roomX)
{
    mapArray[roomY, roomX] = '1';
    countSpawnedRooms++;
    MakeRoomActive(roomY, roomX);
}
bool OutOfBounds(int targetY, int targetX)
{

    return (targetY < 0 || targetX < 0 || targetY >= length || targetX >= length);

}
void CreateStartMap()
{
    for (int y = 0; y < length; y++)
    {
        for (int x = 0; x < length; x++)
        {
            mapArray[y, x] = '0';
        }
    }
}

void CreateFirstRoom()
{
    int roomY = (length - 1) / 2;
    int roomX = (length - 1) / 2;

    AddRoom(roomY, roomX);

}

void DrawMap()
{
    for (int y = 0; y < length; y++)
    {
        for (int x = 0; x < length; x++)
        {
            Console.Write(mapArray[y, x]);
        }
        Console.WriteLine();
    }
}
