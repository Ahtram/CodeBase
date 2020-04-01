using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This class will cache any requested Texture.
//You can clear the cache before any scene switching to prevent it from bloating.
static public class TextureCache {

    //Main cache body.
    static private Dictionary<string, Texture2D> m_textureCache = new Dictionary<string, Texture2D>();

    //Try get the sprite from cache. If it's not exist in the cache than load and cache and return it.
    static public Texture2D Get(string path) {

        //Return the cache if it's exist.
        Texture2D returnTexture2D;
        if (m_textureCache.TryGetValue(path, out returnTexture2D)) {
            return returnTexture2D;
        }

        //Cache not exist. Load the sprite and cache it.
        returnTexture2D = Resources.Load<Texture2D>(path);
        if (returnTexture2D != null) {
            m_textureCache.Add(path, returnTexture2D);
        }

        //Return the loaded sprite whatever if it's null. (It will possible be null if this sprite is not exist!)
        return returnTexture2D;
    }

    static public IEnumerator GetAsync(string path, System.Action<Texture2D> onFinish) {
        Texture2D returnTexture2D;
        if (m_textureCache.TryGetValue(path, out returnTexture2D)) {
            if (onFinish != null) {
                onFinish(returnTexture2D);
            }
        } else {
            ResourceRequest resourceRequest = Resources.LoadAsync(path, typeof(Texture2D));
            while (!resourceRequest.isDone) {
                yield return null;
            }
            returnTexture2D = (Texture2D)resourceRequest.asset;

            if (returnTexture2D != null) {
                if (!m_textureCache.ContainsKey(path)) {
                    m_textureCache.Add(path, returnTexture2D);
                }
                if (onFinish != null) {
                    onFinish(returnTexture2D);
                }
            }

            if (onFinish != null) {
                onFinish(returnTexture2D);
            }
        }
    }

    //Clear the cache.
    static public void Clear() {
        m_textureCache.Clear();
    }

}
