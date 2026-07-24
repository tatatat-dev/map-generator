
int length = 7;
int countSpawnedRooms = 0;
int countTargetRooms = 5;



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

CreateStartMap();
CreateFirstRoom();
NewRooms();
DrawMap();

void NewRooms()
{
    while (countSpawnedRooms < countTargetRooms)
    {

        int roomNow = rng.Next(0, activeRooms.Count);
        if (!CheckIsRoomActive(activeRooms[roomNow].y, activeRooms[roomNow].x)) continue;
        int nextDirection = rng.Next(0, 4);
        int targetY = activeRooms[roomNow].y + directions[nextDirection].y;
        int targetX = activeRooms[roomNow].x + directions[nextDirection].x;
        if (OutOfBounds(targetY, targetX)) continue;

        AddRoom(targetY, targetX);

    }
}

//TODO: делает 2 дела сразу, и проверяет ли комната активна и редактирует комнаты, надо распилить на 2 части
bool CheckIsRoomActive(int yRoom, int xRoom)
{
    bool isActive = false;
    //TODO:тут перегружено, если на первой иттерации видно что комната активна то выходить сразу нада
    for (int i = 0; i < directions.Length; i++)
    {
        bool currentResult = false;
        int targetY = yRoom + directions[i].y;
        int targetX = xRoom + directions[i].x;
        //TODO: можно 2 ифа просто соединить вместе
        if (OutOfBounds(targetY, targetX)) continue;
        if (mapArray[targetY, targetX] == '0') currentResult = true;
        isActive = isActive || currentResult;
    }

    if (isActive)
    {
        if (activeRooms.Contains((yRoom, xRoom))) return true;
        activeRooms.Add((yRoom, xRoom)); return true;

    }
    activeRooms.Remove((yRoom, xRoom));
    return false;
}
void AddRoom(int yRoom, int xRoom)
{
    mapArray[yRoom, xRoom] = '1';
    countSpawnedRooms++;
    CheckIsRoomActive(yRoom, xRoom);
}
bool OutOfBounds(int targetY, int targetX)
{
    //TODO: можно просто сразу строчку возвращать
    if (targetY < 0 || targetX < 0 || targetY >= length || targetX >= length) return true;
    return false;
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

