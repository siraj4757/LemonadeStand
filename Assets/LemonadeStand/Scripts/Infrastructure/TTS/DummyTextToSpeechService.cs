namespace LemonadeStand.Infrastructure.TTS
{
    public interface ITextToSpeechService
    {
        void Speak(string text);
    }

    public class DummyTextToSpeechService : ITextToSpeechService
    {
        public void Speak(string text)
        {
            //  TODO text to speach 
        }
    }
}
