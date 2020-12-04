using UnityEngine;
using CodeBaseExtensions;

/// <summary>
/// This is a base class for general UI components. It contains the minimal and most useful functnios.
/// </summary>
public class UIBase : MonoBehaviour {

    /// <summary>
    /// Is the application window currently been focused?
    /// </summary>
    private bool m_hasApplicationFocus = true;

    /// <summary>
    /// Is the application window currently been focused?
    /// </summary>
    /// <returns></returns>
    virtual public bool HasApplicationFocus() {
        return m_hasApplicationFocus;
    }

    /// <summary>
    /// Activate this UI. Make it visible and start working.
    /// </summary>
    virtual public void Activate() {
        gameObject.SetActive(true);
	}
	
	/// <summary>
	/// Deactivate this UI. Make it disappear and stop working.
	/// </summary>
	virtual public void Deactivate() {
        gameObject.SetActive(false);
	}

	/// <summary>
	/// Check if this UI is activated.
	/// </summary>
	/// <returns></returns>
    virtual public bool IsActiveInHierarchy() {
        return gameObject.activeInHierarchy;
    }

	/// <summary>
	/// Toggle activated this UI.
	/// </summary>
    virtual public void ToggleActivate() {
        if(gameObject.activeInHierarchy) {
            gameObject.SetActive(false);
        }else {
            gameObject.SetActive(true);
        }
    }

	/// <summary>
	/// Implement this function if you have any localized string reset when the system do a language changed refresh.
	/// </summary>
    virtual public void RefreshUILanguage() {

    }

    /// <summary>
    /// Get the seekable component if there's one on me.
    /// </summary>
    /// <returns>UISeekable component.</returns>
    public UISeekable Seekable {
		get {
            return GetComponent<UISeekable>();
        }
	}

	/// <summary>
	/// Is this guy seekable?
	/// </summary>
	/// <returns></returns>
	public bool IsSeekable() {
        UISeekable seekable = Seekable;
        if (seekable != null) {
            return seekable.canBeSeeked;
        }
        return false;
    }

    /// <summary>
    /// Seek a neighbor of me by direction.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public UIBase SeekNeighbor(UISeekable.SeekDirection direction, bool includeInactive = false) {
		UISeekable seekable = Seekable;
		if (seekable != null) {
			//By specific direction?
			if (seekable.useSpecifiedSeekTarget) {
                return seekable.GetSpecifiedNeighborBase(direction);
            }
            //By calculation.
            return seekable.SeekNeighborBase(direction, includeInactive);
        }
        return null;
    }

    /// <summary>
    /// Listen to the ApplicationFocus event of Unity.
    /// </summary>
    /// <param name="hasFocus"></param>
    protected void OnApplicationFocus(bool hasFocus) {
        m_hasApplicationFocus = hasFocus;
    }

    /// <summary>
    /// Try get the RectTransform directly.
    /// </summary>
    /// <returns></returns>
    public RectTransform GetRectTransform() {
        return this.GetComponent<RectTransform>();
    }

    /// <summary>
    /// Try get the Canvas of this object.
    /// </summary>
    /// <returns></returns>
    public Canvas GetCanvas() {
        return gameObject.GetComponentInParent<Canvas>();
    }

    /// <summary>
    /// Set the anchor of this UI.
    /// </summary>
    /// <param name="allign"></param>
    /// <param name="offsetX"></param>
    /// <param name="offsetY"></param>
    public void SetAnchor(AnchorPresets allign, int offsetX = 0, int offsetY = 0) {
        RectTransform rectTransform = GetRectTransform();
        if (rectTransform != null) {
            rectTransform.SetAnchor(allign, offsetX, offsetY);
        }
    }

    /// <summary>
    /// Set the pivot of this UI.
    /// </summary>
    /// <param name="preset"></param>
    public void SetPivot(PivotPresets preset) {
        RectTransform rectTransform = GetRectTransform();
        if (rectTransform != null) {
            rectTransform.SetPivot(preset);
        }
    }

    /// <summary>
    /// Try get the Canvas RectTransform of this object.
    /// </summary>
    /// <returns></returns>
    public RectTransform GetCanvasRectTransform() {
        Canvas canvas = gameObject.GetComponentInParent<Canvas>();
        if (canvas != null) {
            return canvas.GetComponent<RectTransform>();
        }
        return null;
    }

    /// <summary>
    /// Try get the render camera of the canvas of this object.
    /// </summary>
    /// <returns></returns>
    public Camera GetCanvasRenderCamera() {
        Canvas canvas = gameObject.GetComponentInParent<Canvas>();
        if (canvas != null) {
            return canvas.worldCamera;
        } else {
            return null;
        }
    }

    /// <summary>
    /// Try get the view port rect of this UI object.
    /// </summary>
    /// <returns></returns>
    public Rect GetViewPortRect() {
        RectTransform rectTransform = GetRectTransform();
        if (rectTransform != null) {
            return rectTransform.GetViewPortRect();
        } else {
            return new Rect(0, 0, 0, 0);
        }
        
    }

    /// <summary>
    /// Try get the view port rect of this UI object.
    /// </summary>
    /// <param name="marginTop"></param>
    /// <param name="marginBottom"></param>
    /// <param name="marginLeft"></param>
    /// <param name="marginRight"></param>
    /// <returns></returns>
    public Rect GetViewPortRect(float marginTop, float marginBottom, float marginLeft, float marginRight) {
        RectTransform rectTransform = GetRectTransform();
        if (rectTransform != null) {
            return rectTransform.GetViewPortRect(marginTop, marginBottom, marginLeft, marginRight);
        } else {
            return new Rect(0, 0, 0, 0);
        }
    }

    /// <summary>
    /// Get the world position of the left.
    /// </summary>
    /// <returns></returns>
    public float WorldLeft() {
        RectTransform rectTransform = GetRectTransform();
        if (rectTransform != null) {
            return rectTransform.WorldLeft();
        } else {
            return 0.0f;
        }
    }

    /// <summary>
    /// Get the world position of the top.
    /// </summary>
    /// <returns></returns>
    public float WorldTop() {
        RectTransform rectTransform = GetRectTransform();
        if (rectTransform != null) {
            return rectTransform.WorldTop();
        } else {
            return 0.0f;
        }
    }

    /// <summary>
    /// Get the world position of the right.
    /// </summary>
    /// <returns></returns>
    public float WorldRight() {
        RectTransform rectTransform = GetRectTransform();
        if (rectTransform != null) {
            return rectTransform.WorldRight();
        } else {
            return 0.0f;
        }
    }

    /// <summary>
    /// Get the world position of the bottom.
    /// </summary>
    /// <returns></returns>
    public float WorldBottom() {
        RectTransform rectTransform = GetRectTransform();
        if (rectTransform != null) {
            return rectTransform.WorldBottom();
        } else {
            return 0.0f;
        }
    }

}
