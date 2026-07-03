using NAudio.Wave;
using CaveGame.Menu;
using System;
using System.Diagnostics;
using System.IO;

namespace CaveGame.Core
{
    class AudioManager
    {
        private Random rnd = new Random();
        private WaveOutEvent? bgMusic;
        private bool audioAvailable;
        private string? linuxAudioPlayer;

        private bool volumeEnabled => !Settings.selectedVolumeMode;

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

        public AudioManager()
        {
            if (OperatingSystem.IsWindows())
            {
                bgMusic = new WaveOutEvent();
                audioAvailable = true;
            }
            else if (OperatingSystem.IsMacOS())
            {
                audioAvailable = true;
            }
            else if (OperatingSystem.IsLinux())
            {
                linuxAudioPlayer = FindLinuxPlayer();
                audioAvailable = linuxAudioPlayer != null;
            }
            else
            {
                audioAvailable = false;
            }
        }

        private static string? FindLinuxPlayer()
        {
            foreach (var player in new[] { "mpg123", "mpv", "ffplay", "cvlc" })
            {
                if (CommandExists(player))
                {
                    return player;
                }
            }

            return null;
        }

        private static bool CommandExists(string command)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "which",
                    Arguments = command,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                process?.WaitForExit();
                return process?.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        private void PlayViaAfplay(string path, float volume)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "afplay",
                Arguments = $"-v {volume.ToString(System.Globalization.CultureInfo.InvariantCulture)} \"{path}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process.Start(psi);
        }

        private void PlayViaLinux(string path)
        {
            if (linuxAudioPlayer == null)
            {
                return;
            }

            ProcessStartInfo psi;
            switch (linuxAudioPlayer)
            {
                case "mpg123":
                    psi = new ProcessStartInfo
                    {
                        FileName = "mpg123",
                        Arguments = $"-q \"{path}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    break;
                case "mpv":
                    psi = new ProcessStartInfo
                    {
                        FileName = "mpv",
                        Arguments = $"--no-video --really-quiet \"{path}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    break;
                case "ffplay":
                    psi = new ProcessStartInfo
                    {
                        FileName = "ffplay",
                        Arguments = $"-nodisp -autoexit -loglevel quiet \"{path}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    break;
                case "cvlc":
                    psi = new ProcessStartInfo
                    {
                        FileName = "cvlc",
                        Arguments = $"--play-and-exit --quiet \"{path}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    break;
                default:
                    return;
            }

            Process.Start(psi);
        }

        public void PlayRandomSteps(float volume)
        {
            string randomStepPath = stepSoundList[rnd.Next(stepSoundList.Length)];
            PlaySound(randomStepPath, volume);
        }

        public void PlayBackground(string path)
        {
            if (!audioAvailable || !volumeEnabled)
            {
                return;
            }

            if (OperatingSystem.IsWindows())
            {
                if (bgMusic == null)
                {
                    bgMusic = new WaveOutEvent();
                }

                var audioFile = new AudioFileReader(path);
                bgMusic.Init(audioFile);
                bgMusic.Play();
            }
            else if (OperatingSystem.IsMacOS())
            {
                PlayViaAfplay(path, 1.0f);
            }
            else if (OperatingSystem.IsLinux())
            {
                PlayViaLinux(path);
            }
        }

        public void PlaySound(string path, float volume)
        {
            if (!audioAvailable || !volumeEnabled)
            {
                return;
            }

            if (OperatingSystem.IsWindows())
            {
                var audioFile = new AudioFileReader(path);
                audioFile.Volume = volume;
                var output = new WaveOutEvent();
                output.Init(audioFile);
                output.Play();
            }
            else if (OperatingSystem.IsMacOS())
            {
                PlayViaAfplay(path, volume);
            }
            else if (OperatingSystem.IsLinux())
            {
                PlayViaLinux(path);
            }
        }
    }
}