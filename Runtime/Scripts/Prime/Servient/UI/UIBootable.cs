using UnityEngine;
using System;

/// <summary>
/// Contains an animator which could do "come in" and "go out" animation.
/// </summary>
public class UIBootable : UIBase {

    //On complete actions.
    public Action onComeInComplete;
    public Action onGoOutComplete;

    /// <summary>
    /// This animator SHOULD BE PROVIDED with specific rule.
    /// It should contains a bool param "In" and a trigger param "Work" and depend on them to do the animations.
    /// Look for exist example in the project for further understand is adviced.
    /// </summary>
    public Animator animator;

	/// <summary>
	/// A logic bool which should be used to block user inputs when it's false.
	/// </summary>
	protected bool m_enableOperation = false;

	/// <summary>
	/// Logic booleans which indicated this UI is currently inside(displaying) or outside(hiding).
	/// </summary>
	protected bool m_isInside = false;
    protected bool m_isOutside = true;

    /// <summary>
    /// Turn on the enable user operation flag.
    /// </summary>
    public void EnableOperation() {
        m_enableOperation = true;
    }

    /// <summary>
    /// Turn off the enable user operation flag.
    /// </summary>
    public void DisableOperation() {
        m_enableOperation = false;
    }

    /// <summary>
    /// If this UI is currently inside user's view.
    /// </summary>
    /// <returns></returns>
    public bool IsInside() {
        return m_isInside;
    }

    /// <summary>
    /// If this UI is currently outside user's view.
    /// </summary>
    /// <returns></returns>
    public bool IsOutside() {
        return m_isOutside;
    }

	//-----------------------------------------------

    /// <summary>
    /// Bring in this UI.
    /// </summary>
	virtual public void ComeIn(float speed = 1.0f) {
        Activate();
        if (!animator.GetBool("In")) {
            m_isOutside = false;
            animator.speed = speed;
            animator.SetBool("In", true);
            animator.SetTrigger("Work");
        }
    }

    /// <summary>
    /// Bring in this UI.
    /// </summary>
    virtual public void ComeIn() {
        ComeIn(1.0f);
    }

    /// <summary>
    /// Animation event for come in complete.
    /// </summary>
    virtual public void OnComeInComplete() {
        m_isInside = true;
        EnableOperation();

        onComeInComplete?.Invoke();
    }

    /// <summary>
    /// Send away this UI.
    /// </summary>
    virtual public void GoOut(float speed = 1.0f) {
        if (IsActiveInHierarchy()) {
            if (animator.GetBool("In")) {
                m_isInside = false;
                DisableOperation();
                animator.speed = speed;
                animator.SetBool("In", false);
                animator.SetTrigger("Work");
            }
        }
    }

    /// <summary>
    /// Send away this UI.
    /// </summary>
    virtual public void GoOut() {
        GoOut(1.0f);
    }

    /// <summary>
    /// Animation event for go out complete.
    /// </summary>
    virtual public void OnGoOutComplete() {
		Deactivate();
		m_isOutside = true;

        onGoOutComplete?.Invoke();
    }

}
