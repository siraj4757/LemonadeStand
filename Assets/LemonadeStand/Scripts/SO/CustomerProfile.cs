using UnityEngine;

namespace LemonadeStand.Core.Domain
{
    /// <summary>
    /// Data for a single customer character (fox, rabbit, bear).
    /// 
    /// This is a pure data ScriptableObject so designers can add
    /// new characters without touching code.
    /// </summary>
    [CreateAssetMenu(fileName = "CustomerProfile", menuName = "LemonadeStand/Customer Profile",order = 0)]
    public class CustomerProfile : ScriptableObject
    {
        [Header("Identification")]
        public string id;               // e.g. "fox", "rabbit", "bear"
        public string displayName;      // e.g. "Fiona Fox"

        [Header("Sprites")]
        public Sprite idleSprite;
        public Sprite talkingSprite;
        public Sprite happySprite;
        public Sprite sadSprite;
    }
}
