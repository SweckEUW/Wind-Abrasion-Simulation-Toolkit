using UnityEngine;
using UnityEditor;

public class PreperationTool : EditorWindow{

    [MenuItem("Tools/Load Voxel Data")]
    public static void ShowWindow(){
        GetWindow(typeof(PreperationTool));
    }

    private void OnGUI(){
        // Load Voxel
        GUILayout.Label("Load Voxel File", EditorStyles.boldLabel);


        if(GUILayout.Button("Import Voxel")){
            if(GameObject.Find("Cubes"))
                if(!EditorUtility.DisplayDialog("Voxel already in scene. Want to replace existing Voxels?","Voxel already in scene. Want to replace existing Voxels?","ok","cancle"))
                    return;
                else
                    DestroyImmediate(GameObject.Find("Cubes"));

            string voxelFilePath = EditorUtility.OpenFilePanel("Overwrite with png", "", "txt");
            if(voxelFilePath != "")
                ImportVoxel.Import(voxelFilePath);
        }
        
        // Export Voxel
        GUILayout.Label("Export Mesh", EditorStyles.boldLabel);

        if(GUILayout.Button("Export Mesh")){
            if(GameObject.Find("Cubes") == null)
                EditorUtility.DisplayDialog("No Voxels in scene found","No Voxels in scene found","ok");
            else{
                ConvertVoxelToMesh.Convert();
                ExportMesh.Export();
            }  
        }
    }
}
