using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;
using System.Linq;

namespace pioj.ImageLevel
{
    public class Editor_SpriteCombiner : ScriptableWizard
    {

        SpriteRenderer[] mySprites;

        [MenuItem("Tools/SmartMeshCombiner/Combine selected Sprites")]
        static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<Editor_SpriteCombiner>("Combine selected Sprites", "Combine!");
        }

        void OnWizardCreate()
        {
            mySprites = Selection.GetFiltered<SpriteRenderer>(SelectionMode.Unfiltered);

            //EndObject part
            var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.name = "TileGroup";
            DestroyImmediate(go.GetComponent<MeshCollider>());
            var filter = go.GetComponent<MeshFilter>();
            var renderer = go.GetComponent<MeshRenderer>();

            /* esta parte da warnings, paso...
            var mat = AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");
            renderer.sharedMaterial = maty;
            renderer.sharedMaterial.enableInstancing = true;
            */

            Mesh finalMesh = new Mesh();
            var combineList = new CombineInstance[mySprites.Length];
            for (int i = 0; i < mySprites.Length; i++) {

                var spr = mySprites[i].sprite;
                Mesh result = new Mesh();
                
                //verts
                var verts = spr.vertices;
                var verts3d = new Vector3[verts.Length];
                for (int j = 0; j < verts.Length; j++)
                {
                    verts3d[j] = verts[j];
                }
                result.vertices = verts3d; 

                // uvs
                result.uv = SpriteUtility.GetSpriteUVs(spr, false);

                // colors and normals
                var colors = new Color[result.vertices.Length];
                var normals = new Vector3[result.vertices.Length];
                for (int j = 0; j < colors.Length; j++)
                {
                    colors[j] = mySprites[i].color;
                    normals[j] = Vector3.back;
                }
                result.colors = colors;
                result.normals = normals;

                // indicies
                var indicies2d = spr.triangles;
                var indicies = new int[indicies2d.Length];
                for (int j = 0; j < indicies.Length; j++)
                {
                    indicies[j] = indicies2d[j];
                }

                // finish it up
                result.SetIndices(indicies, MeshTopology.Triangles, 0);

                //
                combineList[i].mesh = result;
            }
            finalMesh.CombineMeshes(combineList);
            

        }

        void OnWizardUpdate()
        {
            helpString = Selection.gameObjects.Length.ToString() + " sprites selected.";
        }

        // When the user presses the "Apply" button OnWizardOtherButton is called.
        void OnWizardOtherButton() { }
    }
}
