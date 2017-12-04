//
// Copyright (c) 2012-2013 Mikola Lysenko
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections.Generic;
using UnityEngine;

namespace Hertzole.HertzVox.Experimental
{
    public class MergedFaceMeshBuilder
    {
        public static void ReduceMesh(Chunk chunk, MeshData meshData)
        {
            if (chunk.IsEmpty())
                return;

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            int size = Chunk.CHUNK_SIZE;

            // Sweep over 3-axes
            for (int d = 0; d < 3; d++)
            {
                int i, j, k, l, w, h, u = (d + 1) % 3, v = (d + 2) % 3;

                int[] x = new int[3];
                int[] q = new int[3];
                int[] mask = new int[(size + 1) * (size + 1)];

                q[d] = 1;

                for (x[d] = -1; x[d] < size;)
                {
                    // Compute the mask
                    int n = 0;
                    for (x[v] = 0; x[v] < size; ++x[v])
                    {
                        for (x[u] = 0; x[u] < size; ++x[u], ++n)
                        {
                            int a = 0; if (0 <= x[d])
                            {
                                a = (int)VoxelTerrain.GetBlock(chunk, x[0], x[1], x[2]).type;
                            }
                            int b = 0; if (x[d] < size - 1)
                            {
                                b = (int)VoxelTerrain.GetBlock(chunk, x[0] + q[0], x[1] + q[1], x[2] + q[2]).type;
                            }

                            if (a != -1 && b != -1 && a == b)
                            {
                                mask[n] = 0;
                            }
                            else if (a > 0)
                            {
                                a = 1;
                                mask[n] = a;
                            }
                            else
                            {
                                b = 1;
                                mask[n] = -b;
                            }
                        }
                    }

                    // Increment x[d]
                    ++x[d];

                    // Generate mesh for mask using lexicographic ordering
                    n = 0;
                    for (j = 0; j < size; ++j)
                    {
                        for (i = 0; i < size;)
                        {
                            var c = mask[n]; if (c > -3)
                            {
                                // Compute width
                                for (w = 1; c == mask[n + w] && i + w < size; ++w) { }

                                // Compute height
                                bool done = false;
                                for (h = 1; j + h < size; ++h)
                                {
                                    for (k = 0; k < w; ++k)
                                    {
                                        if (c != mask[n + k + h * size])
                                        {
                                            done = true; break;
                                        }
                                    }
                                    if (done)
                                        break;
                                }
                                // Add quad   
                                bool flip = false;
                                x[u] = i; x[v] = j;
                                int[] du = new int[3];
                                int[] dv = new int[3];
                                if (c > -1)
                                {
                                    du[u] = w;
                                    dv[v] = h;
                                }
                                else
                                {
                                    flip = true;
                                    c = -c;
                                    du[u] = w;
                                    dv[v] = h;
                                }

                                Vector3 v1 = new Vector3(x[0], x[1], x[2]);
                                Vector3 v2 = new Vector3(x[0] + du[0], x[1] + du[1], x[2] + du[2]);
                                Vector3 v3 = new Vector3(x[0] + du[0] + dv[0], x[1] + du[1] + dv[1], x[2] + du[2] + dv[2]);
                                Vector3 v4 = new Vector3(x[0] + dv[0], x[1] + dv[1], x[2] + dv[2]);

                                if (c > 0 && !flip)
                                {
                                    AddFace(v1, v2, v3, v4, vertices, triangles, 0);
                                }

                                if (flip)
                                {
                                    AddFace(v4, v3, v2, v1, vertices, triangles, 0);
                                }

                                // Zero-out mask
                                for (l = 0; l < h; ++l)
                                    for (k = 0; k < w; ++k)
                                    {
                                        mask[n + k + l * size] = 0;
                                    }

                                // Increment counters and continue
                                i += w; n += w;
                            }

                            else
                            {
                                ++i;
                                ++n;
                            }
                        }
                    }
                }
            }

            meshData.ColliderVertices = vertices;
            meshData.ColliderTriangles = triangles;
        }

        private static void AddFace(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, List<Vector3> vertices, List<int> triangles, int order)
        {
            if (order == 0)
            {
                int index = vertices.Count;

                vertices.Add(v1 - new Vector3(0.5f, 0.5f, 0.5f));
                vertices.Add(v2 - new Vector3(0.5f, 0.5f, 0.5f));
                vertices.Add(v3 - new Vector3(0.5f, 0.5f, 0.5f));
                vertices.Add(v4 - new Vector3(0.5f, 0.5f, 0.5f));

                triangles.Add(index);
                triangles.Add(index + 1);
                triangles.Add(index + 2);
                triangles.Add(index + 2);
                triangles.Add(index + 3);
                triangles.Add(index);
            }

            if (order == 1)
            {
                int index = vertices.Count;

                vertices.Add(v1);
                vertices.Add(v2);
                vertices.Add(v3);
                vertices.Add(v4);

                triangles.Add(index);
                triangles.Add(index + 3);
                triangles.Add(index + 2);
                triangles.Add(index + 2);
                triangles.Add(index + 1);
                triangles.Add(index);
            }
        }
    }
}
