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

    public void ConnectHeavyGates()
    {
        Random rand = new Random(this.SavedSeed + 300);
        // Collect all gates
        List<Room> gates = GetRoomsOfType(RoomType.GateHor);
        gates.AddRange(GetRoomsOfType(RoomType.GateVer));

        List<Room> connectors = GetFreeConnectors();
        List<Room> corridors = GetFreeCorridors();

        foreach (Room gate in gates)
        {
            Room randConnector = connectors[rand.Next(connectors.Count)];
            if (rand.Next(0, 2) == 0)
            {
                Room randCorridor = corridors[rand.Next(corridors.Count)];
                ConnectRooms(gate, randConnector, randCorridor);
                corridors.Remove(randCorridor);
            }
            else
            {
                ConnectRooms(gate, randConnector);
            }

            corridors.Remove(randConnector);
        }
    }


    protected void InitializeRooms()
    {
        _roomCounter = 0;
        _graph = new();
        AddRooms(RoomType.Keter, KetersNum);
        AddRooms(RoomType.Euclid, EuclidNum);
        AddRooms(RoomType.Safe, SafeNum);
        AddRooms(RoomType.Misc, MiscNum);
        AddRooms(RoomType.Connector, ConnectorsNum);
        AddRooms(RoomType.Corridor, _corridorsNum);
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