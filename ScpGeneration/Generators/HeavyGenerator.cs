using Edgar.GraphBasedGenerator.Grid2D;
using Edgar.GraphBasedGenerator.Grid2D.Drawing;
using Edgar.Graphs;
using Edgar.Legacy.Utils;
using ScpGeneration.Data;
using ScpGeneration.Rooms.Descriptions.Heavy;
using ScpGeneration.Rooms.Descriptions.Heavy.SCP;

namespace ScpGeneration.Generators;

public class HeavyGenerator : Generator<Room>
{
    private const int KetersNum = 2;
    private const int EuclidNum = 6;
    private const int SafeNum = 2;
    private const int MiscNum = 2;
    private readonly Range<int> _scpConnectorRange = new(1, 4);
    private const int GatesNum = 2;

    private const int MaxConnectorSlots = 4;

    readonly RoomDescriptionGrid2D _miscRooms = new HeavyRooms().Get();
    readonly RoomDescriptionGrid2D _heavyConnectors = new HeavyConnectors().Get();
    readonly RoomDescriptionGrid2D _corridors = new HeavyCorridors().Get();
    readonly RoomDescriptionGrid2D _vertGates = new HeavyVerticalGates().Get();
    readonly RoomDescriptionGrid2D _horGates = new HeavyHorizontalGates().Get();

    readonly RoomDescriptionGrid2D _safe = new HeavySafe().Get();
    readonly RoomDescriptionGrid2D _euclid = new HeavyEuclid().Get();
    readonly RoomDescriptionGrid2D _keter = new HeavyKeter().Get();


    private int _roomCounter = 0;
    private ScpGraph<Room> _graph = new();

    private Dictionary<RoomType, RoomDescriptionGrid2D> _descriptions;

    public HeavyGenerator(int seed) : base(seed)
    {
        _descriptions = new Dictionary<RoomType, RoomDescriptionGrid2D>
        {
            { RoomType.GateVer, _vertGates },
            { RoomType.GateHor, _horGates },
            { RoomType.Corridor, _corridors },
            { RoomType.Connector, _heavyConnectors },
            { RoomType.Misc, _miscRooms },
            { RoomType.Safe, _safe },
            { RoomType.Euclid, _euclid },
            { RoomType.Keter, _keter }
        };
    }

    public override LevelDescriptionGrid2D<Room> GetLevelDescription()
    {
        MakeHeavyZone();
        _graph.RemoveIsolatedVertices();

        LevelDescriptionGrid2D<Room> descriptionGrid2D = new LevelDescriptionGrid2D<Room>();
        foreach (Room? vertex in _graph.Vertices)
        {
            descriptionGrid2D.AddRoom(vertex, _descriptions[vertex.RoomType]);
        }

        foreach (IEdge<Room>? edge in _graph.Edges)
        {
            descriptionGrid2D.AddConnection(edge.From, edge.To);
        }

        return descriptionGrid2D;
    }

    protected void MakeHeavyZone()
    {
        Random random = new Random(SavedSeed + 501);

        (Room first, Room second, Room Mid) leftBranch = GenerateLeftBranch(random);
        Room rightBranch = GenerateRightBranch(random);
        ConnectBranches(leftBranch.Mid, rightBranch);
    }

    protected void ConnectBranches(Room leftBranch, Room rightBranch)
    {
        Room central = new Room(_roomCounter++, RoomType.Connector);
        _graph.AddVertex(central);
        Room centralLeft = new Room(_roomCounter++, RoomType.Connector);
        _graph.AddVertex(centralLeft);
        Room centralRight = new Room(_roomCounter++, RoomType.Connector);
        _graph.AddVertex(centralRight);

        Room a = new Room(_roomCounter++, RoomType.Corridor);
        _graph.AddVertex(a);
        Room b = new Room(_roomCounter++, RoomType.Corridor);
        _graph.AddVertex(b);

        Room c = new Room(_roomCounter++, RoomType.Corridor);
        _graph.AddVertex(c);
        Room d = new Room(_roomCounter++, RoomType.Corridor);
        _graph.AddVertex(d);

        ConnectRooms(leftBranch, centralLeft, a);
        ConnectRooms(centralLeft, central, b);

        ConnectRooms(rightBranch, centralRight, c);
        ConnectRooms(centralRight, central, d);
    }

    protected Room GenerateRightBranch(Random rand)
    {
        List<Room> branch = [];

        int randCount = rand.Next(2, 4); // 2 or 3
        for (int i = 0; i < randCount; ++i)
        {
            branch.Add(GenerateScpConnector(rand));
        }

        Room connector = new Room(_roomCounter++, RoomType.Connector);
        _graph.AddVertex(connector);
        ConnectScpRoomsWithConnector(branch, connector);

        return connector;
    }


    protected (Room first, Room second, Room Mid) GenerateLeftBranch(Random rand)
    {
        List<Room> firstBranch = [];

        int randCount = rand.Next(2, 4); // 2 or 3
        for (int i = 0; i < randCount; ++i)
        {
            firstBranch.Add(GenerateScpConnector(rand));
        }

        Room firstConnector = new Room(_roomCounter++, RoomType.Connector);
        _graph.AddVertex(firstConnector);
        ConnectScpRoomsWithConnector(firstBranch, firstConnector);

        List<Room> secondBranch = [];
        randCount = rand.Next(2, 4); // 2 or 3
        for (int i = 0; i < randCount; ++i)
        {
            secondBranch.Add(GenerateScpConnector(rand));
        }

        Room secondConnector = new Room(_roomCounter++, RoomType.Connector);
        _graph.AddVertex(secondConnector);
        ConnectScpRoomsWithConnector(secondBranch, secondConnector);

        Room midConnector = new Room(_roomCounter++, RoomType.Connector);
        _graph.AddVertex(midConnector);
        Room aCor = new Room(_roomCounter++, RoomType.Corridor);
        _graph.AddVertex(aCor);
        Room bCor = new Room(_roomCounter++, RoomType.Corridor);
        _graph.AddVertex(bCor);
        ConnectRooms(firstConnector, midConnector, aCor);
        ConnectRooms(secondConnector, midConnector, bCor);

        return (firstConnector, secondConnector, midConnector);
    }

    protected void ConnectScpRoomsWithConnector(List<Room> scpRooms, Room connector)
    {
        foreach (Room scp in scpRooms)
        {
            Room corridor = new Room(_roomCounter++, RoomType.Corridor);
            _graph.AddVertex(corridor);

            ConnectRooms(connector, scp, corridor);
        }
    }

    protected Room GenerateScpConnector(Random random)
    {
        Room connector = new Room(_roomCounter++, RoomType.Connector);
        _graph.AddVertex(connector);
        int maxDoors = random.Next(_scpConnectorRange.Minimum, _scpConnectorRange.Maximum);
        for (int i = 0; i < maxDoors; ++i)
        {
            Room scp = GenerateRandomScpRoom(random);
            if (random.Next(0, 2) == 0)
            {
                Room corridor = new Room(_roomCounter++, RoomType.Corridor);
                _graph.AddVertex(corridor);

                ConnectRooms(connector, scp, corridor);
            }
            else
            {
                ConnectRooms(scp, connector);
            }
        }

        return connector;
    }

    protected Room GenerateRandomScpRoom(Random random)
    {
        Room room = new Room(_roomCounter++, GetRandomRoomType(random));
        _graph.AddVertex(room);
        return room;
    }

    public RoomType GetRandomRoomType(Random random)
    {
        List<RoomType> allRoomTypes = ((RoomType[])Enum.GetValues(typeof(RoomType))).ToList();

        // Перемешиваем порядок типов комнат случайным образом
        allRoomTypes.Shuffle(random);

        // Пытаемся вернуть первый доступный тип комнаты
        foreach (RoomType roomType in allRoomTypes)
        {
            if (IsAbleToCreate(roomType))
            {
                return roomType;
            }
        }

        // Если ни один тип не доступен, возвращаем Misc (по умолчанию)
        return RoomType.Misc;
    }

    protected bool IsAbleToCreate(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.Misc:
                return GetRoomsOfType(roomType).Count() < MiscNum;
            case RoomType.Safe:
                return GetRoomsOfType(roomType).Count() < SafeNum;
            case RoomType.Euclid:
                return GetRoomsOfType(roomType).Count() < EuclidNum;
            case RoomType.Keter:
                return GetRoomsOfType(roomType).Count() < KetersNum;
            default:
                return false;
        }
    }

    protected void ConnectRooms(Room a, Room b)
    {
        _graph.AddEdge(a, b);
    }

    protected void ConnectRooms(Room a, Room b, Room corridor)
    {
        _graph.AddEdge(a, corridor);
        _graph.AddEdge(corridor, b);
    }

    protected List<Room> GetFreeCorridors()
    {
        List<Room> res = [];
        foreach (var room in _graph.Vertices)
        {
            if (room.RoomType == RoomType.Corridor)
            {
                if (!_graph.GetNeighbours(room).Any())
                {
                    res.Add(room);
                }
            }
        }

        return res;
    }

    protected List<Room> GetFreeConnectors()
    {
        List<Room> res = [];
        foreach (var room in _graph.Vertices)
        {
            if (room.RoomType == RoomType.Connector)
            {
                if (_graph.GetNeighbours(room).Count() < MaxConnectorSlots)
                {
                    res.Add(room);
                }
            }
        }

        return res;
    }

    protected List<Room> GetRoomsOfType(RoomType roomType)
    {
        List<Room> res = [];
        foreach (var room in _graph.Vertices)
        {
            if (room.RoomType == roomType)
            {
                res.Add(room);
            }
        }

        return res;
    }
}