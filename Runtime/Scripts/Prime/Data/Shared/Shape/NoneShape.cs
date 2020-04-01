using System.Xml.Serialization;
using System;
using UnityEngine;

[XmlRoot("NS")]
[XmlType("NS")]
[Serializable]
public class NoneShape : Shape {

    public NoneShape() {
        type = Type.None;
    }

    //Attach the stuffs above.
    override public void AttachMeshAndCollider2D(GameObject go, bool colliderIsTrigger = false) {
        base.AttachCollider2D(go, colliderIsTrigger);
        base.AttachMeshRenderer(go);
    }

    //Attach all stuffs.
    override public void AttachAllComponents(GameObject go, bool colliderIsTrigger = false) {
        base.AttachCollider2D(go, colliderIsTrigger);
        base.AttachMeshRenderer(go);
        base.AttachLineRenderer(go);
    }

}
