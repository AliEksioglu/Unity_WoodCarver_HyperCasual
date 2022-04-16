using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakedPose : MonoBehaviour
{
    private static Dictionary<string, Mesh> batchedMeshes = new Dictionary<string, Mesh>();
private void Freeze() // This is called when all the animations in the block has finished
    {
        // Note that this function is only called once for a city block

        // Get all the SkinnedMeshRenderers that belong to this block
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            string key = renderers[i].sharedMesh.name; // assume name is unique for each mesh
            if (!batchedMeshes.ContainsKey(key)) // If no baked pose exists, make one!
            {
                Mesh m = new Mesh();
                Vector3 lScale = renderers[i].GetComponentInParent<Animator>().transform.localScale;

                // Ensure the scale of the mesh is one before baking as this will
                // be used with other buildings with different scale
                renderers[i].GetComponentInParent<Animator>().transform.localScale = Vector3.one;

                // Bake the current pose
                renderers[i].BakeMesh(m);
                batchedMeshes.Add(key, m);
                renderers[i].GetComponentInParent<Animator>().transform.localScale = lScale;
            }

            // Create a new GameObject to house our mesh
            GameObject staticMesh = new GameObject("StaticMeshInstance");

            // Setup its transforms
            staticMesh.transform.parent = renderers[i].transform.parent;
            staticMesh.transform.localPosition = renderers[i].transform.localPosition;
            staticMesh.transform.localRotation = renderers[i].transform.localRotation;
            staticMesh.transform.localScale = Vector3.one;

            // Setup the rendering components
            staticMesh.AddComponent<MeshFilter>().sharedMesh = batchedMeshes[key];
            staticMesh.AddComponent<MeshRenderer>().sharedMaterial = renderers[i].sharedMaterial;

            // Disable shadows so Unity can get batching
            staticMesh.GetComponent<MeshRenderer>().receiveShadows = false;
            staticMesh.GetComponent<MeshRenderer>().shadowCastingMode =
                UnityEngine.Rendering.ShadowCastingMode.Off;
            // Turn off the SkinnedMeshRenderer
            renderers[i].enabled = false;
        }

    }

    private void UnFreeze() // Called when the block needs to be animated again for disappearing
    {
        // The block will be destroyed soon, so we simply toggle the visibility of the two renderers
        foreach (var m in GetComponentsInChildren<MeshRenderer>())
        {
            m.enabled = false;
        }
        foreach (var m in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            m.enabled = true;
        }
    }
}
