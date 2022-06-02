using System;
using UnityEngine;

namespace Runtime.Data.PlayerData.LayoutSpriteData.Impls
{
    [CreateAssetMenu(menuName = "PlayerData/LayoutSpriteData", fileName = "LayoutSpriteData")]
    public class LayoutSpiteData : ScriptableObject, ILayoutSpriteData
    {
        public LayoutSprite[] Get() => _layoutSprite;
        [SerializeField] private LayoutSprite[] _layoutSprite;
        
        public LayoutSprite Get(ELayoutSprite type)
        {
            for (var i = 0; i < _layoutSprite.Length; i++)
            {
                var sprite = _layoutSprite[i];
                if (sprite.Type == type)
                    return sprite;
            }
            throw new Exception("[LayoutData] Can't find skin with key: " + type);
        }
    }
}