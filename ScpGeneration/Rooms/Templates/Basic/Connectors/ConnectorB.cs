using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Common;
using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates.Basic.Connectors;

/*
 * + Cross Shape
 */
public class ConnectorB : RoomTemplate
{
    public override RoomTemplateGrid2D Get()
    {
        PolygonGrid2D corridorOutline = new PolygonGrid2DBuilder()
            .AddPoint(0, 0)
            .AddPoint(0, 1)
            .AddPoint(1, 1)
            .AddPoint(1, 2)
            .AddPoint(2, 2)
            .AddPoint(2, 3)
            .AddPoint(3, 3)
            .AddPoint(3, 2)
            .AddPoint(4, 2)
            .AddPoint(4, 1)
            .AddPoint(5, 1)
            .AddPoint(5, 0)
            .AddPoint(4, 0)
            .AddPoint(4, -1)
            .AddPoint(3, -1)
            .AddPoint(3, -2)
            .AddPoint(2, -2)
            .AddPoint(2, -1)
            .AddPoint(1, -1)
            .AddPoint(1, 0)
            .Build();

        IDoorModeGrid2D corridorDoors = new ManualDoorModeGrid2D([
            new DoorGrid2D(new Vector2Int(0, 0), new Vector2Int(0, 1)),
            new DoorGrid2D(new Vector2Int(2, 3), new Vector2Int(3, 3)),
            new DoorGrid2D(new Vector2Int(5, 0), new Vector2Int(5, 1)),
            new DoorGrid2D(new Vector2Int(2, -2), new Vector2Int(3, -2)),
        ]);

        return new RoomTemplateGrid2D(corridorOutline, corridorDoors, GetName(),
            RoomTemplateRepeatMode.AllowRepeat,
            allowedTransformations: TransformationGrid2DHelper.GetRotations());
    }
}