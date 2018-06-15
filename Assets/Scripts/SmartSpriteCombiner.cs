using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


public class SmartSpriteCombiner : MonoBehaviour {

    [MenuItem("Tools/SmartSpriteCombiner/Combine selected Sprites")]
    static void CombineSelected() {

        SpriteRenderer[] mySprites = Selection.GetFiltered<SpriteRenderer>(SelectionMode.Unfiltered);

        //mySelection.RemoveAll(t => t == null);
        //myFilters.RemoveAll(t => t == null);
        //mySelection.

        Debug.Log("Selected Sprites: " + Selection.gameObjects.Length.ToString());

        float PivotsX = 0f;
        float PivotsY = 0f;

        
        for (int i = 0; i < mySprites.Length; i++)
        {
            //saco los pivots
            PivotsX += mySprites[i].transform.position.x;
            PivotsY += mySprites[i].transform.position.y;

            var temp = mySprites[i].sprite.vertices;
            

            //disable GO's for debuggin purposes
            mySprites[i].gameObject.SetActive(false);
        }

        

        var go = new GameObject("SpriteGroup");
        var Filter = go.AddComponent<MeshFilter>();
        var Renderer = go.AddComponent<MeshRenderer>();

        Vector2 newPivot = new Vector2(PivotsX / mySprites.Length, PivotsY / mySprites.Length);




        go.transform.position = newPivot;

        
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
