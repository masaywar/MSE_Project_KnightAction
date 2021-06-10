using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : Singleton<SpriteManager>
{
    private Dictionary<string, Sprite> _spriteDict = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> spriteDict
    {
        get => _spriteDict;
    }

    private Dictionary<string, RuntimeAnimatorController> _animationDict = new Dictionary<string, RuntimeAnimatorController>();
    public Dictionary<string, RuntimeAnimatorController> animationDict
    {
        get => _animationDict;
    }

    public string spritePath = "Sprites";
    public string animPath = "Animations"; 

    public void Initialize()
    {
        var addedSprite = Resources.LoadAll<Sprite>(spritePath);
        var addedAnim = Resources.LoadAll<RuntimeAnimatorController>(animPath);

        addedSprite.ForEach(sprite=> {
            print(sprite.name);
            spriteDict.Add(sprite.name, sprite); });
        addedAnim.ForEach(anim => animationDict.Add(anim.name, anim));

        DontDestroyOnLoad(this.gameObject);
    }

    public Sprite GetSprite(string name)
    {
        if (spriteDict.TryGetValue(name, out var sprite))
        {
            return sprite;        
        }

        return null;
    }

    public RuntimeAnimatorController GetAnim(string name)
    {
        if (animationDict.TryGetValue(name, out var anim))
        {
            return anim;
        }

        return null;
    }
}
