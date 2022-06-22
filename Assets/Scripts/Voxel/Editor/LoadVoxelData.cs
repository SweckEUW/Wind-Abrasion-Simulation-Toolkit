using UnityEngine;
using System.IO;
using System;
using UnityEditor;

 
public class LoadVoxelData : EditorWindow{

    string voxelFilePath = "";

    [MenuItem("Tools/Load Voxel Data")]
    public static void ShowWindow(){
        GetWindow(typeof(LoadVoxelData));
    }

    private void OnGUI(){
        // Load Voxel
        GUILayout.Label("Load Voxel File", EditorStyles.boldLabel);

        if(GUILayout.Button("Select Voxel File"))
            voxelFilePath = EditorUtility.OpenFilePanel("Overwrite with png", "", "txt");
        

        if(GUILayout.Button("Spawn Voxel")){
            if(voxelFilePath == ""){
                EditorUtility.DisplayDialog("Please select a voxel file","Please select a voxel file","ok");
            }else{
                ReadVoxelFile(); 
            }
        }

        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color (0.5f,0.5f,0.5f, 1));
        
        // Export Voxel
        GUILayout.Label("Export Mesh", EditorStyles.boldLabel);

        if(GUILayout.Button("Export Mesh")){
            if(GameObject.Find("Cubes") == null)
                EditorUtility.DisplayDialog("No Voxels in scene found","No Voxels in scene found","ok");
            else
                Debug.Log("Smooth");
                // Example.Convert();
        }
    }

    public void ReadVoxelFile(){
        if(GameObject.Find("Cubes"))
            if(!EditorUtility.DisplayDialog("Voxel already in scene. Want to replace existing Voxels?","Voxel already in scene. Want to replace existing Voxels?","ok","cancle"))
                return;
            else
                DestroyImmediate(GameObject.Find("Cubes"));

        StreamReader reader = new StreamReader(voxelFilePath);
        
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
