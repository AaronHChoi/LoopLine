using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Automatic importer compatible with the Blender "Props" addon.
/// Detects _COL meshes and LOD hierarchy under an empty root object.
/// </summary>
public class PropsImporter : AssetPostprocessor
{
    void OnPostprocessModel(GameObject root)
    {
        var transforms = root.GetComponentsInChildren<Transform>(true);

        // ---------- COLLISIONS ----------
        foreach (var t in transforms)
        {
            string name = t.name.ToLower();

            if (!name.EndsWith("_col"))
                continue;

            var meshFilter = t.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null)
                continue;

            var renderer = t.GetComponent<MeshRenderer>();
            if (renderer != null)
                Object.DestroyImmediate(renderer);

            Mesh mesh = meshFilter.sharedMesh;

            if (IsSimpleBox(mesh))
            {
                var box = t.gameObject.AddComponent<BoxCollider>();
                box.center = mesh.bounds.center;
                box.size = mesh.bounds.size;
            }
            else
            {
                var meshCollider = t.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = mesh;
                meshCollider.convex = true;
            }
        }

        // ---------- LOD GROUPS (empty parent workflow) ----------
        foreach (var t in transforms)
        {
            // We only care about EMPTY root objects (no MeshRenderer)
            if (t.GetComponent<MeshRenderer>() != null)
                continue;

            var children = t.GetComponentsInChildren<MeshRenderer>(true);
            if (children.Length == 0)
                continue;

            var lod0 = children.FirstOrDefault(r => r.name.EndsWith("_LOD0") || r.name == t.name);
            var lod1 = children.FirstOrDefault(r => r.name.EndsWith("_LOD1"));
            var lod2 = children.FirstOrDefault(r => r.name.EndsWith("_LOD2"));

            if (lod0 == null || lod1 == null)
                continue; // Need at least LOD0 + LOD1 to be valid

            var lodGroup = t.gameObject.AddComponent<LODGroup>();

            var lods = new List<LOD>
            {
                new LOD(0.6f, new[] { lod0 })
            };

            if (lod1 != null)
                lods.Add(new LOD(0.3f, new[] { lod1 }));

            if (lod2 != null)
                lods.Add(new LOD(0.1f, new[] { lod2 }));

            lodGroup.SetLODs(lods.ToArray());
            lodGroup.RecalculateBounds();
        }
    }

    bool IsSimpleBox(Mesh mesh)
    {
        return mesh.vertexCount <= 24;
    }
}