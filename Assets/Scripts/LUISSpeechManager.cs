using IntentRecognitionResults;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Intent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyJson;
using UnityEngine;

public class LUISSpeechManager : Singleton<LUISSpeechManager>
{
    
    [SerializeField] private string LuisAppId = "Your LUIS App Id";
    [SerializeField] private string LuisEndpointKey = "Your LUIS Endpoint Key";
    [SerializeField] private string LuisRegion = "westus";
    [SerializeField] private string LuisLanguage = "it-it";
    
    private SpeechConfig luisConfig;
    private IntentRecognizer recognizer;
    private IntentResult result = null;
    private bool readyToProcess = false;

    private void Start()
    {
        luisConfig = SpeechConfig.FromSubscription(LuisEndpointKey, LuisRegion);
        luisConfig.SpeechRecognitionLanguage = LuisLanguage;
    }

    public void StartIntentRecognition()
    {
        RecognizeIntentAsync();
    }
    public void StopRecognition()
    {
        StopIntentRecognition();
    }

    private void Update()
    {
        if (readyToProcess)
            ProcessIntent();
    }

    private async void RecognizeIntentAsync()
    {
        recognizer = new IntentRecognizer(luisConfig);
        
        // Creates a Language Understanding model using the app id, and adds specific intents from your model
        var model = LanguageUnderstandingModel.FromAppId(LuisAppId);
        recognizer.AddIntent(model, "changeColor", "changeColor");
        recognizer.AddIntent(model, "moveObject", "moveObject");

        // add event handlers
        recognizer.Recognized += Recognizer_Recognized;
        recognizer.Recognizing += Recognizer_Recognizing;
        recognizer.Canceled += Recognizer_Canceled;

        //Start recognizing
        Debug.Log("Say something...");
        await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
    }

    private void Recognizer_Canceled(object sender, IntentRecognitionCanceledEventArgs e)
    {
        readyToProcess = false;

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
        readyToProcess = false;
        Debug.Log($"Recognizing: {e.Result.Text}");
    }

    private void Recognizer_Recognized(object sender, IntentRecognitionEventArgs e)
    {
        if (e.Result.Reason == ResultReason.RecognizedIntent)
        {
            Debug.Log($"RECOGNIZED: Text={e.Result.Text}");
            Debug.Log($"    Intent Id: {e.Result.IntentId}.");

            string json = e.Result.Properties.GetProperty(PropertyId.LanguageUnderstandingServiceResponse_JsonResult);

            Debug.Log($"    Language Understanding JSON: {json}.");

            result = json.FromJson<IntentResult>();
            if (result != null)
            {
                readyToProcess = true;
            }
        }
        else if (e.Result.Reason == ResultReason.RecognizedSpeech)
        {
            readyToProcess = false;
            Debug.Log($"RECOGNIZED: Text={e.Result.Text}");
            Debug.Log($"    Intent not recognized.");
        }
        else if (e.Result.Reason == ResultReason.NoMatch)
        {
            readyToProcess = false;
            Debug.Log($"NOMATCH: Speech could not be recognized.");
        }
    }

    private void ProcessIntent()
    {
        switch (result.topScoringIntent.intent)
        {
            case "changeColor":
                SpeechManager.Instance.ChangeColor(result.entities.First(e => e.type == "object").entity, result.entities.First(e => e.type == "color").entity);
                break;
            case "moveObject":
                break;
            default:
                break;
        }

        readyToProcess = false;
        result = null;
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
