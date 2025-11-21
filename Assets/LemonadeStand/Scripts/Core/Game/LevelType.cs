namespace LemonadeStand.Core.Game
{
    public enum GameState
    {
        Splash,
        Loading,
        NameEntry,
        Practice,
        Test,
        PracticeComplete,
        Score
    }

    public enum LevelType
    {
        SizeSelection,      // big / small / massive  (Level 1)
        CountLemons,        // how many lemons       (Level 2)
        CompareContainers   // which holds more      (Level 3)
    }
}
