using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Common;
using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates.Basic.Connectors;

public class ConnectorD : RoomTemplate
{
    public override RoomTemplateGrid2D Get()
    {
        PolygonGrid2D corridorOutline = new PolygonGrid2DBuilder()
            .AddPoint(0, 0)
            .AddPoint(0, 1)
            .AddPoint(6, 1)
            .AddPoint(6, 0)
            .Build();

        IDoorModeGrid2D corridorDoors = new ManualDoorModeGrid2D([
            new DoorGrid2D(new Vector2Int(0, 0), new Vector2Int(0, 1)),
            new DoorGrid2D(new Vector2Int(6, 0), new Vector2Int(6, 1)),
            new DoorGrid2D(new Vector2Int(2, 0), new Vector2Int(3, 0)),
            new DoorGrid2D(new Vector2Int(3, 1), new Vector2Int(4, 1)),
        ]);

        return new RoomTemplateGrid2D(corridorOutline, corridorDoors, GetName(),
            RoomTemplateRepeatMode.AllowRepeat,
            allowedTransformations: TransformationGrid2DHelper.GetRotations());
    }
}