
using HullDelaunayVoronoi.Primitives;

namespace HullDelaunayVoronoi.Delaunay
{
    public class DelaunayCell<VERTEX> : IDelaunayCell<VERTEX>
        where VERTEX : class, IVertex, new ()
    {
        public Simplex<VERTEX> Simplex
        {
            get;
            private set;
        }
        
        public VERTEX CircumCenter
        {
            get;
            private set;
        }
        
        public float Radius
        {
            get;
            private set;
        }
        
        public DelaunayCell(Simplex<VERTEX> simplex, float[] circumCenter, float radius)
        {
            Simplex = simplex;
            CircumCenter = new VERTEX
            {
                Position = circumCenter
            };
            Radius = radius;
        }
    }

    public class DecoratedDelaunayCell<VERTEX, CELLDATA> : DelaunayCell<VERTEX>, IDecoratedDelaunayCell<VERTEX, CELLDATA>
        where VERTEX : class, IVertex, new ()
        where CELLDATA : new ()
    {
        public CELLDATA UserData
        {
            get;
            private set;
        }
        
        public DecoratedDelaunayCell(Simplex<VERTEX> simplex, float[] circumCenter, float radius) : base(simplex, circumCenter, radius)
        {
            UserData = new CELLDATA();
        }
    }
}
