using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// FINAL production‑ready Props importer.
///
/// Features:
/// - Detects _COL meshes and converts to real colliders
/// - Removes MeshFilter from collision objects (clean production setup)
/// - Builds exact LODGroup distribution (100 → 30 → 10 → 1 culled)
/// - Creates professional hierarchy:
///     Root
///       └─ Render (LOD meshes)
/// - Colliders remain directly under Root (no unnecessary empty)
/// - Applies Static flags to the FULL hierarchy
/// </summary>
public class PropsImporter : AssetPostprocessor
{
    void OnPostprocessModel(GameObject root)
    {
        var transforms = root.GetComponentsInChildren<Transform>(true);

        // ---------- CREATE RENDER ROOT ----------
        var renderRoot = new GameObject("Render").transform;
        renderRoot.SetParent(root.transform, false);

        // ---------- COLLISIONS ----------
        foreach (var t in transforms)
        {
            string name = t.name.ToLower();

            if (!name.Contains("_col"))
                continue;

            var meshFilter = t.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null)
                continue;

            Mesh mesh = meshFilter.sharedMesh;

            // Keep collider directly under root (no Collision empty)
            t.SetParent(root.transform, true);

            // Remove renderer
            var renderer = t.GetComponent<MeshRenderer>();
            if (renderer != null)
                Object.DestroyImmediate(renderer);

            // Create proper collider
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

            // Remove MeshFilter AFTER assigning mesh to collider
            Object.DestroyImmediate(meshFilter);
        }

        // ---------- RENDER MESHES + LOD DETECTION ----------
        var meshRenderers = root.GetComponentsInChildren<MeshRenderer>(true)
            .Where(r => !r.name.ToLower().Contains("_col"))
            .ToArray();

        foreach (var r in meshRenderers)
            r.transform.SetParent(renderRoot, true);

        var lod0 = meshRenderers.FirstOrDefault(r => r.name.EndsWith("_LOD0") || r.name == root.name);
        var lod1 = meshRenderers.FirstOrDefault(r => r.name.EndsWith("_LOD1"));
        var lod2 = meshRenderers.FirstOrDefault(r => r.name.EndsWith("_LOD2"));

        // ---------- LOD GROUP ----------
        if (lod0 != null && lod1 != null && lod2 != null)
        {
            var lodGroup = root.GetComponent<LODGroup>();
            if (lodGroup == null)
                lodGroup = root.AddComponent<LODGroup>();

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

        // ---------- STATIC FLAGS ----------
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
