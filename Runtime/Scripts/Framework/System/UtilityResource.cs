using UnityEngine;

/// <summary>
/// Provide some convenient default resource for basic need.
/// </summary>
static public class UtilityResource {
	
	/// <summary>
	/// Returns a transparant image Texture2D.
	/// </summary>
	/// <returns></returns>
	static public Texture2D EmptyTexture() {
        return TextureCache.Get(SysPath.CodeBaseImagePath + "Empty");
    }

	/// <summary>
	/// Returns a transparant image sprite.
	/// </summary>
	/// <returns></returns>
	static public Sprite EmptySprite() {
        return SpriteCache.Get(SysPath.CodeBaseImagePath + "Empty");
    }

    /// <summary>
    /// Returns a white image Texture2D.
    /// </summary>
    /// <returns></returns>
    static public Texture2D WhiteTexture() {
        return TextureCache.Get(SysPath.CodeBaseImagePath + "White");
    }

    /// <summary>
    /// Returns a white image sprite.
    /// </summary>
    /// <returns></returns>
    static public Sprite WhiteSprite() {
        return SpriteCache.Get(SysPath.CodeBaseImagePath + "White");
    }

    /// <summary>
    /// Returns a black image Texture2D.
    /// </summary>
    /// <returns></returns>
    static public Texture2D BlackTexture() {
        return TextureCache.Get(SysPath.CodeBaseImagePath + "Black");
    }

    /// <summary>
    /// Returns a black image sprite.
    /// </summary>
    /// <returns></returns>
    static public Sprite BlackSprite() {
        return SpriteCache.Get(SysPath.CodeBaseImagePath + "Black");
    }

    /// <summary>
    /// Returns a mosaic image Texture2D.
    /// </summary>
    /// <returns></returns>
    static public Texture2D MosaicTexture() {
        return TextureCache.Get(SysPath.CodeBaseImagePath + "Mosaic");
    }

    /// <summary>
    /// Returns a mosaic image sprite.
    /// </summary>
    /// <returns></returns>
    static public Sprite MosaicSprite() {
        return SpriteCache.Get(SysPath.CodeBaseImagePath + "Mosaic");
    }

    /// <summary>
    /// Returns a neo's image Texture2D.
    /// </summary>
    /// <returns></returns>
    static public Texture2D NeoTexture() {
        return TextureCache.Get(SysPath.CodeBaseImagePath + "Neo128");
    }

    /// <summary>
    /// Returns a neo's image sprite.
    /// </summary>
    /// <returns></returns>
    static public Sprite NeoSprite() {
        return SpriteCache.Get(SysPath.CodeBaseImagePath + "Neo128");
    }

}
