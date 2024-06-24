using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Common;
using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates.Basic.Corridors;

public class CorridorF : RoomTemplate
{
    public override RoomTemplateGrid2D Get()
    {
        PolygonGrid2D? corridorOutline = new PolygonGrid2DBuilder()
            .AddPoint(0, 0)
            .AddPoint(0, 1)
            .AddPoint(3, 1)
            .AddPoint(3, 4)
            .AddPoint(4, 4)
            .AddPoint(4, 0)
            .Build();
        ManualDoorModeGrid2D corridorDoors = new ManualDoorModeGrid2D([
                new DoorGrid2D(new Vector2Int(0, 0), new Vector2Int(0, 1)), 
                new DoorGrid2D(new Vector2Int(3, 4), new Vector2Int(4, 4))
            ]
        );

        return new RoomTemplateGrid2D(corridorOutline, corridorDoors, GetName(),
            RoomTemplateRepeatMode.AllowRepeat,
            allowedTransformations: TransformationGrid2DHelper.GetRotations());
    }
}