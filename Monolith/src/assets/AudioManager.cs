using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Monolith.assets;

public static class AudioManager
{
	private static Dictionary<string, SoundEffectInstance> sounds, music;
	private static float soundVolume, musicVolume;

	internal static void Initialize()
	{
		sounds = new Dictionary<string, SoundEffectInstance>();
		music = new Dictionary<string, SoundEffectInstance>();

		SoundEffect.MasterVolume = 1f;
		soundVolume = 1f;
		musicVolume = 1f;
	}

	public static void SetMasterVolume(float value)
	{
		SoundEffect.MasterVolume = value;
	}

	public static float GetMasterVolume()
	{
		return SoundEffect.MasterVolume;
	}

	public static void SetSoundVolume(float value)
	{
		foreach (KeyValuePair<string, SoundEffectInstance> pair in sounds)
			pair.Value.Volume = value;
	}

	public static void SetMusicVolume(float value)
	{
		foreach (KeyValuePair<string, SoundEffectInstance> pair in music)
			pair.Value.Volume = value;
	}

	public static void PlaySound(string asset)
	{
		SoundEffectInstance instance = AssetManager.GetSound(asset).CreateInstance();
		sounds.Add(asset, instance);

		instance.Volume = soundVolume;
		instance.Play();
	}

	public static void PauseSound(string asset)
	{
		if (sounds.ContainsKey(asset))
		{
			SoundEffectInstance instance = sounds[asset];
			if (instance.State == SoundState.Playing)
				instance.Pause();
		}
	}

	public static void ResumeSound(string asset)
	{
		if (sounds.ContainsKey(asset))
		{
			SoundEffectInstance instance = sounds[asset];
			if (instance.State == SoundState.Paused)
				instance.Resume();
		}
	}

	public static void StopSound(string asset)
	{
		if (sounds.ContainsKey(asset))
		{
			SoundEffectInstance instance = sounds[asset];
			instance.Stop();
			sounds.Remove(asset);
		}
	}

	public static void PlayMusic(string asset, bool looped = false)
	{
		SoundEffectInstance instance = AssetManager.GetSound(asset).CreateInstance();
		music.Add(asset, instance);

		instance.IsLooped = looped;
		instance.Volume = musicVolume;
		instance.Play();
	}

	public static void PauseMusic(string asset)
	{
		if (music.ContainsKey(asset))
		{
			SoundEffectInstance instance = music[asset];
			if (instance.State == SoundState.Playing)
				instance.Pause();
		}
	}

	public static void ResumeMusic(string asset)
	{
		if (music.ContainsKey(asset))
		{
			SoundEffectInstance instance = music[asset];
			if (instance.State == SoundState.Paused)
				instance.Resume();
		}
	}

	public static void StopMusic(string asset)
	{
		if (music.ContainsKey(asset))
		{
			SoundEffectInstance instance = music[asset];
			instance.Stop();
			music.Remove(asset);
		}
	}
	
	public static void ResumeAll()
	{
		foreach (KeyValuePair<string, SoundEffectInstance> pair in sounds)
			if (pair.Value.State == SoundState.Paused)
				pair.Value.Play();

		foreach (KeyValuePair<string, SoundEffectInstance> pair in music)
			if (pair.Value.State == SoundState.Paused)
				pair.Value.Play();
	}

	public static void PauseAll()
	{
		foreach (KeyValuePair<string, SoundEffectInstance> pair in sounds)
			if (pair.Value.State == SoundState.Playing)
				pair.Value.Pause();

		foreach (KeyValuePair<string, SoundEffectInstance> pair in music)
			if (pair.Value.State == SoundState.Playing)
				pair.Value.Pause();
	}

	public static void StopAll()
	{
		foreach (KeyValuePair<string, SoundEffectInstance> pair in sounds)
			pair.Value.Stop();

		foreach (KeyValuePair<string, SoundEffectInstance> pair in music)
			pair.Value.Stop();
	}
}