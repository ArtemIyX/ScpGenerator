using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Common;
using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates.Basic.Gates;

public class GateHorizontal : RoomTemplate
{
    public override RoomTemplateGrid2D Get()
    {
        PolygonGrid2D? outline = new PolygonGrid2DBuilder()
            .AddPoint(0, 0)
            .AddPoint(0, 4)
            .AddPoint(6, 4)
            .AddPoint(6, 0)
            .Build();
        ManualDoorModeGrid2D doors = new ManualDoorModeGrid2D([
                new DoorGrid2D(new Vector2Int(0, 3), new Vector2Int(0, 4)),
                new DoorGrid2D(new Vector2Int(6, 3), new Vector2Int(6, 4)),
            ]
        );

        return new RoomTemplateGrid2D(outline, doors, GetName(),
            RoomTemplateRepeatMode.AllowRepeat,
            allowedTransformations: [TransformationGrid2D.Identity]);
    }
}