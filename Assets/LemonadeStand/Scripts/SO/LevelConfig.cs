using UnityEngine;

namespace LemonadeStand.Core.Game
{
    [CreateAssetMenu(fileName = "LevelConfig",menuName = "LemonadeStand/Level Config",order = 0)]

    public class LevelConfig : ScriptableObject
    {
        public string levelId;
        public LevelType levelType;

        [TextArea] public string customerPrompt; // e.g. "I'd like a BIG lemonade, please!"

        public string[] sizeOptions;   // used for SizeSelection
        public int correctCount;       // used for CountLemons
        public int[] containerVolumes; // used for CompareContainers (abstract units)
        public int correctContainerIndex;
        public bool isPractice;

        public Sprite[] containerSprites;
    }
}
