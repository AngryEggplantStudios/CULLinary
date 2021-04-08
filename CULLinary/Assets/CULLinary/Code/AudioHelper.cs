using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public static class AudioHelper {
    // Credits:
    // https://gamedevbeginner.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/
    public static IEnumerator FadeAudio(AudioMixer audioMixer, string channel, float duration)
    {
        float rawVolume;
        audioMixer.GetFloat(channel, out rawVolume);
        float currentVolume = Mathf.Pow(10, rawVolume / 20);
        float targetVolume = 0.0001f;

        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime = currentTime + Time.deltaTime;
            float newVolume = Mathf.Lerp(currentVolume, targetVolume, currentTime / duration);
            audioMixer.SetFloat(channel, Mathf.Log10(newVolume) * 20);
            yield return null;
        }
        yield break;
    }
}
