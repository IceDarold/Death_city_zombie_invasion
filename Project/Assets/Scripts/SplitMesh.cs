//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitMesh : MonoBehaviour
{
    public MeshFilter meshFilter;

    public Color[] gizmosColors;

    protected Mesh mesh;

    protected List<List<int>> finalMeshTrians;

    private Vector3[] test = new Vector3[5] {
        Vector3.zero,
        Vector3.one,
        Vector3.one * 2f,
        Vector3.one * 3f,
        Vector3.one * 4f
    };

    private int[] testTrain = new int[6] {
        0,
        1,
        2,
        1,
        2,
        4
    };

    private void Awake()
    {
        this.mesh = this.meshFilter.mesh;
    }

    [ContextMenu("MeshCollider 2 BoxCollider")]
    private void ChangeCollider()
    {
    }

    [ContextMenu("SplitMesh")]
    private void Test()
    {
        this.mesh = this.meshFilter.sharedMesh;
        Mesh mesh = new Mesh();
        mesh.vertices = this.mesh.vertices;
        mesh.triangles = this.mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
        for (int i = 0; i < vertices.Length; i++)
        {
            bool flag = false;
            IEnumerator enumerator = ((IDictionary)dictionary).Keys.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    int num = (int)enumerator.Current;
                    if (vertices[num] == vertices[i])
                    {
                        flag = true;
                        dictionary[num].Add(i);
                        break;
                    }
                }
            }
            finally
            {
                IDisposable disposable;
                if ((disposable = (enumerator as IDisposable)) != null)
                {
                    disposable.Dispose();
                }
            }
            if (!flag)
            {
                dictionary.Add(i, new List<int>());
                dictionary[i].Add(i);
            }
        }
        int[] array = new int[mesh.triangles.Length];
        for (int j = 0; j < mesh.triangles.Length; j++)
        {
            array[j] = mesh.triangles[j];
            foreach (KeyValuePair<int, List<int>> item in dictionary)
            {
                if (item.Value.Contains(mesh.triangles[j]))
                {
                    array[j] = item.Key;
                    break;
                }
            }
        }
        mesh.triangles = array;
        int[] triangles = mesh.triangles;
        List<List<int>> list = new List<List<int>>();
        for (int k = 0; k < triangles.Length; k += 3)
        {
            List<int> list2 = new List<int>();
            for (int l = 0; l < 3; l++)
            {
                if (list.Count == 0)
                {
                    break;
                }
                for (int m = 0; m < list.Count; m++)
                {
                    if (list[m].Contains(triangles[k + l]) && !list2.Contains(m))
                    {
                        list2.Add(m);
                    }
                }
            }
            if (list2.Count == 0)
            {
                List<int> list3 = new List<int>();
                list3.Add(triangles[k]);
                list3.Add(triangles[k + 1]);
                list3.Add(triangles[k + 2]);
                list.Add(list3);
            }
            else if (list2.Count == 1)
            {
                for (int n = 0; n < 3; n++)
                {
                    list[list2[0]].Add(triangles[k + n]);
                }
            }
            else
            {
                int index = list2[0];
                list2.RemoveAt(0);
                for (int num2 = 0; num2 < 3; num2++)
                {
                    list[index].Add(triangles[k + num2]);
                }
                list2.Sort((int a, int b) => a.CompareTo(b));
                for (int num3 = list2.Count - 1; num3 >= 0; num3--)
                {
                    for (int num4 = 0; num4 < list[list2[num3]].Count; num4++)
                    {
                        list[index].Add(list[list2[num3]][num4]);
                    }
                    list.RemoveAt(list2[num3]);
                }
            }
        }
        this.finalMeshTrians = list;
        this.gizmosColors = new Color[this.finalMeshTrians.Count];
        for (int num5 = 0; num5 < this.gizmosColors.Length; num5++)
        {
            this.gizmosColors[num5] = Color.white;
        }
        for (int num6 = 0; num6 < list.Count; num6++)
        {
            GameObject gameObject = new GameObject("SplitMesh_" + num6.ToString());
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            Mesh mesh2 = new Mesh();
            mesh2.name = "SplitMesh_" + num6.ToString();
            this.DeleteUnusedVertice(ref mesh2, this.mesh.vertices, list[num6], null, null, null);
            meshFilter.mesh = mesh2;
            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
            Bounds bounds = meshCollider.bounds;
            Vector3 max = bounds.max;
            float x = max.x;
            Vector3 min = bounds.min;
            float x2 = x - min.x;
            Vector3 max2 = bounds.max;
            float y = max2.y;
            Vector3 min2 = bounds.min;
            float y2 = y - min2.y;
            Vector3 max3 = bounds.max;
            float z = max3.z;
            Vector3 min3 = bounds.min;
            float z2 = z - min3.z;
            Vector3 max4 = bounds.max;
            float x3 = max4.x;
            Vector3 min4 = bounds.min;
            float x4 = (x3 + min4.x) / 2f;
            Vector3 max5 = bounds.max;
            float y3 = max5.y;
            Vector3 min5 = bounds.min;
            float y4 = (y3 + min5.y) / 2f;
            Vector3 max6 = bounds.max;
            float z3 = max6.z;
            Vector3 min6 = bounds.min;
            float z4 = (z3 + min6.z) / 2f;
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(x2, y2, z2);
            boxCollider.center = new Vector3(x4, y4, z4);
            boxCollider.isTrigger = true;
            gameObject.transform.parent = base.transform.parent;
            gameObject.transform.localPosition = base.transform.localPosition;
            gameObject.transform.localRotation = base.transform.localRotation;
            gameObject.transform.localScale = base.transform.localScale;
            meshFilter.sharedMesh.bounds = new Bounds
            {
                center = new Vector3(x4, y4, z4)
            };
            UnityEngine.Object.DestroyImmediate(meshCollider);
        }
    }

    [ContextMenu("DeleteUnusedVertice")]
    private void Testaa()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < this.testTrain.Length; i++)
        {
            list.Add(this.testTrain[i]);
        }
    }

    private void DeleteUnusedVertice(ref Mesh mesh, Vector3[] normalVert, int[] trians, Vector3[] normal, Vector2[] uv, Vector2[] uv2)
    {
        List<int> list = new List<int>();
        List<Vector3> list2 = new List<Vector3>();
        List<Vector2> list3 = new List<Vector2>();
        List<Vector2> list4 = new List<Vector2>();
        for (int i = 0; i < trians.Length; i++)
        {
            list.Add(trians[i]);
        }
        this.DeleteUnusedVertice(ref mesh, normalVert, list, normal, uv, uv2);
    }

    private void DeleteUnusedVertice(ref Mesh mesh, Vector3[] normalVert, List<int> trians, Vector3[] normals = null, Vector2[] uv = null, Vector2[] uv2 = null)
    {
        List<Vector3> list = new List<Vector3>();
        List<Vector3> list2 = new List<Vector3>();
        List<Vector2> list3 = new List<Vector2>();
        List<Vector2> list4 = new List<Vector2>();
        for (int i = 0; i < normalVert.Length; i++)
        {
            list.Add(normalVert[i]);
            if (normals != null)
            {
                list2.Add(normals[i]);
            }
            if (uv != null)
            {
                list3.Add(uv[i]);
            }
            if (uv2 != null)
            {
                list4.Add(uv2[i]);
            }
        }
        int num = 0;
        while (num < list.Count)
        {
            if (trians.Contains(num))
            {
                num++;
            }
            else
            {
                list.RemoveAt(num);
                if (normals != null)
                {
                    list2.RemoveAt(num);
                }
                if (uv != null)
                {
                    list3.RemoveAt(num);
                }
                if (uv2 != null)
                {
                    list4.RemoveAt(num);
                }
                for (int j = 0; j < trians.Count; j++)
                {
                    if (trians[j] >= num)
                    {
                        List<int> list5;
                        int index;
                        list5 = trians; index = j; (list5)[index] = list5[index] - 1;
                    }
                }
            }
        }
        mesh.vertices = list.ToArray();
        mesh.triangles = trians.ToArray();
        if (normals != null)
        {
            mesh.normals = list2.ToArray();
        }
        if (uv != null)
        {
            mesh.uv = list3.ToArray();
        }
        if (uv2 != null)
        {
            mesh.uv2 = list4.ToArray();
        }
    }

    [ContextMenu("Split Mesh By Sub-Mesh")]
    private void SplitBySubMesh()
    {
        this.mesh = this.meshFilter.sharedMesh;
        MeshRenderer component = this.meshFilter.gameObject.GetComponent<MeshRenderer>();
        Vector3[] vertices = this.mesh.vertices;
        if (this.mesh.subMeshCount > 1)
        {
            int[] triangles = this.mesh.GetTriangles(0);
            List<Vector3> list = new List<Vector3>();
            List<Vector2> list2 = new List<Vector2>();
            List<Vector2> list3 = new List<Vector2>();
            List<int> list4 = new List<int>();
            for (int i = 0; i < triangles.Length; i++)
            {
                if (!list4.Contains(triangles[i]))
                {
                    list4.Add(triangles[i]);
                }
                Vector3 vector = vertices[triangles[i]];
                list.Add(this.mesh.normals[triangles[i]]);
                list2.Add(this.mesh.uv[triangles[i]]);
                list3.Add(this.mesh.uv2[triangles[i]]);
            }
            UnityEngine.Debug.Log("Vertice num = " + list4.Count);
            GameObject gameObject = new GameObject("Split");
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            Mesh mesh = new Mesh();
            mesh.name = "Split";
            meshFilter.sharedMesh = mesh;
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            meshRenderer.sharedMaterial = component.sharedMaterials[0];
            mesh.normals = this.mesh.normals;
            mesh.uv = this.mesh.uv;
            mesh.uv2 = this.mesh.uv2;
            gameObject.transform.localScale = this.meshFilter.gameObject.transform.localScale;
            this.DeleteUnusedVertice(ref mesh, vertices, triangles, this.mesh.normals, this.mesh.uv, this.mesh.uv2);
        }
    }

    public void OnDrawGizmos()
    {
        if (this.finalMeshTrians != null && this.finalMeshTrians.Count != 0)
        {
            for (int i = 0; i < this.finalMeshTrians.Count; i++)
            {
                for (int j = 0; j < this.finalMeshTrians[i].Count; j++)
                {
                    Vector3 position = this.meshFilter.sharedMesh.vertices[this.finalMeshTrians[i][j]];
                    Vector3 center = base.transform.TransformPoint(position);
                    Gizmos.color = this.gizmosColors[i];
                    Gizmos.DrawSphere(center, 0.01f);
                    Gizmos.color = Color.white;
                }
            }
        }
    }
}


