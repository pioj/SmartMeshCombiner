using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


public class SmartMeshCombiner : MonoBehaviour {

    [MenuItem("Tools/SmartMeshCombiner/Combine selected GameObjects")]
    static void CombineSelected() {

        MeshFilter[] myFilters = Selection.GetFiltered<MeshFilter>(SelectionMode.Unfiltered);

        //mySelection.RemoveAll(t => t == null);
        //myFilters.RemoveAll(t => t == null);
        //mySelection.

        Debug.Log("Selected Objects: " + Selection.gameObjects.Length.ToString());

        float PivotsX = 0f;
        float PivotsY = 0f;

        var combine = new CombineInstance[myFilters.Length];
        for (int i = 0; i < myFilters.Length; i++)
        {
            combine[i].mesh = myFilters[i].sharedMesh;
            combine[i].transform = myFilters[i].transform.localToWorldMatrix;

            //saco los pivots
            PivotsX += myFilters[i].transform.position.x;
            PivotsY += myFilters[i].transform.position.y;

            //disable GO's for debuggin purposes
            myFilters[i].gameObject.SetActive(false); 
        }

        

        var go = new GameObject("TileGroup");
        var filter = go.AddComponent<MeshFilter>();
        var renderer = go.AddComponent<MeshRenderer>();

        Vector2 newPivot = new Vector2(PivotsX / myFilters.Length, PivotsY / myFilters.Length);

        //this line is in charge of the texture. Let's see whatever we can hack here...
        renderer.sharedMaterial = myFilters[0].GetComponent<MeshRenderer>().sharedMaterial;

        go.transform.position = newPivot;

        var mesh = new Mesh() { name = "tiles" };

        filter.sharedMesh = mesh;
        mesh.CombineMeshes(combine);

        //mark the new GO' as static
        go.isStatic = true;

        //IF WE WANT TO DELETE THE SELECTED GAMEOBJECTS...
        //RemoveSelected();

        //select the new gameobject
        Selection.activeGameObject = go;
    }


    //only when its ready and safe enough...
    private void RemoveSelected() {
        GameObject[] RemovalList = Selection.gameObjects;

        for (int i = 0; i < RemovalList.Length; i++) {
            Destroy(RemovalList[i]);
        }

        Selection.activeGameObject = null;

        RemovalList = null;
    }

}
