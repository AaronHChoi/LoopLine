using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// FINAL production-ready Props importer.
///
/// Rules:
/// - If the model has NO _LOD and NO _COL → importer does NOTHING.
/// - If it has _COL → converts to real colliders and places them under a Collision root (EMPTY preserved).
/// - If it has full LOD chain → builds exact LODGroup (100 → 30 → 10 → 1).
/// - Creates Render and Collision roots only when processing is needed.
/// - Applies Static flags only when processing occurs.
/// </summary>
public class PropsImporter : AssetPostprocessor
{
    void OnPostprocessModel(GameObject root)
    {
        var transforms = root.GetComponentsInChildren<Transform>(true);

        bool hasLOD0 = transforms.Any(t => t.name.EndsWith("_LOD0") || t.name == root.name);
        bool hasLOD1 = transforms.Any(t => t.name.EndsWith("_LOD1"));
        bool hasLOD2 = transforms.Any(t => t.name.EndsWith("_LOD2"));

        bool hasFullLOD = hasLOD0 && hasLOD1 && hasLOD2;
        bool hasCOL = transforms.Any(t => t.name.ToLower().Contains("_col"));

        // ---------- DO ABSOLUTELY NOTHING IF NO FULL LOD AND NO COLLISION ----------
        if (!hasFullLOD && !hasCOL)
            return;

        // ---------- CREATE RENDER ROOT ONLY IF LODs EXIST ----------
        Transform renderRoot = null;
        if (hasFullLOD)
        {
            renderRoot = new GameObject("Render").transform;
            renderRoot.SetParent(root.transform, false);
        }

        // ---------- CREATE COLLISION ROOT ONLY IF COLLIDERS EXIST ----------
        Transform collisionRoot = null;
        if (hasCOL)
        {
            collisionRoot = new GameObject("Collision").transform;
            collisionRoot.SetParent(root.transform, false);
        }

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

            // Ensure collider stays under Collision EMPTY
            if (collisionRoot != null)
                t.SetParent(collisionRoot, true);

            // Remove renderer
            var renderer = t.GetComponent<MeshRenderer>();
            if (renderer != null)
                Object.DestroyImmediate(renderer);

            // Create collider
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

            // Remove MeshFilter AFTER assigning mesh
            Object.DestroyImmediate(meshFilter);
        }

        // ---------- RENDER MESHES ----------
        var meshRenderers = root.GetComponentsInChildren<MeshRenderer>(true)
            .Where(r => !r.name.ToLower().Contains("_col"))
            .ToArray();

        if (renderRoot != null)
        {
            foreach (var r in meshRenderers)
                r.transform.SetParent(renderRoot, true);
        }

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
