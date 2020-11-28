using HullDelaunayVoronoi.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HullDelaunayVoronoi.Hull
{
    public class MathHelper2 : MathHelper<Vertex2>
    {
        private MathHelper2()
        {
        }
        
        public static IList<Vertex2> OrderClockwise(IList<Vertex2> vertices)
        {
            var orderedVertices = new List<Vertex2>();
            var bottomLeft = vertices[0];

            for (int i = 1; i < vertices.Count(); ++i)
            {
                var vertex = vertices[i];

                if (bottomLeft.Position[0] < vertex.Position[0])
                {
                    continue;
                }

                if (bottomLeft.Position[0] == vertex.Position[0]
                    && bottomLeft.Position[1] < vertex.Position[1])
                {
                    continue;
                }

                bottomLeft = vertex;
            }

            orderedVertices.Add(bottomLeft);
            orderedVertices.AddRange(vertices
                .Where(vertex => vertex.Position[0] != bottomLeft.Position[0] || vertex.Position[1] != bottomLeft.Position[1])
                .OrderByDescending(vertex =>
            {
                var dir = Normalize(Subtract(vertex, bottomLeft));
                return Dot(dir, Vertex2.UnitY);
            }));
            return orderedVertices;
        }
    }

    /// <summary>
    /// A helper class mostly for normal computation. If convex hulls are computed
    /// in higher dimensions, it might be a good idea to add a specific
    /// FindNormalVectorND function.
    /// </summary>
    public class MathHelper<VERTEX>
        where VERTEX : class, IVertex, new ()
    {
        private static readonly float[] ntX = new float[4];
        
        private static readonly float[] ntY = new float[4];
        
        private static readonly float[] ntZ = new float[4];
        
        protected MathHelper()
        {
        }
        
        // Source: http://en.wikipedia.org/wiki/Centroid#Centroid_of_polygon
        public static VERTEX GetCentroid(IList<VERTEX> vertices)
        {
            var length = vertices.Count();
            int i = 0, j = length - 1;
            var doubleArea = 0.0f;
            var centroid = new VERTEX();

            while (i < length)
            {
                var v0 = vertices[j];
                var v1 = vertices[i];
                var cross = Cross(v0, v1);
                centroid = Add(centroid, MultiplyByScalar(Add(v0, v1), cross));
                doubleArea += cross;
                j = i++;
            }

            var m = 3.0f * doubleArea;

            if (m != 0)
            {
                return DivideByScalar(centroid, m);
            }

            else
            {
                throw new Exception("Cannot compute centroid of a degenerate polygon.");
            }
        }
        
        public static VERTEX Unit(int i)
        {
            var unit = new VERTEX();
            unit.Position[i] = 1;
            return unit;
        }
        
        /// <summary>
        /// Squared length of the vector.
        /// </summary>
        public static float LengthSquared(float[] x)
        {
            float norm = 0;

            for (int i = 0; i < x.Length; i++)
            {
                float t = x[i];
                norm += t * t;
            }

            return norm;
        }
        
        public static float Length(VERTEX v)
        {
            return (float)Math.Sqrt(SqrLength(v));
        }
        
        public static VERTEX Normalize(VERTEX v)
        {
            return DivideByScalar(v, Length(v));
        }
        
        public static VERTEX MultiplyByScalar(VERTEX v, float s)
        {
            var q = new VERTEX();

            for (int i = 0; i < v.Dimension; i++)
            {
                q.Position[i] = v.Position[i] * s;
            }

            return q;
        }
        
        public static VERTEX Add(VERTEX a, VERTEX b)
        {
            var c = new VERTEX();

            for (int i = 0; i < a.Dimension; i++)
            {
                c.Position[i] = a.Position[i] + b.Position[i];
            }

            return c;
        }
        
        public static VERTEX Subtract(VERTEX a, VERTEX b)
        {
            var c = new VERTEX();

            for (int i = 0; i < a.Dimension; i++)
            {
                c.Position[i] = a.Position[i] - b.Position[i];
            }

            return c;
        }
        
        public static VERTEX DivideByScalar(VERTEX v, float s)
        {
            var q = new VERTEX();

            for (int i = 0; i < v.Dimension; i++)
            {
                q.Position[i] = v.Position[i] / s;
            }

            return q;
        }
        
        public static float Cross(VERTEX a, VERTEX b)
        {
            if (a.Dimension == 2)
            {
                return Cross2(a as Vertex2, b as Vertex2);
            }

            throw new NotImplementedException();
        }
        
        private static float Cross2(Vertex2 a, Vertex2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
        
        public static float Dot(VERTEX a, VERTEX b)
        {
            float dot = 0;

            for (int i = 0; i < a.Dimension; i++)
            {
                dot += a.Position[i] * b.Position[i];
            }

            return dot;
        }
        
        public static double SqrLength(VERTEX p)
        {
            float sqrNorm = 0;

            for (int i = 0; i < p.Dimension; i++)
            {
                var t = p.Position[i];
                sqrNorm += t * t;
            }

            return sqrNorm;
        }
        
        /// <summary>
        /// Subtracts vectors x and y and stores the result to target.
        /// </summary>
        public static void SubtractFast(float[] x, float[] y, float[] target)
        {
            int d = x.Length;

            for (int i = 0; i < d; i++)
            {
                target[i] = x[i] - y[i];
            }
        }
        
        /// <summary>
        /// Finds 4D normal vector.
        /// </summary>
        private static void FindNormalVector4D(VERTEX[] vertices, float[] normal)
        {
            SubtractFast(vertices[1].Position, vertices[0].Position, ntX);
            SubtractFast(vertices[2].Position, vertices[1].Position, ntY);
            SubtractFast(vertices[3].Position, vertices[2].Position, ntZ);
            var x = ntX;
            var y = ntY;
            var z = ntZ;
            // This was generated using Mathematica
            var nx = x[3] * (y[2] * z[1] - y[1] * z[2])
                + x[2] * (y[1] * z[3] - y[3] * z[1])
                + x[1] * (y[3] * z[2] - y[2] * z[3]);
            var ny = x[3] * (y[0] * z[2] - y[2] * z[0])
                + x[2] * (y[3] * z[0] - y[0] * z[3])
                + x[0] * (y[2] * z[3] - y[3] * z[2]);
            var nz = x[3] * (y[1] * z[0] - y[0] * z[1])
                + x[1] * (y[0] * z[3] - y[3] * z[0])
                + x[0] * (y[3] * z[1] - y[1] * z[3]);
            var nw = x[2] * (y[0] * z[1] - y[1] * z[0])
                + x[1] * (y[2] * z[0] - y[0] * z[2])
                + x[0] * (y[1] * z[2] - y[2] * z[1]);
            float norm = (float)Math.Sqrt(nx * nx + ny * ny + nz * nz + nw * nw);
            float f = 1.0f / norm;
            normal[0] = f * nx;
            normal[1] = f * ny;
            normal[2] = f * nz;
            normal[3] = f * nw;
        }
        
        /// <summary>
        /// Finds 3D normal vector.
        /// </summary>
        private static void FindNormalVector3D(VERTEX[] vertices, float[] normal)
        {
            SubtractFast(vertices[1].Position, vertices[0].Position, ntX);
            SubtractFast(vertices[2].Position, vertices[1].Position, ntY);
            var x = ntX;
            var y = ntY;
            var nx = x[1] * y[2] - x[2] * y[1];
            var ny = x[2] * y[0] - x[0] * y[2];
            var nz = x[0] * y[1] - x[1] * y[0];
            float norm = (float)Math.Sqrt(nx * nx + ny * ny + nz * nz);
            float f = 1.0f / norm;
            normal[0] = f * nx;
            normal[1] = f * ny;
            normal[2] = f * nz;
        }
        
        /// <summary>
        /// Finds 2D normal vector.
        /// </summary>
        private static void FindNormalVector2D(VERTEX[] vertices, float[] normal)
        {
            SubtractFast(vertices[1].Position, vertices[0].Position, ntX);
            var x = ntX;
            var nx = -x[1];
            var ny = x[0];
            float norm = (float)Math.Sqrt(nx * nx + ny * ny);
            float f = 1.0f / norm;
            normal[0] = f * nx;
            normal[1] = f * ny;
        }
        
        /// <summary>
        /// Finds normal vector of a hyper-plane given by vertices.
        /// Stores the results to normalData.
        /// </summary>
        public static void FindNormalVector(VERTEX[] vertices, float[] normalData)
        {
            switch (vertices[0].Dimension)
            {
                case 2:
                    FindNormalVector2D(vertices, normalData);
                    return;

                case 3:
                    FindNormalVector3D(vertices, normalData);
                    return;

                case 4:
                    FindNormalVector4D(vertices, normalData);
                    return;
            }
        }
        
        /// <summary>
        /// Check if the vertex is "visible" from the face.
        /// The vertex is "over face" if the return value is > Constants.PlaneDistanceTolerance.
        /// </summary>
        /// <returns>The vertex is "over face" if the result is positive.</returns>
        internal static float GetVertexDistance(VERTEX v, SimplexWrap<VERTEX> f)
        {
            float[] normal = f.Normal;
            float[] p = v.Position;
            float distance = f.Offset;

            for (int i = 0; i < v.Dimension; i++)
            {
                distance += normal[i] * p[i];
            }

            return distance;
        }
        
        /// <summary>
        /// Check if the vertex is "visible" from the face.
        /// The vertex is "over face" if the return value is > Constants.PlaneDistanceTolerance.
        /// </summary>
        /// <returns>The vertex is "over face" if the result is positive.</returns>
        public static float GetVertexDistance(VERTEX v, Simplex<VERTEX> f)
        {
            float[] normal = f.Normal;
            float[] p = v.Position;
            float distance = f.Offset;

            for (int i = 0; i < v.Dimension; i++)
            {
                distance += normal[i] * p[i];
            }

            return distance;
        }
    }

}