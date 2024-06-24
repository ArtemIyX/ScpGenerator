using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Common;
using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates.Basic.Corridors;

public class CorridorG : RoomTemplate
{
    public override RoomTemplateGrid2D Get()
    {
        PolygonGrid2D? corridorOutline = new PolygonGrid2DBuilder()
            .AddPoint(0, 0)
            .AddPoint(0, 1)
            .AddPoint(7, 1)
            .AddPoint(7, 0)
            .Build();
        ManualDoorModeGrid2D corridorDoors = new ManualDoorModeGrid2D([
                new DoorGrid2D(new Vector2Int(0, 0), new Vector2Int(0, 1)), 
                new DoorGrid2D(new Vector2Int(7, 0), new Vector2Int(7, 1))
            ]
        );

        return new RoomTemplateGrid2D(corridorOutline, corridorDoors, GetName(),
            RoomTemplateRepeatMode.AllowRepeat,
            allowedTransformations: TransformationGrid2DHelper.GetRotations());
    }
}