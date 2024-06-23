using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Common;
using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates.Basic.Corridors;


public class CorridorB : RoomTemplate
{
    public override RoomTemplateGrid2D Get()
    {
        PolygonGrid2D? corridorOutline = PolygonGrid2D.GetRectangle(2, 1);
        ManualDoorModeGrid2D corridorDoors = new ManualDoorModeGrid2D([
                new DoorGrid2D(new Vector2Int(0, 0), new Vector2Int(0, 1)),
                new DoorGrid2D(new Vector2Int(2, 0), new Vector2Int(2, 1)),
            ]
        );

        return new RoomTemplateGrid2D(corridorOutline, corridorDoors, GetName(),
            RoomTemplateRepeatMode.AllowRepeat,
            allowedTransformations: TransformationGrid2DHelper.GetRotations());
    }
}