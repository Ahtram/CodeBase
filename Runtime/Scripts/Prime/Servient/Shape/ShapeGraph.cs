using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeBaseExtensions;
using System;

/// <summary>
/// A convenient component for drawing Shape (data) base graph.
/// Also proveide convenient way to listen to trigger event.
/// Use the Create() / Destroy Staic API to proper instantiate stuffs.
/// </summary>
public class ShapeGraph : MonoBehaviour {

    public Action<Collider2D> onTriggerEnter2D;
    public Action<Collider2D> onTriggerStay2D;
    public Action<Collider2D> onTriggerExit2D;

    //預設 Mesh Mat.
    private const string DEFAILT_MESHRENDERER_MAT = "Materials/ShapeGraphMeshDefault";

    //預設 Line Mat.
    private const string DEFAILT_LINERENDERER_MAT = "Materials/ShapeGraphLineDefault";

    //存下動態產出的 materials.
    private Material m_meshMat = null;
    private Material m_outlineMat = null;

    //Cached stuffs.
    private float m_alpha = 1.0f;
    private Color m_meshColor = Color.white;
    private Color m_outlineColor = Color.white;

    //初始化 ShapeGraph
    public void Setup(Shape shape, bool colliderIsTrigger = true, bool attachCollider = true, bool attachMeshRenderer = true, bool attachLineRenderer = true) {
        //Attach collider / mesh / lineRenderer.
        if (attachCollider) {
            shape.AttachCollider2D(gameObject, colliderIsTrigger);
        }

        if (attachMeshRenderer) {
            shape.AttachMeshRenderer(gameObject);
        }

        if (attachLineRenderer) {
            shape.AttachLineRenderer(gameObject);
        }

        // shape.AttachAllComponents(gameObject, colliderIsTrigger);

        if (shape.type != Shape.Type.None) {
            //Mesh renderer material.
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null) {
                m_meshMat = new Material(Resources.Load(DEFAILT_MESHRENDERER_MAT) as Material);
                meshRenderer.material = m_meshMat;
                m_meshColor = m_meshMat.color;
            }

            //Line renderer material.
            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            if (lineRenderer != null) {
                m_outlineMat = new Material(Resources.Load(DEFAILT_LINERENDERER_MAT) as Material);
                lineRenderer.material = m_outlineMat;
                m_outlineColor = m_outlineMat.color;
            }
        }

        if (shape.HasACenterPosition()) {
            transform.localPosition = shape.CenterPosition();
        }
    }

    //設定 Mesh 顏色
    public Color MeshColor {
        set {
            m_meshColor = value;
            UpdateColorAlpha();
        }
    }

    //設定 Line 顏色
    public Color OutLineColor {
        set {
            m_outlineColor = value;
            UpdateColorAlpha();
        }
    }

    //設定整體 Aplha (0.0f ~ 1.0f)
    public float Alpha {
        set {
            m_alpha = value;
            UpdateColorAlpha();
        }
        get {
            return m_alpha;
        }
    }

    //設定ShapeGraph是否可視
    public void SetVisible(bool b) {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer != null) {
            lineRenderer.enabled = b;
        }

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null) {
            meshRenderer.enabled = b;
        }
    }

    //更新顏色
    private void UpdateColorAlpha() {
        if (m_meshMat != null) {
            m_meshMat.color = new Color(m_meshColor.r, m_meshColor.g, m_meshColor.b, m_meshColor.a * m_alpha);
        }

        if (m_outlineMat != null) {
            m_outlineMat.color = new Color(m_outlineColor.r, m_outlineColor.g, m_outlineColor.b, m_outlineColor.a * m_alpha);
        }
    }

    //設定 Line 寬度
    public void SetLineWidth(float start, float end) {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer != null) {
            lineRenderer.startWidth = start;
            lineRenderer.endWidth = end;
        }
    }

    //設定排序層
    public void SetSortingLayer(string sortingLayerName, int sortingOrder) {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer != null) {
            lineRenderer.sortingLayerName = sortingLayerName;
            lineRenderer.sortingOrder = 0;
        }

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null) {
            meshRenderer.sortingLayerName = sortingLayerName;
            meshRenderer.sortingOrder = 0;
        }
    }

    //清除 ShapeGraph 上的東西
    public void Clear() {
        onTriggerEnter2D = null;
        onTriggerStay2D = null;
        onTriggerExit2D = null;
        //[Important!]: Destroy all dynamic materials.
        UnityEngine.Object.Destroy(m_meshMat);
        m_meshMat = null;
        UnityEngine.Object.Destroy(m_outlineMat);
        m_outlineMat = null;
    }

    void OnTriggerEnter2D(Collider2D collider2D) {
        onTriggerEnter2D?.Invoke(collider2D);
    }

    void OnTriggerStay2D(Collider2D collider2D) {
        onTriggerStay2D?.Invoke(collider2D);
    }

    void OnTriggerExit2D(Collider2D collider2D) {
        onTriggerExit2D?.Invoke(collider2D);
    }

    //============ Static APIs =============

    //Create (Instantiate) a new ShapeGraph object.
    static public ShapeGraph Create(string gameObjectName, GameObject parent, Shape shape, Vector3 localPosision = new Vector3(), bool colliderIsTrigger = true, bool attachCollider = true, bool attachMeshRenderer = true, bool attachLineRenderer = true) {
        GameObject GO = new GameObject(gameObjectName);
        ShapeGraph shapeGraph = GO.AddComponent<ShapeGraph>();
        if (shapeGraph != null) {
            GO.transform.SetParent(parent.transform);
            GO.SetLayer(parent.layer);
            GO.transform.localPosition = localPosision;
            shapeGraph.Setup(shape, colliderIsTrigger, attachCollider, attachMeshRenderer, attachLineRenderer);
            return shapeGraph;
        } else {
            return null;
        }
    }

    //Destroy a ShapeGraph GameObject.
    static public void Destroy(ShapeGraph shapeGraph) {
        if (shapeGraph != null) {
            shapeGraph.Clear();
            GameObject.Destroy(shapeGraph.gameObject);
        }
    }

}
