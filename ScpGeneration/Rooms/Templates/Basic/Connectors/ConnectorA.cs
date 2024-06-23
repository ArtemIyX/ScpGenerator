using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Common;
using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates.Basic.Connectors;

/*
 * T shape
 */
public class ConnectorA : RoomTemplate
{
    public override RoomTemplateGrid2D Get()
    {
        PolygonGrid2D corridorOutline = new PolygonGrid2DBuilder()
            .AddPoint(0, 0)
            .AddPoint(0, 1)
            .AddPoint(2, 1)
            .AddPoint(2, 5)
            .AddPoint(3, 5)
            .AddPoint(3, 1)
            .AddPoint(5, 1)
            .AddPoint(5, 0)
            .Build();

        IDoorModeGrid2D corridorDoors = new ManualDoorModeGrid2D([
            new DoorGrid2D(new Vector2Int(0, 0), new Vector2Int(0, 1)),
            new DoorGrid2D(new Vector2Int(5, 0), new Vector2Int(5, 1)),
            new DoorGrid2D(new Vector2Int(2, 5), new Vector2Int(3, 5))
        ]);

        return new RoomTemplateGrid2D(corridorOutline, corridorDoors, GetName(),
            RoomTemplateRepeatMode.AllowRepeat,
            allowedTransformations: TransformationGrid2DHelper.GetRotations());
    }
}