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
    private const int ConnectorsNum = 8;
    private readonly Range<int> _corridorsRange = new(16, 21);
    private readonly int _corridorsNum;
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
        _corridorsNum = new Random(seed + 100).Next(_corridorsRange.Minimum, _corridorsRange.Maximum);
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
        InitializeRooms();
        ConnectHeavyGates();
        CreateConnectors();
        _graph.RemoveIsolatedVertices();
        List<Room> connectors = GetRoomsOfType(RoomType.Connector);
        for (int i = 0; i < connectors.Count - 1; ++i)
        {
            var cor = new Room(_roomCounter, RoomType.Corridor);
            _roomCounter++;
            _graph.AddVertex(cor);
            ConnectRooms(connectors[i], connectors[i + 1], cor);
        }

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

    protected void CreateConnectors()
    {
        Random random = new Random(SavedSeed + 501);
        GenerateConnector(random);
    }

    protected Room GenerateConnector(Random random)
    {
        Room connector = new Room(_roomCounter++, RoomType.Connector);
        _graph.AddVertex(connector);
        int maxDoors = 2;
        for (int i = 0; i < maxDoors; ++i)
        {
            Room corridor = new Room(_roomCounter++, RoomType.Corridor);
            _graph.AddVertex(corridor);
            Room scp = GenerateRandomScpRoom(random);
            ConnectRooms(connector, scp, corridor);
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

    protected void ConnectHeavyGates()
    {
        // Collect all gates
        List<Room> gates = GetRoomsOfType(RoomType.GateHor);
        gates.AddRange(GetRoomsOfType(RoomType.GateVer));

        foreach (Room gate in gates)
        {
            Room connector = new Room(_roomCounter++, RoomType.Connector);
            _graph.AddVertex(connector);

            Room corridor = new Room(_roomCounter++, RoomType.Corridor);
            _graph.AddVertex(corridor);
            ConnectRooms(gate, connector, corridor);
        }
    }


    protected void InitializeRooms()
    {
        _roomCounter = 0;
        _graph = new();
        AddRooms(RoomType.GateVer, GatesNum);
        AddRooms(RoomType.GateHor, GatesNum);
    }

    protected void AddRooms(RoomType roomType, int n)
    {
        for (int i = 0; i < n; ++i)
        {
            _graph.AddVertex(new Room(_roomCounter, roomType));
            _roomCounter++;
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

    List<Room> GetFreeCorridors()
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

    List<Room> GetFreeConnectors()
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