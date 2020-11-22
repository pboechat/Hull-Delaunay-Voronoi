using HullDelaunayVoronoi.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace HullDelaunayVoronoi.Delaunay
{
    public abstract class DelaunayTriangulation<VERTEX> : IDelaunayTriangulation<VERTEX>
        where VERTEX : class, IVertex, new ()
    {
        public int Dimension
        {
            get;
            private set;
        }
        
        public IList<VERTEX> Vertices
        {
            get;
            protected set;
        }
        
        public IList<IDelaunayCell<VERTEX>> Cells
        {
            get;
            protected set;
        }
        
        public VERTEX Centroid
        {
            get;
            private set;
        }
        
        public DelaunayTriangulation(int dimension)
        {
            Dimension = dimension;
            Vertices = new List<VERTEX>();
            Cells = new List<IDelaunayCell<VERTEX>>();
            Centroid = new VERTEX();
        }
        
        public virtual void Clear()
        {
            Cells.Clear();
            Vertices.Clear();
            Centroid = new VERTEX();
        }
        
        public abstract void Generate(IList<VERTEX> input, bool assignIds = true, bool checkInput = false);
    }
}











