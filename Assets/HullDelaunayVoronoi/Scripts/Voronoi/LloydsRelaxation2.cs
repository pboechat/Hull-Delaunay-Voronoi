using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Hull;
using HullDelaunayVoronoi.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HullDelaunayVoronoi.Voronoi
{
    public static class LloydsRelaxation2
    {
        private static IList<VERTEX> GetCentroids<VERTEX>(IVoronoiMesh2<VERTEX> voronoi)
        where VERTEX : class, IVertex, new ()
        {
            return voronoi.Regions
                // Exclude degenerate regions
                .Where(region => region.Edges.Count() > 2)
                // Get region centroids
                .Select(region =>
                    MathHelper<VERTEX>.GetCentroid(region.Edges
                        .Select(edge => edge.From.CircumCenter)
                        .ToList()))
                .ToList();
        }

        public static void Iterate<VERTEX>(IVoronoiMesh2<VERTEX> src, out IVoronoiMesh2<VERTEX> dst)
        where VERTEX : class, IVertex, new ()
        {
            dst = new VoronoiMesh2<VERTEX>();
            dst.Generate(GetCentroids(src));
        }

        public static void Iterate<VERTEX, REGIONDATA, EDGEDATA, CELLDATA>(IDecoratedVoronoiMesh2<VERTEX, REGIONDATA, EDGEDATA, CELLDATA> src, out IDecoratedVoronoiMesh2<VERTEX, REGIONDATA, EDGEDATA, CELLDATA> dst)
        where VERTEX : class, IVertex, new ()
            where REGIONDATA : new ()
            where EDGEDATA : new ()
            where CELLDATA : new ()
        {
            dst = new DecoratedVoronoiMesh2<VERTEX, REGIONDATA, EDGEDATA, CELLDATA>();
            dst.Generate(GetCentroids(src));
        }

        public static void Iterate<VERTEX>(IVoronoiMesh2<VERTEX> src, out IVoronoiMesh2<VERTEX> dst1, out IDelaunayTriangulation<VERTEX> dst2)
        where VERTEX : class, IVertex, new ()
        {
            dst1 = new VoronoiMesh2<VERTEX>();
            dst1.Generate(GetCentroids(src), out dst2);
        }

        public static void Iterate<VERTEX, REGIONDATA, EDGEDATA, CELLDATA>(IDecoratedVoronoiMesh2<VERTEX, REGIONDATA, EDGEDATA, CELLDATA> src, out IDecoratedVoronoiMesh2<VERTEX, REGIONDATA, EDGEDATA, CELLDATA> dst1, out IDecoratedDelaunayTriangulation<VERTEX, CELLDATA> dst2)
        where VERTEX : class, IVertex, new ()
            where REGIONDATA : new ()
            where EDGEDATA : new ()
            where CELLDATA : new ()
        {
            dst1 = new DecoratedVoronoiMesh2<VERTEX, REGIONDATA, EDGEDATA, CELLDATA>();
            dst1.Generate(GetCentroids(src), out dst2);
        }
    }
}
