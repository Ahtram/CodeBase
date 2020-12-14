using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Declare the most used UnityEvent type here for common usage.
/// This may looks stupid but it'll work well.
/// </summary>
namespace Teamuni.Codebase {

	[System.Serializable]
    public class UnityEventBool : UnityEvent<bool> { }

	[System.Serializable]
    public class UnityEventInt : UnityEvent<int> { }

	[System.Serializable]
    public class UnityEventString : UnityEvent<string> { }

	[System.Serializable]
    public class UnityEventFloat : UnityEvent<float> { }
	
	//--------------------------------------------------

	[System.Serializable]
    public class UnityEventBoolBool : UnityEvent<bool, bool> { }

    [System.Serializable]
    public class UnityEventBoolInt : UnityEvent<bool, int> { }

    [System.Serializable]
    public class UnityEventBoolString : UnityEvent<bool, string> { }

    [System.Serializable]
    public class UnityEventBoolFloat : UnityEvent<bool, float> { }

    //--------------------------------------------------

	[System.Serializable]
    public class UnityEventIntBool : UnityEvent<int, bool> { }

    [System.Serializable]
    public class UnityEventIntInt : UnityEvent<int, int> { }

    [System.Serializable]
    public class UnityEventIntString : UnityEvent<int, string> { }

    [System.Serializable]
    public class UnityEventIntFloat : UnityEvent<int, float> { }

    //--------------------------------------------------

	[System.Serializable]
    public class UnityEventStringBool : UnityEvent<string, bool> { }

    [System.Serializable]
    public class UnityEventStringInt : UnityEvent<string, int> { }

    [System.Serializable]
    public class UnityEventStringString : UnityEvent<string, string> { }

    [System.Serializable]
    public class UnityEventStringFloat : UnityEvent<string, float> { }

    //--------------------------------------------------

	[System.Serializable]
    public class UnityEventFloatBool : UnityEvent<float, bool> { }

    [System.Serializable]
    public class UnityEventFloatInt : UnityEvent<float, int> { }

    [System.Serializable]
    public class UnityEventFloatString : UnityEvent<float, string> { }

    [System.Serializable]
    public class UnityEventFloatFloat : UnityEvent<float, float> { }

    //--------------------------------------------------

    [System.Serializable]
    public class UnityEventRectTransform : UnityEvent<RectTransform> { }
}
