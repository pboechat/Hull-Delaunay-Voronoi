using HullDelaunayVoronoi.Primitives;
using System.Collections.Generic;

namespace HullDelaunayVoronoi.Delaunay
{
    public interface IDelaunayTriangulation<VERTEX>
        where VERTEX : class, IVertex, new ()
    {
        int Dimension
        {
            get;
        }
        
        IList<VERTEX> Vertices
        {
            get;
        }
        
        IList<IDelaunayCell<VERTEX>> Cells
        {
            get;
        }
        
        VERTEX Centroid
        {
            get;
        }
        
        void Clear();
        
        void Generate(IList<VERTEX> input, bool assignIds = true, bool checkInput = false);
    }
    
    public interface IDecoratedDelaunayTriangulation<VERTEX, CELLDATA> : IDelaunayTriangulation<VERTEX>
        where VERTEX : class, IVertex, new ()
        where CELLDATA : new ()
    {
        IList<IDecoratedDelaunayCell<VERTEX, CELLDATA>> DecoratedCells
        {
            get;
        }
    }
}












