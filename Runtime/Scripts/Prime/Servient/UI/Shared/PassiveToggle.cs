using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a simple passive toggle for replace UnityEngine.UI.Button
/// We want this because of there's no way to get rid of the selectable part of built-in UGUI.
/// Note that this component should reply on a external EventTrigger to make callback.
/// </summary>
[RequireComponent(typeof(Image))]
public class PassiveToggle : UIBase {

	public Image toggleImage;

	public Sprite toggleOnSprite;
    public Sprite toggleOffSprite;

    public Color toggleOnColor;
    public Color toggleOffColor;

    virtual public void SetToggleOn() {
        if(toggleImage != null && toggleOnSprite != null) {
            toggleImage.sprite = toggleOnSprite;
            toggleImage.color = toggleOnColor;
        } else {
            Console.OutWarning("Oops! It seems like you forget to something on [" + gameObject.name + "]");
        }
    }

    virtual public void SetToggleOff() {
        if (toggleImage != null && toggleOffSprite != null) {
            toggleImage.sprite = toggleOffSprite;
            toggleImage.color = toggleOffColor;
        } else {
            Console.OutWarning("Oops! It seems like you forget to something on [" + gameObject.name + "]");
        }
    }

    public bool IsToggledOn() {
        if (toggleImage != null && toggleOnSprite != null) {
            return (toggleImage.sprite == toggleOnSprite) ? (true) : (false);
        }
        return false;
    }

}
