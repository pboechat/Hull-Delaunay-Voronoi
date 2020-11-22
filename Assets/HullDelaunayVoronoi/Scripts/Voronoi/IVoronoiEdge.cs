using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Primitives;
using System;

namespace HullDelaunayVoronoi.Voronoi
{
    public interface IVoronoiEdge<VERTEX> : IEquatable<IVoronoiEdge<VERTEX>>
        where VERTEX : class, IVertex, new ()
    {
        IDelaunayCell<VERTEX> From
        {
            get;
        }
        
        IDelaunayCell<VERTEX> To
        {
            get;
        }
    }

    public interface IDecoratedVoronoiEdge<VERTEX, EDGEDATA, CELLDATA> : IEquatable<IDecoratedVoronoiEdge<VERTEX, EDGEDATA, CELLDATA>>, IVoronoiEdge<VERTEX>
        where VERTEX : class, IVertex, new ()
        where EDGEDATA : new ()
            where CELLDATA : new ()
    {
        EDGEDATA UserData
        {
            get;
        }
        
        IDecoratedDelaunayCell<VERTEX, CELLDATA> DecoratedFrom
        {
            get;
        }
        
        IDecoratedDelaunayCell<VERTEX, CELLDATA> DecoratedTo
        {
            get;
        }
    }
}
