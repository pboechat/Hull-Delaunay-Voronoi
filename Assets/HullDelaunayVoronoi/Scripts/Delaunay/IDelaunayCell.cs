using HullDelaunayVoronoi.Primitives;

namespace HullDelaunayVoronoi.Delaunay
{
    public interface IDelaunayCell<VERTEX>
        where VERTEX : class, IVertex, new ()
    {
        Simplex<VERTEX> Simplex
        {
            get;
        }
        VERTEX CircumCenter
        {
            get;
        }
        float Radius
        {
            get;
        }
    }

    public interface IDecoratedDelaunayCell<VERTEX, CELLDATA> : IDelaunayCell<VERTEX>
        where VERTEX : class, IVertex, new ()
    {
        CELLDATA UserData
        {
            get;
        }
    }
}
