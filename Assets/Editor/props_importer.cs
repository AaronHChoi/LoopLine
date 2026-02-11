using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Automatic importer compatible with the Blender "Props" addon.
/// Builds colliders, creates an EXACT LOD distribution, and applies Static Flags
/// to the FULL hierarchy reliably on import.
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

            if (!name.Contains("_col"))
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

        // ---------- LOD GROUPS (STRICT VALUES) ----------
        foreach (var t in transforms)
        {
            // Only EMPTY root objects
            if (t.GetComponent<MeshRenderer>() != null)
                continue;

            var children = t.GetComponentsInChildren<MeshRenderer>(true);
            if (children.Length == 0)
                continue;

            var lod0 = children.FirstOrDefault(r => r.name == t.name || r.name.EndsWith("_LOD0"));
            var lod1 = children.FirstOrDefault(r => r.name.EndsWith("_LOD1"));
            var lod2 = children.FirstOrDefault(r => r.name.EndsWith("_LOD2"));

            if (lod0 == null || lod1 == null || lod2 == null)
                continue;

            var lodGroup = t.gameObject.GetComponent<LODGroup>();
            if (lodGroup == null)
                lodGroup = t.gameObject.AddComponent<LODGroup>();

            var lods = new LOD[]
            {
                new LOD(0.30f, new[] { lod0 }),
                new LOD(0.10f, new[] { lod1 }),
                new LOD(0.01f, new[] { lod2 })
            };

            lodGroup.fadeMode = LODFadeMode.CrossFade;
            lodGroup.animateCrossFading = true;

            lodGroup.SetLODs(lods);
            lodGroup.RecalculateBounds();
        }

        // ---------- STATIC FLAGS (APPLY ALWAYS, NOT ONLY IF LOD EXISTS) ----------
        ApplyStaticFlagsRecursively(root);
    }

    void ApplyStaticFlagsRecursively(GameObject root)
    {
        var all = root.GetComponentsInChildren<Transform>(true);

        foreach (var t in all)
        {
            GameObjectUtility.SetStaticEditorFlags(
                t.gameObject,
                StaticEditorFlags.BatchingStatic |
                StaticEditorFlags.ContributeGI |
                StaticEditorFlags.OccluderStatic |
                StaticEditorFlags.OccludeeStatic |
                StaticEditorFlags.ReflectionProbeStatic
            );
        }

        // ALSO ensure the root itself is static
        GameObjectUtility.SetStaticEditorFlags(
            root,
            StaticEditorFlags.BatchingStatic |
            StaticEditorFlags.ContributeGI |
            StaticEditorFlags.OccluderStatic |
            StaticEditorFlags.OccludeeStatic |
            StaticEditorFlags.ReflectionProbeStatic
        );
    }

    bool IsSimpleBox(Mesh mesh)
    {
        return mesh.vertexCount <= 24;
    }
}
