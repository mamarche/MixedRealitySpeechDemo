using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Microsoft.CognitiveServices.Speech;

public class SpeechManager : Singleton<SpeechManager>
{
    [SerializeField] private ObjectBehaviour[] Targets;

    [SerializeField] private string SpeechServicesSubscriptionKey = "Your Subscription Key";
    [SerializeField] private string SpeechServicesRegion = "westeurope";
    [SerializeField] private AudioSource audioSource;

    private SpeechConfig speechConfig;

    private void Start()
    {
        speechConfig = SpeechConfig.FromSubscription(SpeechServicesSubscriptionKey, SpeechServicesRegion);
    }

    public void ChangeColor(string targetEntity, string colorEntity)
    {
        var target = TargetSolver(targetEntity);
        if (target == null)
            Debug.LogWarning($"Target {targetEntity} not found!");

        var color = ColorSolver(colorEntity);

        target.ChangeColor(color);
    }

    public void Move(string targetEntity, string direction)
    {
        var target = TargetSolver(targetEntity);
        if (target == null)
            Debug.LogWarning($"Target {targetEntity} not found!");

        var movement = DirectionSolver(direction);

        target.Move(movement);
    }

    public void Speech(string text)
    {
        using (var synthsizer = new SpeechSynthesizer(speechConfig, null))
        {
            // Starts speech synthesis, and returns after a single utterance is synthesized.
            var result = synthsizer.SpeakTextAsync(text).Result;

            // Checks result.
            string newMessage = string.Empty;
            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                var audioClip = ByteArrayToClip(result.AudioData);
                audioSource.clip = audioClip;
                audioSource.Play();

                Debug.Log("Speech synthesis succeeded!");
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                Debug.Log($"CANCELED:\nReason=[{cancellation.Reason}]\nErrorDetails=[{cancellation.ErrorDetails}]\nDid you update the subscription info?");
            }
        }
    }

    #region Solvers
    private ObjectBehaviour TargetSolver(string targetEntity)
    {
        switch (targetEntity)
        {
            case "cubo":
            case "box":
                return Targets.FirstOrDefault(t => t.ObjectType == ObjectTypes.Cube);
            case "cilindro":
            case "tubo":
                return Targets.FirstOrDefault(t => t.ObjectType == ObjectTypes.Cylinder);
            case "sfera":
            case "palla":
            case "pallone":
                return Targets.FirstOrDefault(t => t.ObjectType == ObjectTypes.Sphere);
            default:
                return null;
        }
    }
    private Color ColorSolver(string colorEntity)
    {
        switch (colorEntity)
        {
            case "rosso": return Color.red;
            case "verde": return Color.green;
            case "blu": return Color.blue;
            case "giallo": return Color.yellow;
            case "viola": 
            case "lilla":
            case "magenta":
                return Color.magenta;
            case "nero": return Color.black;
            case "bianco": return Color.white;
            case "rosa": return new Color(255, 0, 255);
            case "marrone": return new Color(100, 50, 0);
            case "ciano": return Color.cyan;
            case "arancione": return new Color(255, 150, 0);
            default: return Color.white;
        }
    }
    private Vector3 DirectionSolver(string direction)
    {
        switch (direction.ToLower())
        {
            case "sinistra": return Vector3.left;
            case "destra": return Vector3.right;
            default: return Vector3.zero;
        }
    }
    #endregion

    #region Helpers
    private AudioClip ByteArrayToClip(byte[] data)
    {
        // Since native playback is not yet supported on Unity yet (currently only supported on Windows/Linux Desktop),
        // use the Unity API to play audio here as a short term solution.
        // Native playback support will be added in the future release.
        var sampleCount = data.Length / 2;
        var audioData = new float[sampleCount];
        for (var i = 0; i < sampleCount; ++i)
        {
            audioData[i] = (short)(data[i * 2 + 1] << 8 | data[i * 2]) / 32768.0F;
        }

        // The default output audio format is 16K 16bit mono
        var audioClip = AudioClip.Create("SynthesizedAudio", sampleCount, 1, 16000, false);
        audioClip.SetData(audioData, 0);
        return audioClip;
    }
    #endregion
}
