using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace HullDelaunayVoronoi.Voronoi
{
    public class VoronoiRegion<VERTEX> : IVoronoiRegion<VERTEX>
        where VERTEX : class, IVertex, new ()
    {
        public int Id
        {
            get;
            set;
        }
        
        public IList<IDelaunayCell<VERTEX>> Cells
        {
            get;
            private set;
        }
        
        public IList<IVoronoiEdge<VERTEX>> Edges
        {
            get;
            private set;
        }
        
        public VoronoiRegion()
        {
            Cells = new List<IDelaunayCell<VERTEX>>();
            Edges = new List<IVoronoiEdge<VERTEX>>();
        }
        
        public override string ToString()
        {
            return string.Format("[VoronoiRegion: Id={0}, Cells={1}, Edges={2}]", Id, Cells.Count, Edges.Count);
        }
    }

    public class DecoratedVoronoiRegion<VERTEX, REGIONDATA, EDGEDATA, CELLDATA> : VoronoiRegion<VERTEX>, IDecoratedVoronoiRegion<VERTEX, REGIONDATA, EDGEDATA, CELLDATA>
        where VERTEX : class, IVertex, new ()
        where REGIONDATA : new ()
            where EDGEDATA : new ()
            where CELLDATA : new ()
    {
        public REGIONDATA UserData
        {
            get;
            private set;
        }
        
        public IList<IDecoratedDelaunayCell<VERTEX, CELLDATA>> DecoratedCells
        {
            get
            {
                return Cells.Cast<IDecoratedDelaunayCell<VERTEX, CELLDATA>>().ToList();
            }
        }
        
        public IList<IDecoratedVoronoiEdge<VERTEX, EDGEDATA, CELLDATA>> DecoratedEdges
        {
            get
            {
                return Edges.Cast<IDecoratedVoronoiEdge<VERTEX, EDGEDATA, CELLDATA>>().ToList();
            }
        }
        
        public DecoratedVoronoiRegion() : base()
        {
            UserData = new REGIONDATA();
        }
        public override string ToString()
        {
            return string.Format("[VoronoiRegion: Id={0}, Cells={1}, Edges={2}, UserData={3}]", Id, Cells.Count, Edges.Count, UserData);
        }
    }
}
