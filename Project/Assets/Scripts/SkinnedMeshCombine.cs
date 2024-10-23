//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SkinnedMeshCombine : MonoBehaviour
{
    public Transform boneRoot;

    private SkinnedMeshRenderer mRender;

    private void Awake()
    {
        this.Combine();
    }

    [ContextMenu("测试生成")]
    public void Combine()
    {
        if (!((Object)this.boneRoot == (Object)null))
        {
            float realtimeSinceStartup = Time.realtimeSinceStartup;
            SkinnedMeshRenderer[] componentsInChildren = this.boneRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            CombineInstance[] array = new CombineInstance[componentsInChildren.Length];
            Texture2D texture2D = null;
            Material material = null;
            SkinnedMeshRenderer skinnedMeshRenderer = this.boneRoot.gameObject.GetComponent<SkinnedMeshRenderer>();
            if ((Object)skinnedMeshRenderer != (Object)null && (Object)skinnedMeshRenderer.sharedMesh != (Object)null)
            {
                UnityEngine.Object.DestroyImmediate(skinnedMeshRenderer.sharedMesh);
                skinnedMeshRenderer.sharedMesh = null;
            }
            if ((Object)skinnedMeshRenderer == (Object)null)
            {
                skinnedMeshRenderer = this.boneRoot.gameObject.AddComponent<SkinnedMeshRenderer>();
            }
            skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            skinnedMeshRenderer.receiveShadows = false;
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                if ((Object)material == (Object)null)
                {
                    material = componentsInChildren[i].sharedMaterial;
                }
                if ((Object)texture2D == (Object)null && (Object)componentsInChildren[i].sharedMaterial.mainTexture != (Object)null)
                {
                    texture2D = (componentsInChildren[i].sharedMaterial.mainTexture as Texture2D);
                }
            }
            List<Transform> list = new List<Transform>();
            for (int j = 0; j < componentsInChildren.Length; j++)
            {
                if (!((Object)componentsInChildren[j].transform == (Object)this.boneRoot))
                {
                    Mesh mesh = this.CreatMeshWithMesh(componentsInChildren[j].sharedMesh);
                    list.AddRange(componentsInChildren[j].bones);
                    array[j].mesh = mesh;
                    array[j].transform = componentsInChildren[j].transform.localToWorldMatrix;
                    componentsInChildren[j].enabled = false;
                }
            }
            Mesh mesh2 = new Mesh();
            mesh2.CombineMeshes(this.CreatNewCombine(array), true, false);
            material.mainTexture = texture2D;
            skinnedMeshRenderer.bones = list.ToArray();
            skinnedMeshRenderer.rootBone = this.boneRoot;
            skinnedMeshRenderer.sharedMesh = mesh2;
            skinnedMeshRenderer.sharedMaterial = material;
            array = null;
            componentsInChildren = null;
            list.Clear();
            list = null;
        }
    }

    private Mesh CreatMeshWithMesh(Mesh mesh)
    {
        Mesh mesh2 = new Mesh();
        mesh2.vertices = mesh.vertices;
        mesh2.name = mesh.name;
        mesh2.uv = mesh.uv;
        mesh2.uv2 = mesh.uv2;
        mesh2.uv2 = mesh.uv2;
        mesh2.bindposes = mesh.bindposes;
        mesh2.boneWeights = mesh.boneWeights;
        mesh2.bounds = mesh.bounds;
        mesh2.colors = mesh.colors;
        mesh2.colors32 = mesh.colors32;
        mesh2.normals = mesh.normals;
        mesh2.subMeshCount = mesh.subMeshCount;
        mesh2.tangents = mesh.tangents;
        mesh2.triangles = mesh.triangles;
        return mesh2;
    }

    private CombineInstance[] CreatNewCombine(CombineInstance[] combine)
    {
        List<CombineInstance> list = new List<CombineInstance>();
        for (int i = 0; i < combine.Length; i++)
        {
            if ((Object)combine[i].mesh != (Object)null)
            {
                list.Add(combine[i]);
            }
        }
        return list.ToArray();
    }
}


