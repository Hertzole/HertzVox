//using Hertzole.HertzBlocks.Blocks;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Hertzole.HertzBlocks
//{
//    public static class VoxelTerrain
//    {
//        private const int NORTH = 0;
//        private const int EAST = 1;
//        private const int SOUTH = 2;
//        private const int WEST = 3;
//        private const int UP = 4;
//        private const int DOWN = 5;

//        //public static WorldPos GetBlockPos(Vector3 pos)
//        //{
//        //    return new WorldPos(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
//        //}

//        //public static WorldPos GetBlockPos(RaycastHit hit, bool adjacent = false)
//        //{
//        //    Vector3 pos = new Vector3(MoveWithinBlock(hit.point.x, hit.normal.x, adjacent), MoveWithinBlock(hit.point.y, hit.normal.y, adjacent), MoveWithinBlock(hit.point.z, hit.normal.z, adjacent));

//        //    return GetBlockPos(pos);
//        //}

//        //private static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
//        //{
//        //    if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
//        //    {
//        //        if (adjacent)
//        //            pos += (norm / 2);
//        //        else
//        //            pos -= (norm / 2);
//        //    }

//        //    return (float)pos;
//        //}

//        //public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
//        //{
//        //    Chunk chunk = hit.collider.GetComponent<Chunk>();
//        //    if (chunk == null)
//        //        return false;

//        //    WorldPos pos = GetBlockPos(hit, adjacent);

//        //    chunk.World.SetBlock(pos.x, pos.y, pos.z, block);

//        //    return true;
//        //}

//        //public static void SetBlock(int x, int y, int z, Block block)
//        //{
//        //    World.Instance.SetBlock(x, y, z, block);
//        //}

//        //public static void FillBlock(Vector3 start, Vector3 end, Block block)
//        //{
//        //    World.Instance.FillBlocks(start, end, block);
//        //}

//        //public static void FillBlock(WorldPos start, WorldPos end, Block block)
//        //{
//        //    World.Instance.FillBlocks(start, end, block);
//        //}

//        //public static void FillBlock(int startX, int startY, int startZ, int endX, int endY, int endZ, Block block)
//        //{
//        //    World.Instance.FillBlocks(startX, startY, startZ, endX, endY, endZ, block);
//        //}

//        //public static bool DestroyBlock(RaycastHit hit)
//        //{
//        //    Chunk chunk = hit.collider.GetComponent<Chunk>();
//        //    if (chunk == null)
//        //        return false;

//        //    WorldPos pos = GetBlockPos(hit, false);
//        //    chunk.DestroyBlock(pos.x, pos.y, pos.z);

//        //    return true;
//        //}

//        //public static void DestroyBlock(int x, int y, int z)
//        //{
//        //    World.Instance.SetBlock(x, y, z, new BlockAir());
//        //}

//        //public static Block GetBlock(RaycastHit hit, bool adjacent = false)
//        //{
//        //    Chunk chunk = hit.collider.GetComponent<Chunk>();
//        //    if (chunk == null)
//        //        return null;

//        //    WorldPos pos = GetBlockPos(hit, adjacent);
//        //    Block block = chunk.World.GetBlock(pos.x, pos.y, pos.z);
//        //    return block;
//        //}

//        //public static Block GetBlock(int x, int y, int z)
//        //{
//        //    return World.Instance.GetBlock(x, y, z);
//        //}

//        //public static void CreateSphere(Vector3 center, int radius, Block block)
//        //{
//        //    for (int x = -radius; x < radius; x++)
//        //    {
//        //        for (int y = -radius; y < radius; y++)
//        //        {
//        //            for (int z = -radius; z < radius; z++)
//        //            {
//        //                Vector3 position = new Vector3(x + center.x, y + center.y, z + center.z);
//        //                float distance = Vector3.Distance(position, center);
//        //                if (distance < radius)
//        //                    SetBlock(Mathf.FloorToInt(x + center.x), Mathf.FloorToInt(y + center.y), Mathf.FloorToInt(z + center.z), block);
//        //            }
//        //        }
//        //    }
//        //}

//        public static Mesh ReduceMesh(Chunk chunk)
//        {

//            List<Vector3> vertices = new List<Vector3>();
//            List<int> elements = new List<int>();

//            int size = Chunk.CHUNK_SIZE;

//            //Sweep over 3-axes
//            for (int d = 0; d < 3; d++)
//            {

//                int i, j, k, l, w, h, u = (d + 1) % 3, v = (d + 2) % 3;

//                int[] x = new int[3];
//                int[] q = new int[3];

//                //   bool[] mask = new bool[size * size];
//                int[] mask = new int[size * size * size];

//                q[d] = 1;



//                for (x[d] = -1; x[d] < size;)
//                {

//                    // Compute the mask
//                    int n = 0;
//                    for (x[v] = 0; x[v] < size; ++x[v])
//                    {
//                        for (x[u] = 0; x[u] < size; ++x[u], ++n)
//                        {


//                            int a = (0 <= x[d] ? data(chunk, x[0], x[1], x[2]) : 0), b = (x[d] < size - 1 ? data(chunk, x[0] + q[0], x[1] + q[1], x[2] + q[2]) : 0);


//                            if (a != -1 && b != -1 && a == b) { mask[n] = 0; }

//                            else if (a > 0)
//                            {
//                                mask[n] = a;
//                            }
//                            else
//                            {
//                                mask[n] = -b;
//                            }
//                        }
//                    }

//                    // Increment x[d]
//                    ++x[d];

//                    // Generate mesh for mask using lexicographic ordering
//                    n = 0;
//                    for (j = 0; j < size; ++j)
//                    {
//                        for (i = 0; i < size;)
//                        {

//                            var c = mask[n];

//                            if (c > -2)
//                            {
//                                // Compute width
//                                for (w = 1; c == mask[n + w] && i + w < size; ++w) { }

//                                // Compute height (this is slightly awkward
//                                bool done = false;
//                                for (h = 1; j + h < size; ++h)
//                                {
//                                    for (k = 0; k < w; ++k)
//                                    {
//                                        if (c != mask[n + k + h * size])
//                                        {
//                                            done = true;
//                                            break;
//                                        }
//                                    }

//                                    if (done) break;
//                                }

//                                // Add quad
//                                bool flip = false;

//                                x[u] = i;
//                                x[v] = j;
//                                int[] du = new int[3];
//                                int[] dv = new int[3];

//                                if (c > -1)
//                                {
//                                    du[u] = w;
//                                    dv[v] = h;
//                                }
//                                else
//                                {
//                                    flip = true;
//                                    c = -c;
//                                    du[u] = w;
//                                    dv[v] = h;

//                                }


//                                Vector3 v1 = new Vector3(x[0], x[1], x[2]);
//                                Vector3 v2 = new Vector3(x[0] + du[0], x[1] + du[1], x[2] + du[2]);
//                                Vector3 v3 = new Vector3(x[0] + du[0] + dv[0], x[1] + du[1] + dv[1], x[2] + du[2] + dv[2]);
//                                Vector3 v4 = new Vector3(x[0] + dv[0], x[1] + dv[1], x[2] + dv[2]);

//                                bool floor = false;

//                                if (v1.y == 0 && v2.y == 0 && v3.y == 0 && v4.y == v1.y) { floor = true; }

//                                if (c == 1 && !floor)
//                                {
//                                    AddFace(v1, v2, v3, v4, vertices, elements);
//                                }

//                                if (flip)
//                                {
//                                    AddFace(v4, v3, v2, v1, vertices, elements);
//                                }

//                                // Zero-out mask
//                                for (l = 0; l < h; ++l)
//                                    for (k = 0; k < w; ++k)
//                                    {
//                                        mask[n + k + l * size] = 0;
//                                    }

//                                // Increment counters and continue
//                                i += w; n += w;
//                            }

//                            else
//                            {
//                                ++i;
//                                ++n;
//                            }
//                        }
//                    }
//                }
//            }

//            Mesh mesh = new Mesh();
//            mesh.Clear();
//            mesh.vertices = vertices.ToArray();
//            mesh.triangles = elements.ToArray();

//            mesh.RecalculateBounds();
//            mesh.RecalculateNormals();

//            return mesh;

//        }


//        private static void AddFace(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, List<Vector3> vertices, List<int> elements)
//        {
//            int index = vertices.Count;

//            vertices.Add(v1);
//            vertices.Add(v2);
//            vertices.Add(v3);
//            vertices.Add(v4);

//            elements.Add(index);
//            elements.Add(index + 1);
//            elements.Add(index + 2);
//            elements.Add(index + 2);
//            elements.Add(index + 3);
//            elements.Add(index);

//        }


//        private static int data(Chunk chunk, int x, int y, int z)
//        {
//            //return voxelData[x + 32 * y + 32 * 32 * z] != 0;
//            int b = (int)chunk.Blocks[x, y, z].IndexID;
//            if (b > 1) { b = 1; }
//            return b;
//        }

//        //[System.Obsolete("Never use. Horribly broken.")]
//        //public static Mesh CreateColliderMesh(Chunk chunk)
//        //{
//        //    List<Vector3> vertices = new List<Vector3>();
//        //    List<int> triangles = new List<int>();

//        //    int size = Chunk.CHUNK_SIZE;

//        //    //Sweep over 3-axes
//        //    for (int d = 0; d < 3; d++)
//        //    {

//        //        int i, j, k, l, w, h, u = (d + 1) % 3, v = (d + 2) % 3;

//        //        int[] x = new int[3];
//        //        int[] q = new int[3];
//        //        int[] mask = new int[(size + 1) * (size + 1)];

//        //        q[d] = 1;

//        //        for (x[d] = -1; x[d] < size;)
//        //        {

//        //            // Compute the mask
//        //            int n = 0;
//        //            for (x[v] = 0; x[v] < size; ++x[v])
//        //            {
//        //                for (x[u] = 0; x[u] < size; ++x[u], ++n)
//        //                {
//        //                    int a = 0;
//        //                    if (0 <= x[d])
//        //                        a = (int)World.Instance.GetBlock(chunk, x[0], x[1], x[2]).IndexID;
//        //                    int b = 0;
//        //                    if (x[d] < size - 1)
//        //                        b = (int)World.Instance.GetBlock(chunk, x[0] + q[0], x[1] + q[1], x[2] + q[2]).IndexID;
//        //                    if (a != -1 && b != -1 && a == b)
//        //                        mask[n] = 0;
//        //                    else if (a > 0)
//        //                    {
//        //                        a = 1;
//        //                        mask[n] = a;
//        //                    }
//        //                    else
//        //                    {
//        //                        b = 1;
//        //                        mask[n] = -b;
//        //                    }
//        //                }
//        //            }

//        //            // Increment x[d]
//        //            ++x[d];

//        //            // Generate mesh for mask using lexicographic ordering
//        //            n = 0;
//        //            for (j = 0; j < size; ++j)
//        //            {
//        //                for (i = 0; i < size;)
//        //                {
//        //                    var c = mask[n]; if (c > -3)
//        //                    {
//        //                        // Compute width
//        //                        for (w = 1; c == mask[n + w] && i + w < size; ++w) { }

//        //                        // Compute height
//        //                        bool done = false;
//        //                        for (h = 1; j + h < size; ++h)
//        //                        {
//        //                            for (k = 0; k < w; ++k) { if (c != mask[n + k + h * size]) { done = true; break; } }
//        //                            if (done) break;
//        //                        }
//        //                        bool flip = false;
//        //                        x[u] = i;
//        //                        x[v] = j;
//        //                        int[] du = new int[3];
//        //                        int[] dv = new int[3];
//        //                        if (c > -1)
//        //                        {
//        //                            du[u] = w;
//        //                            dv[v] = h;
//        //                        }
//        //                        else
//        //                        {
//        //                            flip = true;
//        //                            c = -c;
//        //                            du[u] = w;
//        //                            dv[v] = h;
//        //                        }

//        //                        Vector3 v1 = new Vector3(x[0], x[1], x[2]);
//        //                        Vector3 v2 = new Vector3(x[0] + du[0], x[1] + du[1], x[2] + du[2]);
//        //                        Vector3 v3 = new Vector3(x[0] + du[0] + dv[0], x[1] + du[1] + dv[1], x[2] + du[2] + dv[2]);
//        //                        Vector3 v4 = new Vector3(x[0] + dv[0], x[1] + dv[1], x[2] + dv[2]);

//        //                        if (c > 0 && !flip)
//        //                        {
//        //                            AddFace(v1, v2, v3, v4, vertices, triangles, 0);
//        //                        }

//        //                        if (flip)
//        //                        {
//        //                            AddFace(v4, v3, v2, v1, vertices, triangles, 0);
//        //                        }

//        //                        // Zero-out mask
//        //                        for (l = 0; l < h; ++l)
//        //                            for (k = 0; k < w; ++k)
//        //                            {
//        //                                mask[n + k + l * size] = 0;
//        //                            }

//        //                        // Increment counters and continue
//        //                        i += w; n += w;
//        //                    }

//        //                    else
//        //                    {
//        //                        ++i;
//        //                        ++n;
//        //                    }
//        //                }
//        //            }
//        //        }
//        //    }

//        //    Mesh mesh = new Mesh();
//        //    mesh.Clear();
//        //    mesh.vertices = vertices.ToArray();
//        //    mesh.triangles = triangles.ToArray();
//        //    mesh.RecalculateBounds();
//        //    mesh.RecalculateNormals();

//        //    return mesh;
//        //}

//        //private static void AddFace(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, List<Vector3> vertices, List<int> triangles, int order)
//        //{
//        //    if (order == 0)
//        //    {
//        //        int index = vertices.Count;

//        //        vertices.Add(v1);
//        //        vertices.Add(v2);
//        //        vertices.Add(v3);
//        //        vertices.Add(v4);

//        //        triangles.Add(index);
//        //        triangles.Add(index + 1);
//        //        triangles.Add(index + 2);
//        //        triangles.Add(index + 2);
//        //        triangles.Add(index + 3);
//        //        triangles.Add(index);

//        //    }

//        //    if (order == 1)
//        //    {
//        //        int index = vertices.Count;

//        //        vertices.Add(v1);
//        //        vertices.Add(v2);
//        //        vertices.Add(v3);
//        //        vertices.Add(v4);

//        //        triangles.Add(index);
//        //        triangles.Add(index + 3);
//        //        triangles.Add(index + 2);
//        //        triangles.Add(index + 2);
//        //        triangles.Add(index + 1);
//        //        triangles.Add(index);

//        //    }
//        //}


//        /////////////////////////////////////////////////////////////////////////
//        //
//        // PORTIONS OF THIS CODE:
//        //
//        // The MIT License (MIT)
//        //
//        // Copyright (c) 2012-2013 Mikola Lysenko
//        //
//        // Permission is hereby granted, free of charge, to any person obtaining a copy
//        // of this software and associated documentation files (the "Software"), to deal
//        // in the Software without restriction, including without limitation the rights
//        // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//        // copies of the Software, and to permit persons to whom the Software is
//        // furnished to do so, subject to the following conditions:
//        //
//        // The above copyright notice and this permission notice shall be included in
//        // all copies or substantial portions of the Software.
//        //
//        // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//        // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//        // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//        // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//        // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//        // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//        // THE SOFTWARE.

//        public static void BuildGreedyCollider(ref Mesh mesh, Vector3 offset, int size)
//        {
//            Vector3[] normalDir = {
//            -Vector3.left,
//            Vector3.up,
//            Vector3.forward
//        };

//            List<Vector3> vertices = new List<Vector3>();
//            List<Vector3> normals = new List<Vector3>();
//            List<int> elements = new List<int>();

//            int[] mask = new int[(size + 1) * (size + 1)];

//            int index = 0;


//            // Sweep over 3-axes
//            for (int d = 0; d < 3; d++)
//            {
//                int i, j, k, l, w, h, u = (d + 1) % 3, v = (d + 2) % 3;

//                int[] x = new int[3];
//                int[] q = new int[3];

//                q[d] = 1;

//                for (x[d] = -1; x[d] < size;)
//                {

//                    // Compute the mask
//                    int n = 0;
//                    for (x[v] = 0; x[v] < size; ++x[v])
//                    {
//                        for (x[u] = 0; x[u] < size; ++x[u], ++n)
//                        {
//                            //int a = (0 <= x[d] ? isSolid(x[0], x[1], x[2]) : 0);

//                            Block aVoxel = GetBlock(x[0] + (int)offset.x, x[1] + (int)offset.y, x[2] + (int)offset.z);
//                            int a = 0;
//                            if (0 <= x[d])
//                            {
//                                //a = isSolid(x[0], x[1], x[2]);
//                                a = 1;
//                            }



//                            //int b = (x[d] < size - 1 ? isSolid(x[0] + q[0], x[1] + q[1], x[2] + q[2]) : 0);

//                            int b = 0;
//                            Block bVoxel = GetBlock(x[0] + q[0] + (int)offset.x, x[1] + q[1] + (int)offset.y, x[2] + q[2] + (int)offset.z);
//                            if (x[d] < size - 1)
//                            {
//                                //b = isSolid(x[0] + q[0], x[1] + q[1], x[2] + q[2]);
//                                b = 1;
//                            }


//                            // KLL: a and b can never be -1
//                            //if (a !=-1 && b !=-1 && a == b) {
//                            if (a == b)
//                            {
//                                // KLL: I believe this means no face
//                                mask[n] = 0;
//                            }
//                            else if (a > 0)
//                            {
//                                mask[n] = a;
//                            }
//                            else
//                            {
//                                mask[n] = -b;
//                            }
//                        }
//                    }

//                    // Increment x[d]
//                    ++x[d];

//                    // Generate mesh for mask using lexicographic ordering
//                    n = 0;
//                    for (j = 0; j < size; ++j)
//                    {
//                        for (i = 0; i < size;)
//                        {

//                            var c = mask[n];

//                            if (c > -2)
//                            {
//                                // Compute width
//                                for (w = 1; c == mask[n + w] && i + w < size; ++w)
//                                {
//                                }

//                                // Compute height (this is slightly awkward
//                                bool done = false;
//                                for (h = 1; j + h < size; ++h)
//                                {
//                                    for (k = 0; k < w; ++k)
//                                    {
//                                        if (c != mask[n + k + h * size])
//                                        {
//                                            done = true;
//                                            break;
//                                        }
//                                    }

//                                    if (done)
//                                        break;
//                                }

//                                // Add quad
//                                bool flip = false;

//                                x[u] = i;
//                                x[v] = j;
//                                int[] du = new int[3];
//                                int[] dv = new int[3];

//                                if (c > -1)
//                                {
//                                    du[u] = w;
//                                    dv[v] = h;
//                                }
//                                else
//                                {
//                                    flip = true;
//                                    c = -c;
//                                    du[u] = w;
//                                    dv[v] = h;

//                                }


//                                Vector3 v1 = new Vector3(x[0], x[1], x[2]);
//                                Vector3 v2 = new Vector3(x[0] + du[0], x[1] + du[1], x[2] + du[2]);
//                                Vector3 v3 = new Vector3(x[0] + du[0] + dv[0], x[1] + du[1] + dv[1], x[2] + du[2] + dv[2]);
//                                Vector3 v4 = new Vector3(x[0] + dv[0], x[1] + dv[1], x[2] + dv[2]);

//                                if (c > 0 && !flip)
//                                {
//                                    //AddFace(v1, v2, v3, v4, vertices, elements, offset);


//                                    vertices.Add(v1 + offset);
//                                    vertices.Add(v2 + offset);
//                                    vertices.Add(v3 + offset);
//                                    vertices.Add(v4 + offset);

//                                    elements.Add(index);
//                                    elements.Add(index + 1);
//                                    elements.Add(index + 2);
//                                    elements.Add(index + 2);
//                                    elements.Add(index + 3);
//                                    elements.Add(index);

//                                    Vector3 normal = normalDir[d];
//                                    normals.Add(normal);
//                                    normals.Add(normal);
//                                    normals.Add(normal);
//                                    normals.Add(normal);

//                                    index += 4;
//                                }
//                                else if (flip)
//                                {
//                                    //AddFace(v4, v3, v2, v1, vertices, elements, offset);

//                                    vertices.Add(v4 + offset);
//                                    vertices.Add(v3 + offset);
//                                    vertices.Add(v2 + offset);
//                                    vertices.Add(v1 + offset);

//                                    elements.Add(index);
//                                    elements.Add(index + 1);
//                                    elements.Add(index + 2);
//                                    elements.Add(index + 2);
//                                    elements.Add(index + 3);
//                                    elements.Add(index);

//                                    Vector3 normal = -normalDir[d];
//                                    normals.Add(normal);
//                                    normals.Add(normal);
//                                    normals.Add(normal);
//                                    normals.Add(normal);

//                                    index += 4;
//                                }

//                                // Zero-out mask
//                                for (l = 0; l < h; ++l)
//                                {
//                                    for (k = 0; k < w; ++k)
//                                    {
//                                        mask[n + k + l * size] = 0;
//                                    }
//                                }

//                                // Increment counters and continue
//                                i += w;
//                                n += w;
//                            }
//                            else
//                            {
//                                ++i;
//                                ++n;
//                            }
//                        }
//                    }
//                }
//            }

//            mesh.Clear();
//            mesh.vertices = vertices.ToArray();
//            mesh.triangles = elements.ToArray();
//            mesh.normals = normals.ToArray();

//            mesh.RecalculateBounds();
//            //mesh.RecalculateNormals();
//        }
//    }
//}
