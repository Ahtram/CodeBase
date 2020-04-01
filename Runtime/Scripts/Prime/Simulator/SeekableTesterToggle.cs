using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//A simple toggle that only for test perpose.
public class SeekableTesterToggle : UIBase {

    public Image image;

    private bool m_isToggled = false;

    public void SetToggle(bool b) {
        m_isToggled = b;
        if (b) {
            image.color = ColorPlus.Azure;
        } else {
            image.color = ColorPlus.White;
		}
	}

	public bool IsToggled() {
        return m_isToggled;
    }

}
