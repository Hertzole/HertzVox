using System;
using UnityEngine;

namespace Hertzole.HertzVox.Blocks.Builders
{
    [Serializable]
    public static class BlockBuilder
    {
        public static void BuildRenderer(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction)
        {
            AddQuadToMeshData(chunk, pos, meshData, direction, false);
        }

        public static void BuildCollider(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction)
        {
            AddQuadToMeshData(chunk, pos, meshData, direction, true);
        }

        public static void BuildColors(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction)
        {
            bool nSolid = false;
            bool eSolid = false;
            bool sSolid = false;
            bool wSolid = false;

            bool wnSolid = false;
            bool neSolid = false;
            bool esSolid = false;
            bool swSolid = false;

            float light = 0;

            switch (direction)
            {
                case Direction.Up:
                    nSolid = chunk.GetBlock(pos.Add(0, 1, 1)).Controller.IsBlockSolid(Direction.South);
                    eSolid = chunk.GetBlock(pos.Add(1, 1, 0)).Controller.IsBlockSolid(Direction.West);
                    sSolid = chunk.GetBlock(pos.Add(0, 1, -1)).Controller.IsBlockSolid(Direction.North);
                    wSolid = chunk.GetBlock(pos.Add(-1, 1, 0)).Controller.IsBlockSolid(Direction.East);

                    wnSolid = chunk.GetBlock(pos.Add(-1, 1, 1)).Controller.IsBlockSolid(Direction.East) && chunk.GetBlock(pos.Add(-1, 1, 1)).Controller.IsBlockSolid(Direction.South);
                    neSolid = chunk.GetBlock(pos.Add(1, 1, 1)).Controller.IsBlockSolid(Direction.South) && chunk.GetBlock(pos.Add(1, 1, 1)).Controller.IsBlockSolid(Direction.West);
                    esSolid = chunk.GetBlock(pos.Add(1, 1, -1)).Controller.IsBlockSolid(Direction.West) && chunk.GetBlock(pos.Add(1, 1, -1)).Controller.IsBlockSolid(Direction.North);
                    swSolid = chunk.GetBlock(pos.Add(-1, 1, -1)).Controller.IsBlockSolid(Direction.North) && chunk.GetBlock(pos.Add(-1, 1, -1)).Controller.IsBlockSolid(Direction.East);

                    light = chunk.GetBlock(pos.Add(0, 1, 0)).data1 / 255f;

                    break;
                case Direction.Down:
                    nSolid = chunk.GetBlock(pos.Add(0, -1, -1)).Controller.IsBlockSolid(Direction.South);
                    eSolid = chunk.GetBlock(pos.Add(1, -1, 0)).Controller.IsBlockSolid(Direction.West);
                    sSolid = chunk.GetBlock(pos.Add(0, -1, 1)).Controller.IsBlockSolid(Direction.North);
                    wSolid = chunk.GetBlock(pos.Add(-1, -1, 0)).Controller.IsBlockSolid(Direction.East);

                    wnSolid = chunk.GetBlock(pos.Add(-1, -1, -1)).Controller.IsBlockSolid(Direction.East) && chunk.GetBlock(pos.Add(-1, -1, -1)).Controller.IsBlockSolid(Direction.South);
                    neSolid = chunk.GetBlock(pos.Add(1, -1, -1)).Controller.IsBlockSolid(Direction.South) && chunk.GetBlock(pos.Add(1, -1, -1)).Controller.IsBlockSolid(Direction.West);
                    esSolid = chunk.GetBlock(pos.Add(1, -1, 1)).Controller.IsBlockSolid(Direction.West) && chunk.GetBlock(pos.Add(1, -1, 1)).Controller.IsBlockSolid(Direction.North);
                    swSolid = chunk.GetBlock(pos.Add(-1, -1, 1)).Controller.IsBlockSolid(Direction.North) && chunk.GetBlock(pos.Add(-1, -1, 1)).Controller.IsBlockSolid(Direction.East);

                    light = chunk.GetBlock(pos.Add(0, -1, 0)).data1 / 255f;

                    break;
                case Direction.North:
                    nSolid = chunk.GetBlock(pos.Add(1, 0, 1)).Controller.IsBlockSolid(Direction.West);
                    eSolid = chunk.GetBlock(pos.Add(0, 1, 1)).Controller.IsBlockSolid(Direction.Down);
                    sSolid = chunk.GetBlock(pos.Add(-1, 0, 1)).Controller.IsBlockSolid(Direction.East);
                    wSolid = chunk.GetBlock(pos.Add(0, -1, 1)).Controller.IsBlockSolid(Direction.Up);

                    esSolid = chunk.GetBlock(pos.Add(-1, 1, 1)).Controller.IsBlockSolid(Direction.East) && chunk.GetBlock(pos.Add(-1, 1, 1)).Controller.IsBlockSolid(Direction.South);
                    neSolid = chunk.GetBlock(pos.Add(1, 1, 1)).Controller.IsBlockSolid(Direction.South) && chunk.GetBlock(pos.Add(1, 1, 1)).Controller.IsBlockSolid(Direction.West);
                    wnSolid = chunk.GetBlock(pos.Add(1, -1, 1)).Controller.IsBlockSolid(Direction.West) && chunk.GetBlock(pos.Add(1, -1, 1)).Controller.IsBlockSolid(Direction.North);
                    swSolid = chunk.GetBlock(pos.Add(-1, -1, 1)).Controller.IsBlockSolid(Direction.North) && chunk.GetBlock(pos.Add(-1, -1, 1)).Controller.IsBlockSolid(Direction.East);

                    light = chunk.GetBlock(pos.Add(0, 0, 1)).data1 / 255f;

                    break;
                case Direction.East:
                    nSolid = chunk.GetBlock(pos.Add(1, 0, -1)).Controller.IsBlockSolid(Direction.Up);
                    eSolid = chunk.GetBlock(pos.Add(1, 1, 0)).Controller.IsBlockSolid(Direction.West);
                    sSolid = chunk.GetBlock(pos.Add(1, 0, 1)).Controller.IsBlockSolid(Direction.Down);
                    wSolid = chunk.GetBlock(pos.Add(1, -1, 0)).Controller.IsBlockSolid(Direction.East);

                    esSolid = chunk.GetBlock(pos.Add(1, 1, 1)).Controller.IsBlockSolid(Direction.West) && chunk.GetBlock(pos.Add(1, 1, 1)).Controller.IsBlockSolid(Direction.North);
                    neSolid = chunk.GetBlock(pos.Add(1, 1, -1)).Controller.IsBlockSolid(Direction.South) && chunk.GetBlock(pos.Add(1, 1, -1)).Controller.IsBlockSolid(Direction.West);
                    wnSolid = chunk.GetBlock(pos.Add(1, -1, -1)).Controller.IsBlockSolid(Direction.East) && chunk.GetBlock(pos.Add(1, -1, -1)).Controller.IsBlockSolid(Direction.North);
                    swSolid = chunk.GetBlock(pos.Add(1, -1, 1)).Controller.IsBlockSolid(Direction.North) && chunk.GetBlock(pos.Add(1, -1, 1)).Controller.IsBlockSolid(Direction.East);

                    light = chunk.GetBlock(pos.Add(1, 0, 0)).data1 / 255f;

                    break;
                case Direction.South:
                    nSolid = chunk.GetBlock(pos.Add(-1, 0, -1)).Controller.IsBlockSolid(Direction.Down);
                    eSolid = chunk.GetBlock(pos.Add(0, 1, -1)).Controller.IsBlockSolid(Direction.West);
                    sSolid = chunk.GetBlock(pos.Add(1, 0, -1)).Controller.IsBlockSolid(Direction.Up);
                    wSolid = chunk.GetBlock(pos.Add(0, -1, -1)).Controller.IsBlockSolid(Direction.South);

                    esSolid = chunk.GetBlock(pos.Add(1, 1, -1)).Controller.IsBlockSolid(Direction.West) && chunk.GetBlock(pos.Add(1, 1, -1)).Controller.IsBlockSolid(Direction.North);
                    neSolid = chunk.GetBlock(pos.Add(-1, 1, -1)).Controller.IsBlockSolid(Direction.South) && chunk.GetBlock(pos.Add(-1, 1, -1)).Controller.IsBlockSolid(Direction.West);
                    wnSolid = chunk.GetBlock(pos.Add(-1, -1, -1)).Controller.IsBlockSolid(Direction.East) && chunk.GetBlock(pos.Add(-1, -1, -1)).Controller.IsBlockSolid(Direction.North);
                    swSolid = chunk.GetBlock(pos.Add(1, -1, -1)).Controller.IsBlockSolid(Direction.North) && chunk.GetBlock(pos.Add(1, -1, -1)).Controller.IsBlockSolid(Direction.East);

                    light = chunk.GetBlock(pos.Add(0, 0, -1)).data1 / 255f;

                    break;
                case Direction.West:
                    nSolid = chunk.GetBlock(pos.Add(-1, 0, 1)).Controller.IsBlockSolid(Direction.Up);
                    eSolid = chunk.GetBlock(pos.Add(-1, 1, 0)).Controller.IsBlockSolid(Direction.West);
                    sSolid = chunk.GetBlock(pos.Add(-1, 0, -1)).Controller.IsBlockSolid(Direction.Down);
                    wSolid = chunk.GetBlock(pos.Add(-1, -1, 0)).Controller.IsBlockSolid(Direction.East);

                    esSolid = chunk.GetBlock(pos.Add(-1, 1, -1)).Controller.IsBlockSolid(Direction.West) && chunk.GetBlock(pos.Add(-1, 1, -1)).Controller.IsBlockSolid(Direction.North);
                    neSolid = chunk.GetBlock(pos.Add(-1, 1, 1)).Controller.IsBlockSolid(Direction.South) && chunk.GetBlock(pos.Add(-1, 1, 1)).Controller.IsBlockSolid(Direction.West);
                    wnSolid = chunk.GetBlock(pos.Add(-1, -1, 1)).Controller.IsBlockSolid(Direction.East) && chunk.GetBlock(pos.Add(-1, -1, 1)).Controller.IsBlockSolid(Direction.North);
                    swSolid = chunk.GetBlock(pos.Add(-1, -1, -1)).Controller.IsBlockSolid(Direction.North) && chunk.GetBlock(pos.Add(-1, -1, -1)).Controller.IsBlockSolid(Direction.East);

                    light = chunk.GetBlock(pos.Add(-1, 0, 0)).data1 / 255f;

                    break;
                default:
                    Debug.LogError("Direction not recognized");
                    break;
            }

            AddColors(meshData, wnSolid, nSolid, neSolid, eSolid, esSolid, sSolid, swSolid, wSolid, light);
        }

        public static void BuildTexture(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, TextureCollection textureCollection)
        {
            Rect texture = textureCollection.GetTexture(chunk, pos, direction);
            Vector2[] UVs = new Vector2[4];

            UVs[0] = new Vector2(texture.x + texture.width, texture.y);
            UVs[1] = new Vector2(texture.x + texture.width, texture.y + texture.height);
            UVs[2] = new Vector2(texture.x, texture.y + texture.height);
            UVs[3] = new Vector2(texture.x, texture.y);

            meshData.UV.AddRange(UVs);
        }

        public static void BuildTexture(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, TextureCollection[] textureCollections)
        {
            try
            {
                Rect texture = new Rect();

                switch (direction)
                {
                    case Direction.Up:
                        texture = textureCollections[0].GetTexture(chunk, pos, direction);
                        break;
                    case Direction.Down:
                        texture = textureCollections[1].GetTexture(chunk, pos, direction);
                        break;
                    case Direction.North:
                        texture = textureCollections[2].GetTexture(chunk, pos, direction);
                        break;
                    case Direction.East:
                        texture = textureCollections[3].GetTexture(chunk, pos, direction);
                        break;
                    case Direction.South:
                        texture = textureCollections[4].GetTexture(chunk, pos, direction);
                        break;
                    case Direction.West:
                        texture = textureCollections[5].GetTexture(chunk, pos, direction);
                        break;
                    default:
                        break;
                }

                Vector2[] UVs = new Vector2[4];

                UVs[0] = new Vector2(texture.x + texture.width, texture.y);
                UVs[1] = new Vector2(texture.x + texture.width, texture.y + texture.height);
                UVs[2] = new Vector2(texture.x, texture.y + texture.height);
                UVs[3] = new Vector2(texture.x, texture.y);

                meshData.UV.AddRange(UVs);

            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private static void AddQuadToMeshData(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, bool useCollisionMesh)
        {
            //Debug.Log(chunk.gameObject.name + " add quad to mesh data");

            // Adding a tiny overlap between block meshes may solve floating point imprecision
            // errors causing pixel size gaps between blocks when looking closely
            float halfBlock = (1f / 2) + 0.0005f;

            //Converting the position to a vector adjusts it based on block size and gives us real world coordinates for x, y and z
            Vector3 vPos = pos;

            switch (direction)
            {
                case Direction.Up:
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y + halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y + halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y + halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y + halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    break;
                case Direction.Down:
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y - halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y - halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y - halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y - halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    break;
                case Direction.North:
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y - halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y + halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y + halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y - halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    break;
                case Direction.East:
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y - halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y + halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y + halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y - halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    break;
                case Direction.South:
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y - halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y + halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y + halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y - halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    break;
                case Direction.West:
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y - halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y + halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y + halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y - halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    break;
                default:
                    Debug.LogError("Direction not recognized");
                    break;
            }

            meshData.AddQuadTriangles(useCollisionMesh);
        }

        private static void AddColors(MeshData meshData, bool wnSolid, bool nSolid, bool neSolid, bool eSolid, bool esSolid, bool sSolid, bool swSolid, bool wSolid, float light)
        {
            float ne = 1;
            float es = 1;
            float sw = 1;
            float wn = 1;

            float aoContrast = 0.2f;

            if (nSolid)
            {
                wn -= aoContrast;
                ne -= aoContrast;
            }

            if (eSolid)
            {
                ne -= aoContrast;
                es -= aoContrast;
            }

            if (sSolid)
            {
                es -= aoContrast;
                sw -= aoContrast;
            }

            if (wSolid)
            {
                sw -= aoContrast;
                wn -= aoContrast;
            }

            if (neSolid)
                ne -= aoContrast;

            if (swSolid)
                sw -= aoContrast;

            if (wnSolid)
                wn -= aoContrast;

            if (esSolid)
                es -= aoContrast;

            meshData.AddColors(ne, es, sw, wn, light);
        }
    }
}
