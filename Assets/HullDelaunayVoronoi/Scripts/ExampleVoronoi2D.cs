using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Primitives;
using HullDelaunayVoronoi.Voronoi;
using System.Collections.Generic;
using UnityEngine;

namespace HullDelaunayVoronoi
{
    public class ExampleVoronoi2D : MonoBehaviour
    {

        public int NumberOfVertices = 1000;

        public float size = 10;

        public int seed = 0;

        public bool drawUnboundedCells = false;

        public bool drawBox = false;

        public int relaxationIterations = 0;

        private IVoronoiMesh2<Vertex2> originalVoronoiMesh;

        private IVoronoiMesh2<Vertex2> currentVoronoiMesh;

        private IDelaunayTriangulation<Vertex2> currentDelaunayTriangulation;

        private Material lineMaterial;

        private bool firstTime;

        private int lastSeenRelaxationIterations;

        private void Start()
        {
            lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
            List<Vertex2> vertices = new List<Vertex2>();
            Random.InitState(seed);

            for (int i = 0; i < NumberOfVertices; ++i)
            {
                float x = size * Random.Range(-1.0f, 1.0f);
                float y = size * Random.Range(-1.0f, 1.0f);
                vertices.Add(new Vertex2(x, y));
            }

            currentVoronoiMesh = new VoronoiMesh2<Vertex2>();
            currentVoronoiMesh.Generate(vertices, out currentDelaunayTriangulation);
            originalVoronoiMesh = currentVoronoiMesh;
            firstTime = true;
        }

        private void Update()
        {
            if (firstTime || lastSeenRelaxationIterations != relaxationIterations)
            {
                currentVoronoiMesh = originalVoronoiMesh;

                for (int i = 0; i < relaxationIterations; ++i)
                {
                    LloydsRelaxation2.Iterate(currentVoronoiMesh, out currentVoronoiMesh, out currentDelaunayTriangulation);
                }

                firstTime = false;
                lastSeenRelaxationIterations = relaxationIterations;
            }
        }

        private void OnPostRender()
        {
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.MultMatrix(Camera.main.worldToCameraMatrix);
            GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
            lineMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(Color.red);

            foreach (var region in currentVoronoiMesh.Regions)
            {
                bool draw = true;

                foreach (var cell in region.Cells)
                {
                    if (!InBound(cell.CircumCenter))
                    {
                        draw = drawUnboundedCells;
                        break;
                    }
                }

                if (!draw)
                {
                    continue;
                }

                foreach (var edge in region.Edges)
                {
                    Vertex2 v0 = edge.From.CircumCenter;
                    Vertex2 v1 = edge.To.CircumCenter;
                    DrawLine(v0, v1);
                }
            }

            GL.End();
            GL.Begin(GL.QUADS);
            GL.Color(Color.yellow);

            foreach (var vertex in currentDelaunayTriangulation.Vertices)
            {
                DrawQuad(vertex);
            }

            GL.End();

            if (drawBox)
            {
                GL.Begin(GL.LINES);
                GL.Color(Color.blue);
                DrawLine(new Vertex2(size, size), new Vertex2(-size, size));
                DrawLine(new Vertex2(-size, size), new Vertex2(-size, -size));
                DrawLine(new Vertex2(-size, -size), new Vertex2(size, -size));
                DrawLine(new Vertex2(size, -size), new Vertex2(size, size));
                GL.End();
            }

            GL.PopMatrix();
        }

        private void DrawLine(Vertex2 v0, Vertex2 v1)
        {
            GL.Vertex3(v0.X, v0.Y, 0.0f);
            GL.Vertex3(v1.X, v1.Y, 0.0f);
        }

        private void DrawQuad(Vertex2 v)
        {
            float x = v.X;
            float y = v.Y;
            float s = 0.05f;
            GL.Vertex3(x + s, y + s, 0.0f);
            GL.Vertex3(x + s, y - s, 0.0f);
            GL.Vertex3(x - s, y - s, 0.0f);
            GL.Vertex3(x - s, y + s, 0.0f);
        }

        private bool InBound(Vertex2 v)
        {
            if (v.X < -size || v.X > size)
            {
                return false;
            }

            if (v.Y < -size || v.Y > size)
            {
                return false;
            }

            return true;
        }

    }

}



















