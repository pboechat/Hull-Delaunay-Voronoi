using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Primitives;
using System;

namespace HullDelaunayVoronoi.Voronoi
{

    public class VoronoiEdge<VERTEX> : IVoronoiEdge<VERTEX>
        where VERTEX : class, IVertex, new ()
    {
        public IDelaunayCell<VERTEX> From
        {
            get;
            private set;
        }
        
        public IDelaunayCell<VERTEX> To
        {
            get;
            private set;
        }
        
        public VoronoiEdge(IDelaunayCell<VERTEX> from, IDelaunayCell<VERTEX> to)
        {
            From = from;
            To = to;
        }
        
        /// <summary>
        /// Are these keys equal.
        /// </summary>
        public static bool operator ==(VoronoiEdge<VERTEX> k1, VoronoiEdge<VERTEX> k2)
        {
            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(k1, k2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)k1 == null) || ((object)k2 == null))
            {
                return false;
            }

            return object.ReferenceEquals(k1.From, k2.To);
        }
        
        /// <summary>
        /// Are these keys not equal.
        /// </summary>
        public static bool operator !=(VoronoiEdge<VERTEX> k1, VoronoiEdge<VERTEX> k2)
        {
            return !(k1 == k2);
        }
        
        /// <summary>
        /// Is the key equal to another key.
        /// </summary>
        public override bool Equals(object o)
        {
            VoronoiEdge<VERTEX> k = o as VoronoiEdge<VERTEX>;
            return k != null && k == this;
        }
        
        /// <summary>
        /// Is the key equal to another key.
        /// </summary>
        public bool Equals(IVoronoiEdge<VERTEX> o)
        {
            VoronoiEdge<VERTEX> k = o as VoronoiEdge<VERTEX>;
            return k == this;
        }
        
        /// <summary>
        /// The keys hash code.
        /// </summary>
        public override int GetHashCode()
        {
            int hashcode = 23;
            hashcode = (hashcode * 37) + From.GetHashCode();
            hashcode = (hashcode * 37) + To.GetHashCode();
            return hashcode;
        }
    }

    public class DecoratedVoronoiEdge<VERTEX, EDGEDATA, CELLDATA> : VoronoiEdge<VERTEX>, IDecoratedVoronoiEdge<VERTEX, EDGEDATA, CELLDATA>
        where VERTEX : class, IVertex, new ()
        where EDGEDATA : new ()
            where CELLDATA : new ()
    {
        public EDGEDATA UserData
        {
            get;
            private set;
        }
        
        public IDecoratedDelaunayCell<VERTEX, CELLDATA> DecoratedFrom
        {
            get
            {
                return From as IDecoratedDelaunayCell<VERTEX, CELLDATA>;
            }
        }
        
        public IDecoratedDelaunayCell<VERTEX, CELLDATA> DecoratedTo
        {
            get
            {
                return To as IDecoratedDelaunayCell<VERTEX, CELLDATA>;
            }
        }
        
        public DecoratedVoronoiEdge(IDelaunayCell<VERTEX> from, IDelaunayCell<VERTEX> to) : base(from, to)
        {
            UserData = new EDGEDATA();
        }
        
        public bool Equals(IDecoratedVoronoiEdge<VERTEX, EDGEDATA, CELLDATA> o)
        {
            var k = o as DecoratedVoronoiEdge<VERTEX, EDGEDATA, CELLDATA>;
            return k != null && k == this;
        }
    }
}
