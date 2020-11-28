using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Primitives;
using System.Collections.Generic;

namespace HullDelaunayVoronoi.Voronoi
{
    public class VoronoiMesh2<VERTEX> : VoronoiMesh<VERTEX>, IVoronoiMesh2<VERTEX>
        where VERTEX : class, IVertex, new ()
    {
        public VoronoiMesh2() : base(2)
        {
        }
        
        public override void Generate(IList<VERTEX> input, bool assignIds = true, bool checkInput = false)
        {
            var delaunay = new DelaunayTriangulation2<VERTEX>();
            Generate(input, delaunay, assignIds, checkInput);
        }
        
        public void Generate(IList<VERTEX> input, out IDelaunayTriangulation<VERTEX> delaunay, bool assignIds = true, bool checkInput = false)
        {
            delaunay = new DelaunayTriangulation2<VERTEX>();
            Generate(input, delaunay, assignIds, checkInput);
        }
    }

    public class DecoratedVoronoiMesh2<VERTEX, REGIONDATA, EDGEDATA, CELLDATA> : DecoratedVoronoiMesh<VERTEX, REGIONDATA, EDGEDATA, CELLDATA>, IDecoratedVoronoiMesh2<VERTEX, REGIONDATA, EDGEDATA, CELLDATA>
        where VERTEX : class, IVertex, new ()
        where REGIONDATA : new ()
            where EDGEDATA : new ()
            where CELLDATA : new ()
    {
        public DecoratedVoronoiMesh2() : base(2)
        {
        }
        
        public override void Generate(IList<VERTEX> input, bool assignIds = true, bool checkInput = false)
        {
            var delaunay = new DecoratedDelaunayTriangulation2<VERTEX, CELLDATA>();
            Generate(input, delaunay, assignIds, checkInput);
        }
        
        public void Generate(IList<VERTEX> input, out IDelaunayTriangulation<VERTEX> delaunay, bool assignIds = true, bool checkInput = false)
        {
            delaunay = new DecoratedDelaunayTriangulation2<VERTEX, CELLDATA>();
            Generate(input, delaunay, assignIds, checkInput);
        }
        
        public void Generate(IList<VERTEX> input, out IDecoratedDelaunayTriangulation<VERTEX, CELLDATA> delaunay, bool assignIds = true, bool checkInput = false)
        {
            delaunay = new DecoratedDelaunayTriangulation2<VERTEX, CELLDATA>();
            Generate(input, delaunay, assignIds, checkInput);
        }
    }
}












