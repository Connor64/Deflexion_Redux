using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Deflexion_Redux {
    public class AudioManager {
        private Dictionary<SoundType, SoundEffectInstance> sounds = new Dictionary<SoundType, SoundEffectInstance>();
        private List<SoundEffectInstance[]> songs = new List<SoundEffectInstance[]>();

        public bool mute = false;
        private float masterVolume = 0;

        private static AudioManager instance;
        public static AudioManager Instance {
            get {
                if (instance == null) {
                    instance = new AudioManager();
                }
                return instance;
            }
        }

        public void UpdateSongs() {
            foreach (SoundEffectInstance[] songPair in songs) {
                if (songPair[0].State == SoundState.Stopped && songPair[1].State == SoundState.Stopped) {
                    Debug.Print("Starting Body...");
                    songPair[1].Play();
                }
            }
            if (mute) {
                SoundEffect.MasterVolume = 0;
            } else {
                SoundEffect.MasterVolume = masterVolume;
            }
        }

        public void setVolume(float volume) {
            SoundEffect.MasterVolume = volume;
            masterVolume = volume;
        }

        public void playSound(SoundType sound, bool loop, float volume) {
            SoundEffectInstance se = AssetManager.sounds[sound].CreateInstance();
            se.IsLooped = loop;
            se.Volume = volume;
            se.Play();
            sounds.Add(sound, se);
        }

        public void playSong(SoundType intro_sound, SoundType body_sound, bool loop, float volume) {
            SoundEffectInstance[] songPair = new SoundEffectInstance[] {
                AssetManager.sounds[intro_sound].CreateInstance(),
                AssetManager.sounds[body_sound].CreateInstance()
            };

            songPair[0].Volume = volume;
            songPair[0].IsLooped = false;
            songPair[1].Volume = volume;
            songPair[1].IsLooped = loop;

            songPair[0].Play();

            songs.Add(songPair);
        }

        public void pauseSound(SoundType sound) {
            if (sounds[sound].State == SoundState.Paused) {
                sounds[sound].Resume();
            } else {
                sounds[sound].Pause();
            }
        }

        public void pauseAllSounds() {
            foreach(KeyValuePair<SoundType, SoundEffectInstance> sound in sounds) {
                if (sound.Value.State == SoundState.Paused) {
                    sound.Value.Resume();
                } else {
                    sound.Value.Pause();
                }
            }
            foreach(SoundEffectInstance[] songPair in songs) {
                if (songPair[0].State == SoundState.Paused) {
                    songPair[0].Resume();
                    songPair[1].Resume();
                } else {
                    songPair[0].Pause();
                    songPair[1].Pause();
                }
            }
        }

        public void stopSound(SoundType sound) {
            sounds[sound].Stop();
            sounds[sound].Dispose();
            sounds.Remove(sound);
        }

        public void stopAllSounds() {
            foreach(KeyValuePair<SoundType, SoundEffectInstance> sound in sounds) {
                sound.Value.Stop();
                sound.Value.Dispose();
            }
            sounds.Clear();

            foreach (SoundEffectInstance[] songPair in songs) {
                songPair[0].Stop();
                songPair[0].Dispose();

                songPair[1].Stop();
                songPair[1].Dispose();
            }

            songs.Clear();
        }
    }
}