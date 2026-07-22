
int length = 7;
int roomsOnMap = 5;



Random rng = new Random();

char[,] mapArray = new char[length, length];
List<(int,int)> activeRooms = new List<(int,int)> (); //у кортежа можно дать именования чтобы не писать Item1 Item2

int[,] directions = //переписать на массив кортежей типа (int dy, int dx)[] directions =  
{
    {-1,1,0,0 },
    {0,0,-1,1 }
};

CreateStartMap();
CreateFirstRoom();
NewRooms();
DrawMap();

void NewRooms()
{
    for (int i = 0; i < roomsOnMap - 1; i++)
    {
        int roomNow = rng.Next(0, activeRooms.Count);
        int nextDirection = rng.Next(0, 4);
        int dy = directions[0, nextDirection];
        int dx = directions[1, nextDirection];

        mapArray[activeRooms[roomNow].Item1 + dy, activeRooms[roomNow].Item2 + dx] = '1';
    }
}
void CreateStartMap()
{
    for (int y = 0; y < length; y++)
    {
        for (int x = 0;  x < length; x++)
        {
            mapArray[y, x] = '0';
        }
    }
}

void CreateFirstRoom()
{
    int roomY = (length - 1) / 2;
    int roomX = (length - 1) / 2;

    mapArray[roomY, roomX] = '1';
    activeRooms.Add((roomY, roomX));

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

