using LemonadeStand.Core.Game;
using System.Collections.Generic;
using UnityEngine;

namespace LemonadeStand.Core.Domain
{
    public class CustomerRequest
    {
        public LevelType LevelType { get; }
        public string Prompt { get; }

        public string[] SizeOptions { get; }
        public int CorrectSizeIndex { get; }

        public int TargetLemonCount { get; }

        public int[] ContainerVolumes { get; }
        public int CorrectContainerIndex { get; }
        public IReadOnlyList<Sprite> ContainerSprites { get; }

        public CustomerRequest(LevelConfig config)
        {
            LevelType = config.levelType;
            Prompt = config.customerPrompt;

            SizeOptions = config.sizeOptions;

            TargetLemonCount = config.correctCount;

            ContainerVolumes = config.containerVolumes;
            CorrectContainerIndex = config.correctContainerIndex;
            ContainerSprites = config.containerSprites;
        }
    }
}
