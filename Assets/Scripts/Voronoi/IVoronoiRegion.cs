using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Primitives;
using System.Collections.Generic;

namespace HullDelaunayVoronoi.Voronoi
{
    public interface IVoronoiRegion<VERTEX>
        where VERTEX : class, IVertex, new ()
    {
        int Id
        {
            get;
            set;
        }
        
        IList<IDelaunayCell<VERTEX>> Cells
        {
            get;
        }
        
        IList<IVoronoiEdge<VERTEX>> Edges
        {
            get;
        }
    }

    public interface IDecoratedVoronoiRegion<VERTEX, REGIONDATA, EDGEDATA, CELLDATA> : IVoronoiRegion<VERTEX>
        where VERTEX : class, IVertex, new ()
        where REGIONDATA : new ()
            where EDGEDATA : new ()
            where CELLDATA : new ()
    {
        REGIONDATA UserData
        {
            get;
        }
        
        IList<IDecoratedDelaunayCell<VERTEX, CELLDATA>> DecoratedCells
        {
            get;
        }
        
        IList<IDecoratedVoronoiEdge<VERTEX, EDGEDATA, CELLDATA>> DecoratedEdges
        {
            get;
        }
    }
}
