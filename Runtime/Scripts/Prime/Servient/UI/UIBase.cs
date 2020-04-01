using UnityEngine;

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

}
