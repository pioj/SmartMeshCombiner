// The MIT License (MIT) - https://gist.github.com/bcatcho/1926794b7fd6491159e7
// Copyright (c) 2015 Brandon Catcho
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.Sprites;
#endif

namespace CatchCo
{
   [RequireComponent(typeof(MeshRenderer))]
   [RequireComponent(typeof(MeshFilter))]
   public class SpriteCombiner : MonoBehaviour
   {
      public Mesh CombinedMesh;
   }


   #if UNITY_EDITOR
   [CustomEditor(typeof(SpriteCombiner))]
   public class SpriteCombinerInspector: Editor
   {
      public override void OnInspectorGUI ()
      {
         DrawDefaultInspector ();
         
         if (GUILayout.Button ("Combine Meshes")) {
            CombineSprites ((SpriteCombiner)target);
         }

         if (GUILayout.Button ("Edit")) {
            EnableEditMode ((SpriteCombiner)target);
         }
      }

      public void EnableEditMode (SpriteCombiner combiner)
      {
         combiner.GetComponent<Renderer>().enabled = false;
         var sprites = GetDirectChildSpriteRenderers (combiner);
         foreach (var s in sprites) {
            s.gameObject.SetActive (true);
            s.gameObject.hideFlags = HideFlags.None;
         }
      }

      private IList<SpriteRenderer> GetDirectChildSpriteRenderers (MonoBehaviour behaviour)
      {
         var parentTx = behaviour.transform;
         var spriteRenderers = behaviour.GetComponentsInChildren<SpriteRenderer> (true);
         var result = spriteRenderers.Where (s => s.transform.parent == parentTx).ToList ();
         return result;
      }

      public void CombineSprites (SpriteCombiner combiner)
      {
         try {
            var sprites = GetDirectChildSpriteRenderers (combiner);
            CombineSprites (ref combiner.CombinedMesh, sprites);
            combiner.GetComponent<MeshFilter> ().sharedMesh = combiner.CombinedMesh;
            foreach (var s in sprites) {
               s.gameObject.hideFlags = HideFlags.HideInHierarchy;
               s.gameObject.SetActive (false);
            }
            combiner.GetComponent<Renderer>().enabled = true;
         } catch (Exception e) {
            Debug.LogException (e);
         }
      }
      
      public void CombineSprites (ref Mesh result, IList<SpriteRenderer> sprites)
      {
         if (result == null) {
            result = new Mesh ();
         }
         var combineList = new CombineInstance[sprites.Count];
         var room = ((SpriteCombiner)target);
         var roomTx = room.transform;
         for (int i = 0; i < combineList.Length; i++) {
            var spriteTx = sprites [i].transform;
            combineList [i] = new CombineInstance ()
            {
               mesh = SpriteToMesh(sprites[i]),
               transform = roomTx.worldToLocalMatrix * spriteTx.localToWorldMatrix
            };
         }
         
         result.Clear ();
         result.CombineMeshes (combineList);
      }
      
      private Mesh SpriteToMesh (SpriteRenderer spriteRenderer)
      {
         var sprite = spriteRenderer.sprite;
         
         Mesh result = new Mesh ();
         result.hideFlags = HideFlags.HideAndDontSave;
         result.Clear ();
         
         //verts
         var verts = sprite.vertices; 
         var verts3d = new Vector3[verts.Length];
         for (int i = 0; i < verts.Length; i++) {
            verts3d [i] = verts [i];
         }
         result.vertices = verts3d;
         
         // uvs
         result.uv = SpriteUtility.GetSpriteUVs (sprite, false);
         
         // colors and normals
         var colors = new Color[result.vertices.Length];
         var normals = new Vector3[result.vertices.Length];
         for (int i = 0; i < colors.Length; i++) {
            colors [i] = spriteRenderer.color;
            normals [i] = Vector3.back;
         }
         result.colors = colors;
         result.normals = normals;
         
         // indicies
         var indicies2d = sprite.triangles;
         var indicies = new int[indicies2d.Length];
         for (int i = 0; i < indicies.Length; i++) {
            indicies [i] = indicies2d [i];
         }
         
         // finish it up
         result.SetIndices (indicies, MeshTopology.Triangles, 0);
         return result;
      }
   }
   #endif
}
