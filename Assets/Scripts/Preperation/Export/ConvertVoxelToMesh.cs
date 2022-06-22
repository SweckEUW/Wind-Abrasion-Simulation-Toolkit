using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

    public static class ConvertVoxelToMesh{

        public static void Convert(){

            //Set the mode used to create the mesh.
            //Cubes is faster and creates less verts, tetrahedrons is slower and creates more verts but better represents the mesh surface.
            Marching marching = new MarchingCubes(); //new MarchingTertrahedron();

            //Surface is the value that represents the surface of mesh
            //For example the perlin noise has a range of -1 to 1 so the mid point is where we want the surface to cut through.
            //The target value does not have to be the mid point it can be any value with in the range.
            marching.Surface = 0.0f;

            //The size of voxel array.
            int width = 500;
            int height = 500;
            int depth = 500;

            var voxels = new VoxelArray(width, height, depth);

            foreach (Transform child in GameObject.Find("Cubes").transform){
                voxels[((int)child.position.x+150),((int)child.position.y+150),((int)child.position.z+150)] = 1;
            }
                
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();

            //The mesh produced is not optimal. There is one vert for each index.
            //Would need to weld vertices for better quality mesh.
            marching.Generate(voxels.Voxels, verts, indices);

            //Create the normals from the voxel.
            for (int i = 0; i < verts.Count; i++){
                //Presumes the vertex is in local space where
                //the min value is 0 and max is width/height/depth.
                Vector3 p = verts[i];

                float u = p.x / (width - 1.0f);
                float v = p.y / (height - 1.0f);
                float w = p.z / (depth - 1.0f);

                Vector3 n = voxels.GetNormal(u, v, w);

                normals.Add(n);
            }
            
            var position = new Vector3(-width / 2, -height / 2, -depth / 2);

            CreateMesh32(verts, normals, indices, position);

        }

        private static void CreateMesh32(List<Vector3> verts, List<Vector3> normals, List<int> indices, Vector3 position){
            Mesh mesh = new Mesh();
            mesh.indexFormat = IndexFormat.UInt32;
            mesh.SetVertices(verts);
            mesh.SetTriangles(indices, 0);

            if (normals.Count > 0)
                mesh.SetNormals(normals);
            else
                mesh.RecalculateNormals();

            mesh.RecalculateBounds();

            GameObject go = new GameObject("Mesh");
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            go.GetComponent<Renderer>().material = new Material(Shader.Find("Diffuse"));
            go.GetComponent<MeshFilter>().mesh = mesh;
            go.transform.localPosition = position;
        }

    }