using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Primitives;
using System.Collections.Generic;

namespace HullDelaunayVoronoi.Voronoi
{
    public interface IVoronoiMesh<VERTEX>
        where VERTEX : class, IVertex, new ()
    {
        int Dimension
        {
            get;
        }
        
        IList<IDelaunayCell<VERTEX>> Cells
        {
            get;
        }
        
        IList<IVoronoiRegion<VERTEX>> Regions
        {
            get;
        }
        
        void Clear();
        
        void Generate(IList<VERTEX> input, bool assignIds = true, bool checkInput = false);
    }

    public interface IDecoratedVoronoiMesh<VERTEX, REGIONDATA, EDGEDATA, CELLDATA> : IVoronoiMesh<VERTEX>
        where VERTEX : class, IVertex, new ()
        where REGIONDATA : new ()
            where EDGEDATA : new ()
            where CELLDATA : new ()
    {
        IList<IDecoratedDelaunayCell<VERTEX, CELLDATA>> DecoratedCells
        {
            get;
        }
        
        IList<IDecoratedVoronoiRegion<VERTEX, REGIONDATA, EDGEDATA, CELLDATA>> DecoratedRegions
        {
            get;
        }
    }
}












