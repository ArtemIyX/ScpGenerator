using Edgar.GraphBasedGenerator.Grid2D;
using Edgar.Graphs;
using ScpGeneration.Rooms.Descriptions.Heavy;

namespace ScpGeneration.Generators;

public class HeavyGenerator(int seed) : Generator<int>(seed)
{
    public override LevelDescriptionGrid2D<int> GetLevelDescription()
    {
        LevelDescriptionGrid2D<int> levelDescription = new LevelDescriptionGrid2D<int>();
        var graph = new UndirectedAdjacencyListGraph<int>();
        graph.AddVerticesRange(0, 3);
        graph.AddEdge(0, 1);
        graph.AddEdge(0, 2);

        RoomDescriptionGrid2D heavyRooms = new HeavyRooms().Get();
        foreach (var room in graph.Vertices)
        {
            levelDescription.AddRoom(room, heavyRooms);
        }

        var corridorCounter = graph.VerticesCount;

        RoomDescriptionGrid2D corridors = new HeavyCorridors().Get();
        foreach (var connection in graph.Edges)
        {
            // We manually insert a new room between each pair of neighboring rooms in the graph
            levelDescription.AddRoom(corridorCounter, corridors);
            // And instead of connecting the rooms directly, we connect them to the corridor room
            levelDescription.AddConnection(connection.From, corridorCounter);
            levelDescription.AddConnection(connection.To, corridorCounter);
            corridorCounter++;
        }

        return levelDescription;
    }
}