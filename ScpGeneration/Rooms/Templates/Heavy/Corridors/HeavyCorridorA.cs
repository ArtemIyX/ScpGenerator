using Edgar.Geometry;
using Edgar.GraphBasedGenerator.Common;
using Edgar.GraphBasedGenerator.Grid2D;

namespace ScpGeneration.Rooms.Templates.Heavy.Corridors;

/*
 * ====
 * |  |
 * ====
 */

public class HeavyCorridorA : IRoomTemplate
{
    public RoomTemplateGrid2D Get()
    {
        PolygonGrid2D? corridorOutline = PolygonGrid2D.GetRectangle(1, 1);
        ManualDoorModeGrid2D corridorDoors = new ManualDoorModeGrid2D([
                new DoorGrid2D(new Vector2Int(0, 0), new Vector2Int(0, 1)),
                new DoorGrid2D(new Vector2Int(1, 0), new Vector2Int(1, 1)),
            ]
        );

        return new RoomTemplateGrid2D(corridorOutline, corridorDoors,this.GetType().Name,
            RoomTemplateRepeatMode.AllowRepeat,
            allowedTransformations: TransformationGrid2DHelper.GetRotations());
    }
}