using Edgar.GraphBasedGenerator.Grid2D;
using Edgar.GraphBasedGenerator.Grid2D.Drawing;
using Edgar.Graphs;
using ScpGeneration.Data;
using ScpGeneration.Rooms.Descriptions.Heavy;
using ScpGeneration.Rooms.Descriptions.Heavy.SCP;

namespace ScpGeneration.Generators;

public class HeavyGenerator : Generator<int>
{
    private const int KetersNum = 2;
    private const int EuclidNum = 6;
    private const int SafeNum = 2;
    private const int MiscNum = 2;
    private const int ConnectorsNum = 8;
    private readonly Range<int> _corridorsRange = new(16, 21);
    private readonly int _corridorsNum;
    private const int GatesNum = 2;

    readonly RoomDescriptionGrid2D _miscRooms = new HeavyRooms().Get();
    readonly RoomDescriptionGrid2D _heavyConnectors = new HeavyConnectors().Get();
    readonly RoomDescriptionGrid2D _corridors = new HeavyCorridors().Get();
    readonly RoomDescriptionGrid2D _vertGates = new HeavyVerticalGates().Get();
    readonly RoomDescriptionGrid2D _horGates = new HeavyHorizontalGates().Get();

    readonly RoomDescriptionGrid2D _safe = new HeavySafe().Get();
    readonly RoomDescriptionGrid2D _euclid = new HeavyEuclid().Get();
    readonly RoomDescriptionGrid2D _keter = new HeavyKeter().Get();

    private int _roomCounter;
    private LevelDescriptionGrid2D<int> _levelDescription;
    private Dictionary<int, RoomType> _roomTypes;

    public HeavyGenerator(int seed) : base(seed)
    {
        _corridorsNum = new Random(seed + 100).Next(_corridorsRange.Minimum, _corridorsRange.Maximum);
        _roomCounter = 0;
        _levelDescription = new LevelDescriptionGrid2D<int>();
        _roomTypes = [];
    }

    public override LevelDescriptionGrid2D<int> GetLevelDescription()
    {
        InitializeRooms();
        return _levelDescription;
    }

    protected void InitializeRooms()
    {
        _levelDescription = new LevelDescriptionGrid2D<int>();
        _roomCounter = 0;
        _roomTypes = [];
        AddRooms(RoomType.Keter, KetersNum, _keter);
        AddRooms(RoomType.Euclid, EuclidNum, _euclid);
        AddRooms(RoomType.Safe, SafeNum, _safe);
        AddRooms(RoomType.Misc, MiscNum, _miscRooms);
        AddRooms(RoomType.Connector, ConnectorsNum, _heavyConnectors);
        AddRooms(RoomType.Corridor, _corridorsNum, _corridors);
        AddRooms(RoomType.GateVer, GatesNum, _vertGates);
        AddRooms(RoomType.GateHor, GatesNum, _horGates);
    }

    protected void AddRooms(RoomType roomType, int n, RoomDescriptionGrid2D desc)
    {
        for (int i = 0; i < n; ++i)
        {
            _levelDescription.AddRoom(_roomCounter++, desc);
            _roomTypes.TryAdd(_roomCounter, roomType);
        }
    }

    protected RoomType GetRoomType(int id)
    {
        return _roomTypes[id];
    }

    public RoomId MakeRoomId(int id, string name)
    {
        return new RoomId(id, name);
    }
}