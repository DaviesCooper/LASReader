using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.PointCloud.Scripts
{
    class TempMesh
    {
        bool created = false;
        public Vector3[] vertices;
        public Color[] colors;
        public int[] indices;

        public TempMesh()
        {
            created = true;
        }

        public Mesh Convert()
        {
            if (!created)
                throw new Exception("Cannot convert a null mesh");
            Mesh m = new Mesh();
            m.vertices = vertices;
            m.colors = colors;
            m.SetIndices(indices, MeshTopology.Points, 0);
            m.uv = new Vector2[vertices.Length];
            m.normals = new Vector3[vertices.Length];
            return m;
        }
    }
}
