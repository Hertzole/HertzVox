using System;
using UnityEngine;

namespace Hertzole.HertzVox.Blocks.Builders
{
    [Serializable]
    public class SlopeBuilder
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

                    light = chunk.GetBlock(pos.Add(0, 1, 0)).Data1 / 255f;

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

                    light = chunk.GetBlock(pos.Add(0, -1, 0)).Data1 / 255f;

                    break;
                case Direction.North:
                    //nSolid = chunk.GetBlock(pos.Add(1, 0, 1)).Controller.IsBlockSolid(Direction.West);
                    //eSolid = chunk.GetBlock(pos.Add(0, 1, 1)).Controller.IsBlockSolid(Direction.Down);
                    //sSolid = chunk.GetBlock(pos.Add(-1, 0, 1)).Controller.IsBlockSolid(Direction.East);
                    //wSolid = chunk.GetBlock(pos.Add(0, -1, 1)).Controller.IsBlockSolid(Direction.Up);

                    //esSolid = chunk.GetBlock(pos.Add(-1, 1, 1)).Controller.IsBlockSolid(Direction.East) && chunk.GetBlock(pos.Add(-1, 1, 1)).Controller.IsBlockSolid(Direction.South);
                    //neSolid = chunk.GetBlock(pos.Add(1, 1, 1)).Controller.IsBlockSolid(Direction.South) && chunk.GetBlock(pos.Add(1, 1, 1)).Controller.IsBlockSolid(Direction.West);
                    //wnSolid = chunk.GetBlock(pos.Add(1, -1, 1)).Controller.IsBlockSolid(Direction.West) && chunk.GetBlock(pos.Add(1, -1, 1)).Controller.IsBlockSolid(Direction.North);
                    //swSolid = chunk.GetBlock(pos.Add(-1, -1, 1)).Controller.IsBlockSolid(Direction.North) && chunk.GetBlock(pos.Add(-1, -1, 1)).Controller.IsBlockSolid(Direction.East);

                    //light = chunk.GetBlock(pos.Add(0, 0, 1)).Data1 / 255f;

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

                    light = chunk.GetBlock(pos.Add(1, 0, 0)).Data1 / 255f;

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

                    light = chunk.GetBlock(pos.Add(0, 0, -1)).Data1 / 255f;

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

                    light = chunk.GetBlock(pos.Add(-1, 0, 0)).Data1 / 255f;

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
                        //texture = textureCollections[2].GetTexture(chunk, pos, direction);
                        return;
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

                if (direction != Direction.East)
                {
                    Vector2[] UVs = new Vector2[4];

                    UVs[0] = new Vector2(texture.x + texture.width, texture.y);
                    UVs[1] = new Vector2(texture.x + texture.width, texture.y + texture.height);
                    UVs[2] = new Vector2(texture.x, texture.y + texture.height);
                    UVs[3] = new Vector2(texture.x, texture.y);

                    meshData.UV.AddRange(UVs);
                }
                else
                {
                    //Vector2[] uvs = new Vector2[] { new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, 0), };
                    Vector2[] uvs = new Vector2[] { new Vector2(1, 0), new Vector2(0, 0), new Vector2(1, 1), };
                    Vector2[] UVs = new Vector2[3];
                    UVs[1] = new Vector2(texture.x + texture.width, texture.y);
                    UVs[0] = new Vector2(texture.x + texture.width, texture.y + texture.height);
                    UVs[2] = new Vector2(texture.x, texture.y);
                    //UVs[3] = new Vector2(texture.x, texture.y);

                    meshData.UV.AddRange(UVs);
                }

            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private static void AddQuadToMeshData(Chunk chunk, BlockPos pos, MeshData meshData, Direction direction, bool useCollisionMesh)
        {
            float halfBlock = (1f / 2f) + 0.0005f;

            Vector3 vPos = pos;

            switch (direction)
            {
                case Direction.Up:
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y + halfBlock - 1, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y + halfBlock - 1, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y + halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y + halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddQuadTriangles(useCollisionMesh);
                    break;
                case Direction.Down:
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y - halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y - halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y - halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y - halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddQuadTriangles(useCollisionMesh);
                    break;
                case Direction.North:
                    //meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y - halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    //meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y + halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    //meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y + halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    //meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y - halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    return;
                case Direction.East:
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y - halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y + halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    //meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y + halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y - halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddQuadTriangles(useCollisionMesh);
                    break;
                case Direction.South:
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y - halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y + halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y + halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x + halfBlock, vPos.y - halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddQuadTriangles(useCollisionMesh);
                    break;
                case Direction.West:
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y - halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y + halfBlock, vPos.z + halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y + halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddVertex(new Vector3(vPos.x - halfBlock, vPos.y - halfBlock, vPos.z - halfBlock), useCollisionMesh);
                    meshData.AddQuadTriangles(useCollisionMesh);
                    break;
                default:
                    Debug.LogError("Direction not recognized");
                    break;
            }


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
