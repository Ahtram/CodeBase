using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This class will cache any requested Sprite.
//You can clear the cache before any scene switching to prevent it from bloating.
static public class SpriteCache {

    //Main cache body.
    static private Dictionary<string, Sprite> m_spriteCache = new Dictionary<string, Sprite>();

    //Multi-sprite cache body.
    static private Dictionary<string, Sprite[]> m_multiSpriteCache = new Dictionary<string, Sprite[]>();

    //Try get the sprite from cache. If it's not exist in the cache than load and cache and return it.
    static public Sprite Get(string path) {

        //Return the cache if it's exist.
        Sprite returnSprite;
        if(m_spriteCache.TryGetValue(path, out returnSprite)) {
            return returnSprite;
        }

        //Cache not exist. Load the sprite and cache it.
        returnSprite = Resources.Load<Sprite>(path);
        if(returnSprite != null) {
            m_spriteCache.Add(path, returnSprite);
        } else {
            //[Temp comment]: some icon file is missing currently.
            //Debug.LogWarning("Oops! Resource not found: " + path);
        }

        //Return the loaded sprite whatever if it's null. (It will possible be null if this sprite is not exist!)
        return returnSprite;
    }

    //Try get multi-sprites in a single texture resource in one time.
    static public Sprite[] GetMulti(string path) {
        Sprite[] returnSprites = null;
        if (m_multiSpriteCache.TryGetValue(path, out returnSprites)) {
            return returnSprites;
        }

        //Cache not exist. Load the sprite and cache it.
        returnSprites = Resources.LoadAll<Sprite>(path);
        if (returnSprites != null) {
            m_multiSpriteCache.Add(path, returnSprites);
        } else {
            //[Temp comment]: some icon file is missing currently.
            //Debug.LogWarning("Oops! Resource not found: " + path);
        }

        return returnSprites;
    }

    static public IEnumerator GetAsync(string path, System.Action<Sprite> onFinish) {
        Sprite returnSprite;
        if (m_spriteCache.TryGetValue(path, out returnSprite)) {
            if (onFinish != null) {
                onFinish(returnSprite);
            }
        } else {
            ResourceRequest resourceRequest = Resources.LoadAsync(path, typeof(Sprite));
            while (!resourceRequest.isDone) {
                yield return null;
            }
            returnSprite = (Sprite)resourceRequest.asset;
            if (returnSprite != null) {
                m_spriteCache.Add(path, returnSprite);
            }
            if (onFinish != null) {
                onFinish(returnSprite);
            }
        }
    }

    //Clear the cache.
    static public void Clear() {
        m_spriteCache.Clear();
    }

}
