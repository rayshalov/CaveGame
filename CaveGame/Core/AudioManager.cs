using NAudio.Wave;

namespace CaveGame.Core
{
    class AudioManager
    {

        private Random rnd = new Random();
        private WaveOutEvent bgMusic = new WaveOutEvent();

        private string[] stepSoundList = new string[]
        {
            "sounds/stepsgame1.mp3",
            "sounds/stepsgame2.mp3",
            "sounds/stepsgame3.mp3",
            "sounds/stepsgame4.mp3",
            "sounds/stepsgame5.mp3"
        };

        private string[] MonsterStepSoundList = new string[]
        {
            "sounds/stepsgame1.mp3",
            "sounds/stepsgame2.mp3",
            "sounds/stepsgame3.mp3",
            "sounds/stepsgame4.mp3",
            "sounds/stepsgame5.mp3"
        };

        public void PlayRandomSteps(float volume)
        {
            string randomStepPath = stepSoundList[rnd.Next(stepSoundList.Length)];
            PlaySound(randomStepPath, volume);
        }


        public void PlayBackground(string path)
        {
            var audioFile = new AudioFileReader(path);
            bgMusic.Init(audioFile);
            bgMusic.Play();
        }

        public void PlaySound(string path, float volume)
        {
            var audioFile = new AudioFileReader(path);
            audioFile.Volume = volume;
            var output = new WaveOutEvent();
            output.Init(audioFile);
            output.Play();
        }
    }
}
