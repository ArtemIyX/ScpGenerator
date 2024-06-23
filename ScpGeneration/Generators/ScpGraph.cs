using Edgar.Graphs;

namespace ScpGeneration.Generators;

public class ScpGraph<T> : IGraph<T>
{
    private readonly Dictionary<T, List<T>> _adjacencyLists = new Dictionary<T, List<T>>();

    public bool IsDirected { get; }

    public IEnumerable<T> Vertices => (IEnumerable<T>)this._adjacencyLists.Keys;

    public IEnumerable<IEdge<T>> Edges => this.GetEdges();

    public int VerticesCount => this._adjacencyLists.Count;

    public void AddVertex(T vertex)
    {
        if (this._adjacencyLists.ContainsKey(vertex))
            throw new ArgumentException("Vertex already exists");
        this._adjacencyLists[vertex] = new List<T>();
    }

    public void AddEdge(T from, T to)
    {
        List<T> objList1;
        List<T> objList2;
        if (!this._adjacencyLists.TryGetValue(from, out objList1) || !this._adjacencyLists.TryGetValue(to, out objList2))
            throw new ArgumentException("One of the vertices does not exist");
        if (objList1.Contains(to))
            throw new ArgumentException("The edge was already added");
        objList1.Add(to);
        objList2.Add(from);
    }

    public IEnumerable<T> GetNeighbours(T vertex)
    {
        List<T> neighbours;
        if (!this._adjacencyLists.TryGetValue(vertex, out neighbours))
            throw new ArgumentException("The vertex does not exist");
        return (IEnumerable<T>)neighbours;
    }

    public bool HasEdge(T from, T to)
    {
        foreach (T neighbour in this.GetNeighbours(from))
        {
            if (neighbour.Equals((object)to))
                return true;
        }

        return false;
    }

    private IEnumerable<IEdge<T>> GetEdges()
    {
        HashSet<Tuple<T, T>> tupleSet = new HashSet<Tuple<T, T>>();
        List<IEdge<T>> edges = new List<IEdge<T>>();
        foreach (KeyValuePair<T, List<T>> adjacencyList in this._adjacencyLists)
        {
            T key = adjacencyList.Key;
            foreach (T to in adjacencyList.Value)
            {
                if (!tupleSet.Contains(Tuple.Create<T, T>(key, to)) && !tupleSet.Contains(Tuple.Create<T, T>(to, key)))
                {
                    edges.Add((IEdge<T>)new Edge<T>(key, to));
                    tupleSet.Add(Tuple.Create<T, T>(key, to));
                }
            }
        }

        return (IEnumerable<IEdge<T>>)edges;
    }

    public void RemoveIsolatedVertices()
    {
        // Create a list to store vertices that have no edges
        List<T> isolatedVertices = new List<T>();

        // Identify isolated vertices
        foreach (var vertex in this.Vertices)
        {
            if (this._adjacencyLists[vertex].Count == 0)
            {
                isolatedVertices.Add(vertex);
            }
        }

        // Remove isolated vertices and their associated edges
        foreach (var vertex in isolatedVertices)
        {
            // Remove vertex from adjacency list
            this._adjacencyLists.Remove(vertex);

            // Remove edges pointing to this vertex
            foreach (var otherVertex in this.Vertices)
            {
                this._adjacencyLists[otherVertex].Remove(vertex);
            }
        }
    }
}