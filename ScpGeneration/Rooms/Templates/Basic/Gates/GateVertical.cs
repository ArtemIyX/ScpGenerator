using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Common;
using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates.Basic.Gates;

public class GateVertical : RoomTemplate
{
    public override RoomTemplateGrid2D Get()
    {
        PolygonGrid2D? outline = new PolygonGrid2DBuilder()
            .AddPoint(0, 0)
            .AddPoint(0, 5)
            .AddPoint(4, 5)
            .AddPoint(4, 0)
            .Build();
        ManualDoorModeGrid2D doors = new ManualDoorModeGrid2D([
                new DoorGrid2D(new Vector2Int(2, 0), new Vector2Int(3, 0)),
                new DoorGrid2D(new Vector2Int(2, 5), new Vector2Int(2, 5)),
            ]
        );

        return new RoomTemplateGrid2D(outline, doors, GetName(),
            RoomTemplateRepeatMode.AllowRepeat,
            allowedTransformations: [TransformationGrid2D.Identity]);
    }
}