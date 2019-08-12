using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Intent;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LUISSpeechManager : Singleton<LUISSpeechManager>
{
    
    [SerializeField] private string LuisAppId = "Your LUIS App Id";
    [SerializeField] private string LuisEndpointKey = "Your LUIS Endpoint Key";
    [SerializeField] private string LuisRegion = "westus";
    [SerializeField] private string LuisLanguage = "it-it";
    
    private SpeechConfig luisConfig;
    private IntentRecognizer recognizer;

    private void Start()
    {
        luisConfig = SpeechConfig.FromSubscription(LuisEndpointKey, LuisRegion);
        luisConfig.SpeechRecognitionLanguage = LuisLanguage;

        RecognizeIntentAsync();
    }

    private async void RecognizeIntentAsync()
    {
        recognizer = new IntentRecognizer(luisConfig);
        
        // Creates a Language Understanding model using the app id, and adds specific intents from your model
        var model = LanguageUnderstandingModel.FromAppId(LuisAppId);
        recognizer.AddIntent(model, "changeColor", "changeColor");
        recognizer.AddIntent(model, "moveObject", "moveObject");

        // add event handlers
        recognizer.SessionStarted += (s, e) => { Debug.Log("Session Started"); };
        recognizer.SessionStopped += (s, e) => { Debug.Log("Session Stopped"); };
        recognizer.SpeechStartDetected += (s, e) => { Debug.Log("Speech Start Detected"); };
        recognizer.SpeechEndDetected += (s, e) => { Debug.Log("Speech End Detected"); };

        recognizer.Recognized += Recognizer_Recognized;
        recognizer.Recognizing += Recognizer_Recognizing;
        recognizer.Canceled += Recognizer_Canceled;

        //Start recognizing
        Debug.Log("Say something...");
        await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
    }

    private void Recognizer_Canceled(object sender, IntentRecognitionCanceledEventArgs e)
    {
        var cancellation = CancellationDetails.FromResult(e.Result);
        Debug.Log($"CANCELED: Reason={cancellation.Reason}");

        if (cancellation.Reason == CancellationReason.Error)
        {
            Debug.Log($"CANCELED: ErrorCode={cancellation.ErrorCode}");
            Debug.Log($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
            Debug.Log($"CANCELED: Did you update the subscription info?");
        }
    }

    private void Recognizer_Recognizing(object sender, IntentRecognitionEventArgs e)
    {
        Debug.Log($"Recognizing: {e.Result.Text}");
    }

    private void Recognizer_Recognized(object sender, IntentRecognitionEventArgs e)
    {
        if (e.Result.Reason == ResultReason.RecognizedIntent)
        {
            Debug.Log($"RECOGNIZED: Text={e.Result.Text}");
            Debug.Log($"    Intent Id: {e.Result.IntentId}.");
            Debug.Log($"    Language Understanding JSON: {e.Result.Properties.GetProperty(PropertyId.LanguageUnderstandingServiceResponse_JsonResult)}.");
        }
        else if (e.Result.Reason == ResultReason.RecognizedSpeech)
        {
            Debug.Log($"RECOGNIZED: Text={e.Result.Text}");
            Debug.Log($"    Intent not recognized.");
        }
        else if (e.Result.Reason == ResultReason.NoMatch)
        {
            Debug.Log($"NOMATCH: Speech could not be recognized.");
        }
    }

    void OnDisable()
    {
        StopIntentRecognition();
    }

    /// <summary>
    /// IntentRecognizer & event handlers cleanup after use
    /// </summary>
    private async void StopIntentRecognition()
    {
        if (recognizer != null)
        {
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            recognizer.Recognizing -= Recognizer_Recognizing;
            recognizer.Recognized -= Recognizer_Recognized;
            recognizer.Canceled -= Recognizer_Canceled;
            recognizer.Dispose();
            recognizer = null;
            
        }
    }
}
