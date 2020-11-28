using HullDelaunayVoronoi.Primitives;
using System.Collections.Generic;

namespace HullDelaunayVoronoi.Hull
{

    public interface IConvexHull<VERTEX>
        where VERTEX : IVertex, new ()
    {
        int Dimension
        {
            get;
        }
        
        IList<VERTEX> Vertices
        {
            get;
        }
        
        IList<Simplex<VERTEX>> Simplexs
        {
            get;
        }
        
        float[] Centroid
        {
            get;
        }
        
        void Clear();
        
        bool Contains(VERTEX vertex);
        
        void Generate(IList<VERTEX> input, bool assignIds = true, bool checkInput = false);
    }
}

