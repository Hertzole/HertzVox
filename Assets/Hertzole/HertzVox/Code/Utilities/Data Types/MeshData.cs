using System.Collections.Generic;
using UnityEngine;

namespace Hertzole.HertzVox
{
    public class MeshData
    {
        private List<Vector3> m_Vertices = new List<Vector3>();
        public List<Vector3> Vertices { get { return m_Vertices; } set { m_Vertices = value; } }

        private List<Vector3> m_ColliderVertices = new List<Vector3>();
        public List<Vector3> ColliderVertices { get { return m_ColliderVertices; } set { m_ColliderVertices = value; } }

        private List<Vector2> m_Uv = new List<Vector2>();
        public List<Vector2> UV { get { return m_Uv; } set { m_Uv = value; } }

        private List<Color> m_Colors = new List<Color>();
        public List<Color> Colors { get { return m_Colors; } set { m_Colors = value; } }

        private List<int> m_Triangles = new List<int>();
        public List<int> Triangles { get { return m_Triangles; } set { m_Triangles = value; } }

        private List<int> m_ColliderTriangles = new List<int>();
        public List<int> ColliderTriangles { get { return m_ColliderTriangles; } set { m_ColliderTriangles = value; } }

        public MeshData() { }

        public void AddQuadTriangles(bool collisionMesh = false)
        {
            if (!collisionMesh)
            {
                m_Triangles.Add(m_Vertices.Count - 4);
                m_Triangles.Add(m_Vertices.Count - 3);
                m_Triangles.Add(m_Vertices.Count - 2);

                m_Triangles.Add(m_Vertices.Count - 4);
                m_Triangles.Add(m_Vertices.Count - 2);
                m_Triangles.Add(m_Vertices.Count - 1);
            }
            else
            {
                m_ColliderTriangles.Add(m_Vertices.Count - 4);
                m_ColliderTriangles.Add(m_Vertices.Count - 3);
                m_ColliderTriangles.Add(m_Vertices.Count - 2);

                m_ColliderTriangles.Add(m_Vertices.Count - 4);
                m_ColliderTriangles.Add(m_Vertices.Count - 2);
                m_ColliderTriangles.Add(m_Vertices.Count - 1);
            }
        }

        public void AddVertex(Vector3 vertex, bool collisionMesh = false)
        {
            if (!collisionMesh)
                m_Vertices.Add(vertex);
            else
                m_ColliderVertices.Add(vertex);
        }

        public void AddTriangle(int triangle, bool collisionMesh = false)
        {
            if (!collisionMesh)
                m_Triangles.Add(triangle);
            else
                m_ColliderTriangles.Add(triangle);
        }

        public void AddTriangle(bool collisionMesh = false)
        {
            if (!collisionMesh)
            {
                m_Triangles.Add(m_Vertices.Count - 3);
                m_Triangles.Add(m_Vertices.Count - 2);
                m_Triangles.Add(m_Vertices.Count - 1);
            }
            else
            {
                m_ColliderTriangles.Add(m_ColliderVertices.Count - 3);
                m_ColliderTriangles.Add(m_ColliderVertices.Count - 2);
                m_ColliderTriangles.Add(m_ColliderVertices.Count - 1);
            }
        }

        public void AddColors(float ne, float es, float sw, float wn, float light)
        {
            float aoStrength = HertzVoxConfig.AOStrength;
            float blockLightStrength = 0;

            wn = (wn * aoStrength) + (light + blockLightStrength);
            ne = (ne * aoStrength) + (light + blockLightStrength);
            es = (es * aoStrength) + (light + blockLightStrength);
            sw = (sw * aoStrength) + (light + blockLightStrength);

            m_Colors.Add(new Color(wn, wn, wn));
            m_Colors.Add(new Color(ne, ne, ne));
            m_Colors.Add(new Color(es, es, es));
            m_Colors.Add(new Color(sw, sw, sw));
        }
    }
}
