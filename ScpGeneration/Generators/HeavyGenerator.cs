using Edgar.GraphBasedGenerator.Grid2D;
using Edgar.Graphs;
using ScpGeneration.Rooms.Descriptions.Heavy;

namespace ScpGeneration.Generators;

public class HeavyGenerator(int seed) : Generator<int>(seed)
{
    public static Edgar.Graphs.UndirectedAdjacencyListGraph<int> GenerateRandomGraph(int n, int m)
    {
        var random = new Random();
        var graph = new Edgar.Graphs.UndirectedAdjacencyListGraph<int>();

        // Add all vertices
        for (int i = 0; i < n; i++)
        {
            graph.AddVertex(i);
        }

        // Add random edges
        for (int i = 0; i < n; i++)
        {
            int connections = random.Next(1, m + 1); // Number of connections for this vertex

            var potentialVertices = Enumerable.Range(0, n).Where(v => v != i).ToList();

            for (int j = 0; j < connections; j++)
            {
                if (potentialVertices.Count == 0)
                    break;

                int randomIndex = random.Next(potentialVertices.Count);
                int vertexToConnect = potentialVertices[randomIndex];
                potentialVertices.RemoveAt(randomIndex);

                if (!graph.HasEdge(i, vertexToConnect))
                {
                    graph.AddEdge(i, vertexToConnect);
                }
            }
        }

        return graph;
    }
    public override LevelDescriptionGrid2D<int> GetLevelDescription()
    {
        LevelDescriptionGrid2D<int> levelDescription = new LevelDescriptionGrid2D<int>();
        RoomDescriptionGrid2D heavyRooms = new HeavyRooms().Get();
        RoomDescriptionGrid2D heavyConnectors = new HeavyConnectors().Get();
        RoomDescriptionGrid2D corridors = new HeavyCorridors().Get();
        RoomDescriptionGrid2D vertGates = new HeavyVerticalGates().Get();
        RoomDescriptionGrid2D horGates = new HeavyHorizontalGates().Get();
        levelDescription.AddRoom(1, heavyRooms);
        levelDescription.AddRoom(2, heavyRooms);
        levelDescription.AddRoom(3, heavyRooms);
        levelDescription.AddRoom(4, heavyRooms);
        levelDescription.AddRoom(0, heavyConnectors);
        
        levelDescription.AddConnection(1, 0);
        levelDescription.AddConnection(2, 0);
        levelDescription.AddConnection(3, 0);
        levelDescription.AddConnection(4, 0);
        /*UndirectedAdjacencyListGraph<int> graph = GenerateRandomGraph(2, 1);
        
        foreach (var room in graph.Vertices)
        {
            levelDescription.AddRoom(room, heavyRooms);
        }

        var corridorCounter = graph.VerticesCount;

       
        foreach (var connection in graph.Edges)
        {
            // We manually insert a new room between each pair of neighboring rooms in the graph
            levelDescription.AddRoom(corridorCounter, corridors);
            // And instead of connecting the rooms directly, we connect them to the corridor room
            levelDescription.AddConnection(connection.From, corridorCounter);
            levelDescription.AddConnection(connection.To, corridorCounter);
            corridorCounter++;
        }*/

        //levelDescription.MinimumRoomDistance = 1;

        return levelDescription;
    }
}