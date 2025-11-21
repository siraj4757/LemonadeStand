namespace LemonadeStand.Core.Domain
{
    public class PlayerProfile
    {
        public string StudentName { get; private set; }
        public int TotalScore { get; private set; }

        public PlayerProfile(string studentName)
        {
            StudentName = studentName;
        }

        public void AddScore(int amount)
        {
            TotalScore += amount;
        }
    }
}
