using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Primitives;
using System.Collections.Generic;

namespace HullDelaunayVoronoi.Voronoi
{
    public interface IVoronoiMesh2<VERTEX> : IVoronoiMesh<VERTEX>
        where VERTEX : class, IVertex, new ()
    {
        void Generate(IList<VERTEX> input, out IDelaunayTriangulation<VERTEX> delaunay, bool assignIds = true, bool checkInput = false);
    }

    public interface IDecoratedVoronoiMesh2<VERTEX, REGIONDATA, EDGEDATA, CELLDATA> : IVoronoiMesh2<VERTEX>, IDecoratedVoronoiMesh<VERTEX, REGIONDATA, EDGEDATA, CELLDATA>
        where VERTEX : class, IVertex, new ()
        where REGIONDATA : new ()
            where EDGEDATA : new ()
            where CELLDATA : new ()
    {
        void Generate(IList<VERTEX> input, out IDecoratedDelaunayTriangulation<VERTEX, CELLDATA> delaunay, bool assignIds = true, bool checkInput = false);
    }
}
