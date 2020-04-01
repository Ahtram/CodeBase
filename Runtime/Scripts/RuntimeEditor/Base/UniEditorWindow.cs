#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

/// <summary>
/// This is our main custom editor base class. Share between all our home made editors.
/// </summary>
abstract public class UniEditorWindow : EditorWindow {

    protected bool ctrlDown = false;
    protected bool shiftDown = false;
    protected bool inspectorUpdateRepaint = false;
    protected bool updateRepaint = false;

    private bool releaseFocusControl = false;

    //For zoom-in the editor window.
    //private Rect originalPosition = Rect.zero;
    //private bool zoomedIn = false;

    virtual public void OnGUI() {
        if (releaseFocusControl) {
            releaseFocusControl = false;
            GUI.FocusControl(null);
        }

        //Hot key!
        switch (Event.current.type) {
            case EventType.KeyUp:
                if (Event.current.keyCode == KeyCode.LeftControl || Event.current.keyCode == KeyCode.RightControl) {
                    ctrlDown = false;
                }
                if (Event.current.keyCode == KeyCode.LeftShift || Event.current.keyCode == KeyCode.RightShift) {
                    shiftDown = false;
                }
                break;
            case EventType.KeyDown:
                if (Event.current.keyCode == KeyCode.LeftControl || Event.current.keyCode == KeyCode.RightControl) {
                    ctrlDown = true;
                }
                if (Event.current.keyCode == KeyCode.LeftShift || Event.current.keyCode == KeyCode.RightShift) {
                    shiftDown = true;
                }
                if (Event.current.keyCode == KeyCode.W) {
                    if (ctrlDown) {
                        Close();
                    }
                }
                if (Event.current.keyCode == KeyCode.D) {
                    if (ctrlDown) {
                        OnCtrlD();
                    }
                }
                ////Maximize window shortcut.
                //if (Event.current.keyCode == KeyCode.M) {
                //    if (ctrlDown) {
                //        if (zoomedIn) {
                //            zoomedIn = false;

                //            position = originalPosition;

                //            Repaint();

                //            //Debug.Log("Zoom out.");
                //        } else {
                //            zoomedIn = true;
                //            originalPosition = position;

                //            //This is a hack! Magic involved.
                //            //http://answers.unity3d.com/questions/960413/editor-window-how-to-center-a-window.html
                //            Rect UnityMainEditorPos = Util.GetEditorMainWindowPos();

                //            position = UnityMainEditorPos;

                //            Repaint();

                //            //Debug.Log("Zoom in.");
                //        }
                //    }
                //}
                break;
        }
    }

    virtual protected void Update() {
        if (updateRepaint) {
            Repaint();
        }
    }

    virtual protected void OnInspectorUpdate() {
        if (inspectorUpdateRepaint) {
            Repaint();
        }
    }

    virtual public void OnFocus() {
        ReleaseFocusControl();
    }

    /// <summary>
    /// General convenient hotkey. Override this to make your own good.
    /// </summary>
    virtual protected void OnCtrlD() {

    }

    /// <summary>
    /// A hack for manually lose its focus on certain control when the window is in background.
    /// </summary>
    public void ReleaseFocusControl() {
        releaseFocusControl = true;
    }

    /// <summary>
    /// A generic way to open any class as an editorWindow. (Hack involved)
    /// </summary>
    /// <typeparam name="T">T should be a derived class from UniEditorWindow class.</typeparam>
    static public UniEditorWindow ShowWindow<T>(bool focus) where T : UniEditorWindow {
        if (typeof(UniEditorWindow).IsAssignableFrom(typeof(T))) {
            //Use Utility windows for OSX: That's just what I prefer.
            bool isUtility = false;

#if UNITY_EDITOR_WIN
            isUtility = false;
#elif UNITY_EDITOR_OSX
            isUtility = true;
#endif
            UniEditorWindow window = (UniEditorWindow)EditorWindow.GetWindow(typeof(T), isUtility, typeof(T).ToString(), focus);
            return window;
        } else {
            Debug.LogError("You are attempting to open a non-editor class! Do you know what you are doing?");
            return null;
        }
    }

    /// <summary>
    /// A generic way to open any class as an editorWindow. (Hack involved)
    /// </summary>
    /// <typeparam name="T">T should be a derived class from UniEditorWindow class.</typeparam>
    static public UniEditorWindow ShowWindow<T>() where T : UniEditorWindow {
        if (typeof(UniEditorWindow).IsAssignableFrom(typeof(T))) {
            //Use Utility windows for OSX: That's just what I prefer.
            bool isUtility = false;

#if UNITY_EDITOR_WIN
            isUtility = false;
#elif UNITY_EDITOR_OSX
            isUtility = true;
#endif
            UniEditorWindow window = (UniEditorWindow)EditorWindow.GetWindow(typeof(T), isUtility, typeof(T).ToString(), true);
            return window;
        } else {
            Debug.LogError("You are attempting to open a non-editor class! Do you know what you are doing?");
            return null;
        }
    }

    /// <summary>
    /// A generic way to create any class as an editorWindow. (Hack involved)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    static public UniEditorWindow CreateWindow<T>() where T : UniEditorWindow {
        if (typeof(UniEditorWindow).IsAssignableFrom(typeof(T))) {
            //Use Utility windows for OSX: That's just what I prefer.
            bool isUtility = false;

#if UNITY_EDITOR_WIN
            isUtility = false;
#elif UNITY_EDITOR_OSX
            isUtility = true;
#endif

            UniEditorWindow instance = EditorWindow.CreateInstance<T>();
            if (isUtility) {
                instance.ShowUtility();
            } else {
                instance.Show();
            }
            return instance;
        } else {
            Debug.LogError("You are attempting to open a non-editor class! Do you know what you are doing?");
            return null;
        }
    }

    /// <summary>
    /// A generic way to create any class as an editorWindow. (Hack involved)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    static public UniEditorWindow CreatePupup<T>() where T : UniEditorWindow {
        if (typeof(UniEditorWindow).IsAssignableFrom(typeof(T))) {
            UniEditorWindow instance = EditorWindow.CreateInstance<T>();
            instance.ShowPopup();
            return instance;
        } else {
            Debug.LogError("You are attempting to open a non-editor class! Do you know what you are doing?");
            return null;
        }
    }

    /// <summary>
    /// A generic way to create any class as an editorWindow. (Hack involved)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    static public UniEditorWindow CreateAuxWindow<T>() where T : UniEditorWindow {
        if (typeof(UniEditorWindow).IsAssignableFrom(typeof(T))) {
            UniEditorWindow instance = EditorWindow.CreateInstance<T>();
            instance.ShowAuxWindow();
            return instance;
        } else {
            Debug.LogError("You are attempting to open a non-editor class! Do you know what you are doing?");
            return null;
        }
    }

    /// <summary>
    /// Close this window.
    /// </summary>
    public void CloseWindow() {
        Close();
    }

    /// <summary>
    /// ShowNotification().
    /// </summary>
    /// <param name="content"></param>
    public void ShowNotify(GUIContent content) {
        ShowNotification(content);
    }

    /// <summary>
    /// ShowNotification().
    /// </summary>
    /// <param name="content"></param>
    public void ShowNotify(string content) {
        ShowNotification(new GUIContent(content));
    }

}

#endif