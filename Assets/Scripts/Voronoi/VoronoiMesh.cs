using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HullDelaunayVoronoi.Voronoi
{
    public abstract class VoronoiMesh<VERTEX> : IVoronoiMesh<VERTEX>
        where VERTEX : class, IVertex, new ()
    {
        public int Dimension
        {
            get;
            private set;
        }
        
        public IList<IDelaunayCell<VERTEX>> Cells
        {
            get;
            private set;
        }
        
        public IList<IVoronoiRegion<VERTEX>> Regions
        {
            get;
            private set;
        }
        
        public VoronoiMesh(int dimension)
        {
            Dimension = dimension;
            Cells = new List<IDelaunayCell<VERTEX>>();
            Regions = new List<IVoronoiRegion<VERTEX>>();
        }
        
        public virtual void Clear()
        {
            Cells.Clear();
            Regions.Clear();
        }
        
        protected virtual IVoronoiRegion<VERTEX> CreateRegion()
        {
            return new VoronoiRegion<VERTEX>();
        }
        
        protected virtual IVoronoiEdge<VERTEX> CreateEdge(IDelaunayCell<VERTEX> from, IDelaunayCell<VERTEX> to)
        {
            return new VoronoiEdge<VERTEX>(from, to);
        }
        
        public abstract void Generate(IList<VERTEX> input, bool assignIds = true, bool checkInput = false);
        
        protected virtual void Generate(IList<VERTEX> input, IDelaunayTriangulation<VERTEX> delaunay, bool assignIds, bool checkInput = false)
        {
            Clear();
            delaunay.Generate(input, assignIds, checkInput);

            for (int i = 0; i < delaunay.Vertices.Count; i++)
            {
                delaunay.Vertices[i].Tag = i;
            }

            for (int i = 0; i < delaunay.Cells.Count; i++)
            {
                delaunay.Cells[i].CircumCenter.Id = i;
                delaunay.Cells[i].Simplex.Tag = i;
                Cells.Add(delaunay.Cells[i]);
            }

            var cells = new List<IDelaunayCell<VERTEX>>();
            var neighbourCell = new Dictionary<int, IDelaunayCell<VERTEX>>();

            for (int i = 0; i < delaunay.Vertices.Count; i++)
            {
                cells.Clear();
                VERTEX vertex = delaunay.Vertices[i];

                for (int j = 0; j < delaunay.Cells.Count; j++)
                {
                    Simplex<VERTEX> simplex = delaunay.Cells[j].Simplex;

                    for (int k = 0; k < simplex.Vertices.Length; k++)
                    {
                        if (simplex.Vertices[k].Tag == vertex.Tag)
                        {
                            cells.Add(delaunay.Cells[j]);
                            break;
                        }
                    }
                }

                if (cells.Count > 0)
                {
                    var region = CreateRegion();

                    for (int j = 0; j < cells.Count; j++)
                    {
                        region.Cells.Add(cells[j]);
                    }

                    neighbourCell.Clear();

                    for (int j = 0; j < cells.Count; j++)
                    {
                        neighbourCell.Add(cells[j].CircumCenter.Id, cells[j]);
                    }

                    for (int j = 0; j < cells.Count; j++)
                    {
                        Simplex<VERTEX> simplex = cells[j].Simplex;

                        for (int k = 0; k < simplex.Adjacent.Length; k++)
                        {
                            if (simplex.Adjacent[k] == null)
                            {
                                continue;
                            }

                            int tag = simplex.Adjacent[k].Tag;

                            if (neighbourCell.ContainsKey(tag))
                            {
                                region.Edges.Add(CreateEdge(cells[j], neighbourCell[tag]));
                            }
                        }
                    }

                    region.Id = Regions.Count;
                    Regions.Add(region);
                }
            }
        }
    }

    public abstract class DecoratedVoronoiMesh<VERTEX, REGIONDATA, EDGEDATA, CELLDATA> : VoronoiMesh<VERTEX>, IDecoratedVoronoiMesh<VERTEX, REGIONDATA, EDGEDATA, CELLDATA>
        where VERTEX : class, IVertex, new ()
        where REGIONDATA: new ()
            where EDGEDATA : new ()
            where CELLDATA : new ()
    {
        public IList<IDecoratedDelaunayCell<VERTEX, CELLDATA>> DecoratedCells
        {
            get
            {
                return Cells.Cast<IDecoratedDelaunayCell<VERTEX, CELLDATA>>().ToList();
            }
        }
        
        public IList<IDecoratedVoronoiRegion<VERTEX, REGIONDATA, EDGEDATA, CELLDATA>> DecoratedRegions
        {
            get
            {
                return Regions.Cast<IDecoratedVoronoiRegion<VERTEX, REGIONDATA, EDGEDATA, CELLDATA>>().ToList();
            }
        }
        
        public DecoratedVoronoiMesh(int dimension) : base(dimension)
        {
        }
        
        protected override void Generate(IList<VERTEX> input, IDelaunayTriangulation<VERTEX> delaunay, bool assignIds, bool checkInput = false)
        {
            if (delaunay as IDecoratedDelaunayTriangulation<VERTEX, CELLDATA> == null)
            {
                // TODO:
                throw new ArgumentException();
            }

            base.Generate(input, delaunay, assignIds, checkInput);
        }
        
        protected override IVoronoiRegion<VERTEX> CreateRegion()
        {
            return new DecoratedVoronoiRegion<VERTEX, REGIONDATA, EDGEDATA, CELLDATA>();
        }
        
        protected override IVoronoiEdge<VERTEX> CreateEdge(IDelaunayCell<VERTEX> from, IDelaunayCell<VERTEX> to)
        {
            if (from as IDecoratedDelaunayCell<VERTEX, CELLDATA> == null
                || to as IDecoratedDelaunayCell<VERTEX, CELLDATA> == null)
            {
                // TODO:
                throw new ArgumentException();
            }

            return new DecoratedVoronoiEdge<VERTEX, EDGEDATA, CELLDATA>(from, to);
        }
    }
}












