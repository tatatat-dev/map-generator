
int length = 7;
int howManyRoomsOnMap = 30;

Random rng = new Random();

char[,] mapArray = new char[length, length];
List<(int, int)> PossibleRooms = new List<(int, int)> ();

CreateStartMap();
PlaceMiddleRoom();
AddRooms();
Neighbours();
BossRoom();
SpecialRooms();
DrawMap();



void BossRoom()
{
    int xRoomNow = (length - 1) / 2;
    int yRoomNow = (length - 1) / 2;
    List<(int,int)> soloRooms = new List<(int,int)> ();
    for (int y = 0; y < length; y++)
    {
        for (int x = 0; x < length; x++)
        {
            if (mapArray[y, x] == '1')
            {
                soloRooms.Add((y, x));
            }
        }
    }
    if (soloRooms.Count != 0)
    {
 
        for (int i = 0; i < soloRooms.Count; i++)
        {
            int xPossible = Math.Abs(((length - 1) / 2) - soloRooms[i].Item2);
            int yPossible = Math.Abs(((length - 1) / 2) - soloRooms[i].Item1);
            if ((xPossible + yPossible) > (Math.Abs(xRoomNow - (length - 1) / 2)) + Math.Abs(yRoomNow - (length - 1) / 2))   
            {
                yRoomNow = soloRooms[i].Item1;
                xRoomNow = soloRooms[i].Item2;

            }
        }
        mapArray[yRoomNow, xRoomNow] = 'B';
    }
    else return; //тут должно быть исключение типа если нет комнат 1 свободных то както по другому босс руму ставь но оно в моем случае
                 // почти никогда не заработает, и мне впадлу это писать так как долго
}
//не эффективная залупа, захочу добавить планетарий прийдеться весь скрипт переписывать
void SpecialRooms()
{
    List<(int,int)> soloRooms = new List<(int, int)> ();
    bool isTreasureRoom = false;
    bool isShopRoom = false;
    for (int y = 0; y < length; y++)
    {
        for (int x = 0; x < length; x++)
        {
            if (mapArray[y,x] == '1')
            {
                soloRooms.Add((y, x));
            }
        }

    }

    for (int i = 0; i < soloRooms.Count && i < 2 /* магическое число */; i++)
    {
        //можно в метод вывести
        int roomNow = rng.Next(0, soloRooms.Count);
        if (!isTreasureRoom) 
        {
            mapArray[soloRooms[roomNow].Item1, soloRooms[roomNow].Item2] = 'T';
            isTreasureRoom = true;
            soloRooms.RemoveAt(roomNow);
        }
        else if (!isShopRoom)
        {
            mapArray[soloRooms[roomNow].Item1, soloRooms[roomNow].Item2] = 'S';
            isShopRoom = true;
            soloRooms.RemoveAt(roomNow);
        }
    }
    while (!isTreasureRoom ||  !isShopRoom)
    {
        int yRoomNow = rng.Next(0,length);
        int xRoomNow = rng.Next(0,length);

        bool canGoToUp = true;
        bool canGoToRight = true;
        bool canGoToLeft = true;
        bool canGoToDown = true;

        bool isThisPlaceGood = false;
        bool orNo = false;

        if (yRoomNow - 1 < 0) canGoToUp = false;
        if (yRoomNow + 1 == length) canGoToDown = false;
        if (xRoomNow - 1 < 0) canGoToLeft = false;
        if (xRoomNow + 1 == length) canGoToRight = false;

        //полный пиздец, впадлу было в метод выносить поэтому так, под снос
        if (canGoToUp)
        {
            if (mapArray[yRoomNow - 1, xRoomNow] != '0' && mapArray[yRoomNow - 1, xRoomNow] != 'S' && mapArray[yRoomNow - 1, xRoomNow] != 'T' && mapArray[yRoomNow - 1, xRoomNow] != 'B')
            {
                if (isThisPlaceGood) orNo = true;
                isThisPlaceGood = true;
            }
        }
        if (canGoToDown)
        {
            if (mapArray[yRoomNow + 1, xRoomNow] != '0' && mapArray[yRoomNow + 1, xRoomNow] != 'S' && mapArray[yRoomNow + 1, xRoomNow] != 'T' && mapArray[yRoomNow + 1, xRoomNow] != 'B')
            {
                if (isThisPlaceGood) orNo = true;
                isThisPlaceGood = true;
            }
        }
        if (canGoToLeft)
        {
            if (mapArray[yRoomNow, xRoomNow - 1] != '0' && mapArray[yRoomNow, xRoomNow - 1] != 'S' && mapArray[yRoomNow, xRoomNow - 1] != 'T' && mapArray[yRoomNow , xRoomNow - 1] != 'B')
            {
                if (isThisPlaceGood) orNo = true;
                isThisPlaceGood = true;
            }
        }
        if (canGoToRight)
        {
            if (mapArray[yRoomNow, xRoomNow + 1] != '0' && mapArray[yRoomNow, xRoomNow + 1] != 'S' && mapArray[yRoomNow, xRoomNow + 1] != 'T' && mapArray[yRoomNow, xRoomNow + 1] != 'B')
            {
                if (isThisPlaceGood) orNo = true;
                isThisPlaceGood = true;
            }
        }

        if (isThisPlaceGood && !orNo)
        {
            if (!isTreasureRoom) { mapArray[yRoomNow, xRoomNow] = 'T'; isTreasureRoom = true; }
            else if (!isShopRoom) {mapArray[yRoomNow, xRoomNow] = 'S'; isShopRoom = true;}
        }
    }
}

void Neighbours()
{
    for (int y = 0; y < length; y++)
    {
        for (int x = 0; x < length; x++)
        {
            int scetchik = 0; //англ название лучше ставить
            //пиздец как нагружает комп, кастрировать и поменять
            List<int> yChecker = new List<int>() { -1, 1 };
            List<int> xChecker = new List<int>() { -1, 1 };
            if (mapArray[y, x] == '0') continue;
            for (int i = 0; i < 4; i++)
            {
                if (y - 1 == -1) yChecker[0] = 0;
                if (x - 1 == -1) xChecker[0] = 0;
                if (y + 1 == length) yChecker[1] = 0;
                if (x + 1 == length) xChecker[1] = 0;
            }
            //кот залупа кот ванючка
            if (mapArray[y + yChecker[0], x] != '0' && y + yChecker[0] != y)
            {
                scetchik++;
            }
            if (mapArray[y + yChecker[1], x] != '0' && y + yChecker[1] != y)
            {
                scetchik++;
            }
            if (mapArray[y, x + xChecker[0]] != '0' && x + xChecker[0] != x)
            {
                scetchik++;
            }
            if (mapArray[y, x + xChecker[1]] != '0' && x + xChecker[1] != x)
            {
                scetchik++;
            }

            char something = (char)('0' + scetchik);
            mapArray[y, x] = something;
        }
    }
}
void AddRooms()
{
    for (int i = 0; i < howManyRoomsOnMap - 1; i++)
    {
        //лучше условие все же таки писать
        while (true)
        {
            int currentRoom = rng.Next(0, PossibleRooms.Count);
            int yRoom = PossibleRooms[currentRoom].Item1;
            int xRoom = PossibleRooms[currentRoom].Item2;

            int possibleY = 0;
            int possibleX = 0;

            if (!CheckisThisRoomCanBreed(yRoom, xRoom)) continue;

            int goTo = rng.Next(1, 5);
            // хуйня, можно написать полегче например через массив и фор в отедльном методе
            if (goTo == 1)
            {
                possibleY = yRoom - 1;
                possibleX = xRoom;
            }
            else if (goTo == 2)
            {
                possibleY = yRoom + 1;
                possibleX = xRoom;
            }
            else if (goTo == 3)
            {
                possibleY = yRoom;
                possibleX = xRoom - 1;
            }
            else
            {
                possibleY = yRoom;
                possibleX = xRoom + 1;
            }
            if (!ProverkaToGo(possibleY, possibleX)) continue;
            else
            {
                yRoom = possibleY;
                xRoom = possibleX;
                mapArray[yRoom, xRoom] = '1';
                CheckisThisRoomCanBreed(yRoom, xRoom);
                break;
            }

        }
    }
}
bool ProverkaToGo(int yRoom, int xRoom) //англ название лучше ставить
{
    //надо поменять
    bool canGoToUpLeft = true;
    bool canGoToUpRight = true;
    bool canGoToDownLeft = true;
    bool canGoToDownRight = true;
    //англ название лучше ставить
    canGoToUpLeft = proverkaToDiagonalTheoryGo(-1, -1, yRoom, xRoom);
    canGoToUpRight = proverkaToDiagonalTheoryGo( -1, 1, yRoom, xRoom);
    canGoToDownLeft = proverkaToDiagonalTheoryGo(1, -1, yRoom, xRoom);
    canGoToDownRight = proverkaToDiagonalTheoryGo(1, 1, yRoom, xRoom);
    //до сюда

    if (yRoom == -1 || yRoom == length || xRoom == -1 || xRoom == length) return false;
    else if (mapArray[yRoom, xRoom] == '1') return false;

    //надо поменять
    if (canGoToUpLeft)
    {
        if (mapArray[yRoom - 1, xRoom] == '1' && mapArray[yRoom, xRoom - 1] == '1' && mapArray[yRoom - 1, xRoom - 1] == '1') return false; //верх лево
    }
    if (canGoToUpRight)
    {
        if (mapArray[yRoom - 1, xRoom] == '1' && mapArray[yRoom, xRoom + 1] == '1' && mapArray[yRoom - 1, xRoom + 1] == '1') return false; //верх право
    }
    if (canGoToDownLeft)
    {
        if (mapArray[yRoom + 1, xRoom] == '1' && mapArray[yRoom, xRoom - 1] == '1' && mapArray[yRoom + 1, xRoom - 1] == '1') return false; //вниз лево
    }
    if (canGoToDownRight)
    {
        if (mapArray[yRoom + 1, xRoom] == '1' && mapArray[yRoom, xRoom + 1] == '1' && mapArray[yRoom + 1, xRoom + 1] == '1') return false; //вниз право
    }
    //до сюда
    return true;
}
bool proverkaToDiagonalTheoryGo(int dy, int dx, int yRoom, int xRoom) //англ название лучше ставить
{
    if (yRoom + dy == -1 || yRoom + dy == length || xRoom + dx == -1 || xRoom + dx == length)
    {
        return false;
    }
    return true;
}


void PlaceMiddleRoom()
{
    //какбудто бы под 1 среднюю комнату отдельный метод хуйня, надо подумать можно ли поменять както
    int yRoom = (length - 1) / 2;
    int xRoom = (length - 1) / 2;
    CheckisThisRoomCanBreed(yRoom, xRoom);
}

bool CheckisThisRoomCanBreed(int yRoom, int xRoom) //что за пиздец а не название
{
    if (!ProverkaCanBreed(yRoom, xRoom))
    {
        PossibleRooms.Remove((yRoom, xRoom));
        return false;

    }
    return true;
}
bool ProverkaCanBreed(int yRoom, int xRoom)
{
    //нельзя так делать
    bool canGoUp = true;
    bool canGoDown = true;
    bool canGoLeft = true;
    bool canGoRight = true;

    //нельзя так делать, в отдельный метод вынести и передаввать значения типа dy, dx
    if (yRoom - 1 == -1) canGoUp = false;
    if (yRoom + 1 == length) canGoDown = false;
    if (xRoom - 1 == -1) canGoLeft = false;
    if (xRoom + 1 == length) canGoRight = false;

    //пиздец
    canGoUp = ProverkaToTheoryGo(canGoUp, -1, 0, yRoom, xRoom);
    canGoDown = ProverkaToTheoryGo(canGoDown, 1, 0, yRoom, xRoom);
    canGoLeft = ProverkaToTheoryGo(canGoLeft, 0, -1, yRoom, xRoom);
    canGoRight = ProverkaToTheoryGo(canGoRight, 0, 1, yRoom, xRoom);

    if (!canGoUp && !canGoDown && !canGoLeft && !canGoRight) return false;
    else
    {
        if (PossibleRooms.Contains((yRoom,xRoom)))
        {
            return true;
        }
        PossibleRooms.Add((yRoom, xRoom));
        return true;
    }
}

//впринципе попытка избавиться от кучи копипаста выше, хз почему и потом не юзал
bool ProverkaToTheoryGo(bool canGo, int dy, int dx, int yRoom, int xRoom ) 
{
    if (canGo)
    {
        if (mapArray[yRoom + dy, xRoom + dx] != '0')
        {
            return false;
        }
        return true;
    }
    return false;
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