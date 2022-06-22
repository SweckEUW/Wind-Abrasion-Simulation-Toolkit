using UnityEngine;
using System.IO;
using System;
using UnityEditor;

    public static class ImportVoxel{

        public static void Import(string file){
            StreamReader reader = new StreamReader(file);
            
            GameObject cubeParent = new GameObject("Cubes");

            while(!reader.EndOfStream){
                string line = reader.ReadLine();
                if(!line.Contains("#")){
                    string[] data = line.Split(" ");
                    int x = Int32.Parse(data[0]);
                    int y = Int32.Parse(data[1]);
                    int z = Int32.Parse(data[2]);
                    string color = data[3];
                    
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(x,y,z);
                    cube.transform.parent = cubeParent.transform;
                }
            }

            cubeParent.transform.Rotate(-90,0,0);

            reader.Close();
        }

    }