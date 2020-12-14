using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Teamuni.Codebase;

//We want ObjectPool can be seperate by scene. So instead of static class we decide to make it a gameObject in scene so it will live/die with the scene.
//Put a ObjectPool gameObject in your scene if you want to use it.
public class GameObjectPool : MonoBehaviour {

    [System.Serializable]
    public class PathCount : System.Object {
        public string path = "";
        public int count = 0;
    }

    //This is the pool body: We'll just use the prefab path as the index of each type of object list.
    public Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

    //Replacement for serialize a dictionary.
    public List<PathCount> loadOnAwake = new List<PathCount>();

    //Use async or instant preload?
    public bool asyncPreload = true;

    //Is this pool done loading after switching scene?
    static public bool isDonePreloading = false;

    //An instance for the static functions.
    static private GameObjectPool m_instance;

    //0.0~1.0
    static public float preloadingProgress = 0.0f;

    void Awake() {
        //Store instance.
        m_instance = this;
        isDonePreloading = false;
        preloadingProgress = 0.0f;

        if (asyncPreload) {
            //Preload all stuffs here.
            StartCoroutine(PreloadAsync());
        } else {
            Preload();
        }
    }

    private IEnumerator PreloadAsync() {
        int totalCount = 1;
        for (int i = 0; i < loadOnAwake.Count; ++i) {
            totalCount += loadOnAwake[i].count;
        }

        int currentProcessCount = 0;
        for (int i = 0; i < loadOnAwake.Count; ++i) {
            bool isDone = false;
            StartCoroutine(AddCountAsync(loadOnAwake[i].path, loadOnAwake[i].count, () => isDone = true, () => { currentProcessCount++; preloadingProgress = (float)currentProcessCount / (float)totalCount; }));
            while (!isDone) {
                yield return null;
            }
        }
        preloadingProgress = 1.0f;
        yield return null;
        isDonePreloading = true;
    }

    private void Preload() {
        int totalCount = 1;
        for (int i = 0; i < loadOnAwake.Count; ++i) {
            totalCount += loadOnAwake[i].count;
        }

        for (int i = 0; i < loadOnAwake.Count; ++i) {
            AddCount(loadOnAwake[i].path, loadOnAwake[i].count);
        }

        preloadingProgress = 1.0f;
        isDonePreloading = true;
    }

    /// <summary>
    /// Get the pool content debug info string.
    /// </summary>
    /// <returns></returns>
    public string PoolInfoStr() {
        string infoStr = "";
        foreach (KeyValuePair<string, Queue<GameObject>> item in pool) {
            infoStr += item.Key;
            infoStr += " | count[";
            infoStr += item.Value.Count;
            infoStr += "]" + System.Environment.NewLine;
        }
        return infoStr;
    }

    static public IEnumerator AddCountAsync(string resPath, int count, System.Action onFinish, System.Action onProcess) {
        //Instantiate a prefab.
        ResourceRequest resourceRequest = Resources.LoadAsync(resPath, typeof(Object));
        while (!resourceRequest.isDone) {
            yield return null;
        }
        Object prefab = resourceRequest.asset;

        //Check if this prefab exist.
        if (prefab == null) {
            Debug.LogError("Error! Cannot load prefab: " + resPath);
        } else {
            const int INSTANTIATE_COUNT_PER_YIELD = 20;
            int accuCount = 0;
            for (int i = 0; i < count; ++i) {
                //Instantiate it.
                GameObject newGO = GameObject.Instantiate(prefab) as GameObject;
                if (newGO != null) {
                    //Check if the path exist in pool.
                    if (!m_instance.pool.ContainsKey(resPath)) {
                        //Add a key and GO List to pool.
                        m_instance.pool.Add(resPath, new Queue<GameObject>());
                    }

                    //Add the new GO to the corrent list.
                    m_instance.pool[resPath].Enqueue(newGO);

                    //Reset transform.
                    if (newGO.transform != null) {
                        newGO.transform.SetParent(m_instance.transform);
                        newGO.transform.localPosition = Vector3.zero;
                        newGO.transform.localRotation = Quaternion.identity;
                        newGO.transform.localScale = Vector3.one;
                    }

                    //Stop auto return counting.
                    AutoReturn autoReturn = newGO.GetComponent<AutoReturn>();
                    if (autoReturn != null) {
                        autoReturn.StopCount();
                    }
                    SelfDestroy selfDestroy = newGO.GetComponent<SelfDestroy>();
                    if (selfDestroy != null) {
                        selfDestroy.StopCount();
                    }

                    newGO.SetActive(false);
                } else {
                    Debug.LogWarning("Oops! Cannot instantiate [" + resPath + "]");
                }

                onProcess?.Invoke();

                accuCount++;
                if (accuCount >= INSTANTIATE_COUNT_PER_YIELD) {
                    accuCount = 0;
                    yield return null;
                }
            }
        }

        onFinish?.Invoke();
    }

    /// <summary>
    /// Add an object to the pool. 
    /// </summary>
    static public void AddCount(string resPath, int count) {
        //Instantiate a prefab.
        Object prefab = Resources.Load(resPath);

        //Check if this prefab exist.
        if (prefab == null) {
            Debug.LogError("Error! Cannot load prefab: " + resPath);
        } else {
            for (int i = 0; i < count; ++i) {
                //Instantiate it.
                GameObject newGO = GameObject.Instantiate(prefab) as GameObject;
                if (newGO != null) {
                    //Check if the path exist in pool.
                    if (!m_instance.pool.ContainsKey(resPath)) {
                        //Add a key and GO List to pool.
                        m_instance.pool.Add(resPath, new Queue<GameObject>());
                    }

                    //Add the new GO to the corrent list.
                    m_instance.pool[resPath].Enqueue(newGO);

                    //Reset transform.
                    if (newGO.transform != null) {
                        newGO.transform.SetParent(m_instance.transform);
                        newGO.transform.localPosition = Vector3.zero;
                        newGO.transform.localRotation = Quaternion.identity;
                        newGO.transform.localScale = Vector3.one;
                    }

                    //Stop auto return counting.
                    AutoReturn autoReturn = newGO.GetComponent<AutoReturn>();
                    if (autoReturn != null) {
                        autoReturn.StopCount();
                    }
                    SelfDestroy selfDestroy = newGO.GetComponent<SelfDestroy>();
                    if (selfDestroy != null) {
                        selfDestroy.StopCount();
                    }

                    newGO.SetActive(false);
                } else {
                    Debug.LogWarning("Oops! Cannot instantiate [" + resPath + "]");
                }
            }
        }
    }

    /// <summary>
    /// Get the current pooling count for a resource.
    /// </summary>
    static public int GetCount(string resPath) {
        if (m_instance != null) {
            if (m_instance.pool.ContainsKey(resPath)) {
                return m_instance.pool[resPath].Count;
            }
        }
        return 0;
    }

    /// <summary>
    /// Clear (Destroy everything!) in the current pool.
    /// </summary>
    static public void Clear() {
        if (m_instance != null) {
            foreach (KeyValuePair<string, Queue<GameObject>> kvp in m_instance.pool) {
                foreach (GameObject go in kvp.Value) {
                    GameObject.Destroy(go);
                }
            }
            m_instance.pool.Clear();
        }
    }

    /// <summary>
    /// Check if a resource path is valid. (Is this prefab exist?)
    /// </summary>
    /// <param name="resPath"></param>
    /// <returns></returns>
    static public bool CanLend(string resPath) {
        return (Resources.Load(resPath) != null) ? (true) : (false);
    }

    /// <summary>
    /// Lend out a GameObject.
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="active"></param>
    /// <returns></returns>
    static public GameObject Lend(string resPath, bool active) {
        if (m_instance != null) {
            //Check if there's a index in the pool.
            Queue<GameObject> existGOQueue;
            if (m_instance.pool.TryGetValue(resPath, out existGOQueue)) {
                if (existGOQueue.Count > 0) {
                    //Fetch one from the pool.
                    GameObject lendGO = existGOQueue.Dequeue();

                    if (lendGO == null) {
                        Debug.LogError("Oops! The lend gameobject is null: [" + resPath + "]");
                    }

                    //Start auto return counting.
                    AutoReturn autoReturn = lendGO.GetComponent<AutoReturn>();
                    if (autoReturn != null) {
                        autoReturn.StartCount();
                        //Add a tag for identify it when returned.
                        lendGO.AddComponent<PoolTag>().path = resPath;
                    } else {
                        //Check if this is a selfdestory object.
                        SelfDestroy selfDestroy = lendGO.GetComponent<SelfDestroy>();
                        if (selfDestroy != null) {
                            //This is a selfdestory object.
                            //In this case we will not give it a pool tag.
                        } else {
                            //Add a tag for identify it when returned.
                            lendGO.AddComponent<PoolTag>().path = resPath;
                        }
                    }

                    if (active) {
                        lendGO.SetActive(true);
                    }

                    return lendGO;
                }
            }
        } else {
            Debug.Log("No ObjectPool exist in current scene. Use the regular instantiate.");
        }

        //No this kind of object in pool. Instantiate new object.
        Object prefab = Resources.Load(resPath);
        if (prefab == null) {
            //We want this failed silently.
            return null;
        } else {
            GameObject newGO = GameObject.Instantiate(prefab) as GameObject;

            //Start auto return counting.
            AutoReturn autoReturn = newGO.GetComponent<AutoReturn>();
            if (autoReturn != null) {
                autoReturn.StartCount();
                //Add a tag for identify it when returned.
                newGO.AddComponent<PoolTag>().path = resPath;
            } else {
                //Check if this is a selfdestory object.
                SelfDestroy selfDestroy = newGO.GetComponent<SelfDestroy>();
                if (selfDestroy != null) {
                    //This is a selfdestory object.
                    //In this case it we will not give it a pool tag.
                } else {
                    //Add a tag for identify it when returned.
                    newGO.AddComponent<PoolTag>().path = resPath;
                }
            }

            if (active) {
                newGO.SetActive(true);
            }

            return newGO;
        }
    }

    /// <summary>
    /// Lend out a GameObject.
    /// </summary>
    /// <param name="resPath"></param>
    /// <param name="parentGO"></param>
    /// <param name="setParentLayer"></param>
    /// <returns></returns>
    static public GameObject Lend(string resPath, GameObject parentGO, bool setParentLayer = true, bool active = true) {
        GameObject lendGO = Lend(resPath, active);
        if (lendGO != null) {
            lendGO.transform.SetParent(parentGO.transform);
            lendGO.transform.localPosition = Vector3.zero;
            lendGO.transform.localRotation = Quaternion.identity;

            if (setParentLayer) {
                lendGO.SetLayer(parentGO.layer);
            }
        }
        return lendGO;
    }

    /// <summary>
    /// Return a GameObject and store it into the pool.
    /// </summary>
    /// <param name="go"></param>
    static public void Return(GameObject go) {
        Return(go, true);
    }

    /// <summary>
    /// Return a GameObject and store it into the pool.
    /// </summary>
    /// <param name="go"></param>
    /// <param name="deActive"></param>
    static public void Return(GameObject go, bool deActive) {
        if (go != null) {
            if (m_instance != null) {
                //Prevent a situation that a selfDestroy object suicide in the pool that will cause exception!
                SelfDestroy selfDestroy = go.GetComponent<SelfDestroy>();
                if (selfDestroy != null) {
                    //Kill it directly now.
                    GameObject.Destroy(go);
                    return;
                }

                PoolTag poolTag = go.GetComponent<PoolTag>();
                //Try get pool tag.
                if (poolTag != null) {
                    //Get resPath.
                    string resPath = poolTag.path;

                    //Check if the path exist in pool.
                    if (!m_instance.pool.ContainsKey(resPath)) {
                        //Add a key and GO List to pool.
                        m_instance.pool.Add(resPath, new Queue<GameObject>());
                    }

                    //Check if this guy is already in the pool to prevent tragedy.
                    if (!m_instance.pool[resPath].Contains(go)) {
                        //Remove the tag.
                        Object.Destroy(poolTag);

                        m_instance.pool[resPath].Enqueue(go);

                        //Reset transform.
                        go.transform.SetParent(m_instance.transform);
                        go.transform.localPosition = Vector3.zero;
                        go.transform.localRotation = Quaternion.identity;
                        go.transform.localScale = Vector3.one;

                        if (deActive) {
                            go.SetActive(false);
                        }
                    } else {
                        Debug.LogError("The returned object is already in the pool! Cannot return it again! " + go.name);
                    }
                } else {
                    Debug.LogWarning("The returned object has no PoolTag: " + go.name);
                    GameObject.Destroy(go);
                }
            } else {
                Debug.Log("No ObjectPool exist in current scene. Use the regular Destroy.");
                GameObject.Destroy(go);
            }
        } else {
            Debug.LogError("The returned object is null!");
        }
    }

    /// <summary>
    /// Try to return (or destroy) all GameObject under a parant GameObject.
    /// </summary>
    /// <param name="parantGO"></param>
    static public void ReturnUnder(GameObject parantGO) {
        for (int i = parantGO.transform.childCount - 1; i >= 0; --i) {
            GameObject go = parantGO.transform.GetChild(i).gameObject;
            AutoReturn autoReturn = go.GetComponent<AutoReturn>();
            if (autoReturn != null) {
                autoReturn.ReturnNow();
            } else {
                Return(go);
            }
        }
    }

}
