using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;
using UBase;
using CodeBaseExtensions;

//A helper component which listen to UIBehavior events.
public class UIBehaviorEvents : UIBehaviour {

    public UnityEventRectTransform onRectTransformDimensionsChange;

    override protected void OnRectTransformDimensionsChange() {
        onRectTransformDimensionsChange.Invoke(this.GetRectTransform());
    }

}
