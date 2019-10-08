using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum IntentEnum
{
    None,
    ChangeColor,
    Move
}

public class BuiltinSpeechManager : Singleton<BuiltinSpeechManager>
{
    private IntentEnum intent;
    private string target;
    private string color;
    private string direction;

    #region Intent Commands
    public void ChangeColor()
    {
        intent = IntentEnum.ChangeColor;
        Debug.Log($"Set intent to {intent}");
        SpeechManager.Instance.Speech($"Set intent to {intent}");
    }
    public void MoveObject()
    {
        intent = IntentEnum.Move;
        Debug.Log($"Set intent to {intent}");
        SpeechManager.Instance.Speech($"Set intent to {intent}");
    }
    #endregion

    #region Target Commands
    public void SetCubeTarget()
    {
        target = "cubo";
        Debug.Log($"Set target to {target}");
        SpeechManager.Instance.Speech($"Set target to {target}");
    }
    public void SetCylinderTarget()
    {
        target = "cilindro";
        Debug.Log($"Set target to {target}");
        SpeechManager.Instance.Speech($"Set target to {target}");
    }
    public void SetSphereTarget()
    {
        target = "sfera";
        Debug.Log($"Set target to {target}");
        SpeechManager.Instance.Speech($"Set target to {target}");
    }
    #endregion

    #region Color Commands
    public void SetColorRed()
    {
        if (intent == IntentEnum.ChangeColor && !string.IsNullOrEmpty(target))
            SpeechManager.Instance.ChangeColor(target, "rosso");
        else
            Debug.LogWarning($"Intent or target doesn't match: Intent={intent} - Target={target} ");
    }

    public void SetColorBlue()
    {
        if (intent == IntentEnum.ChangeColor && !string.IsNullOrEmpty(target))
            SpeechManager.Instance.ChangeColor(target, "blu");
        else
            Debug.LogWarning($"Intent or target doesn't match: Intent={intent} - Target={target} ");
    }

    public void SetColorGreen()
    {
        if (intent == IntentEnum.ChangeColor && !string.IsNullOrEmpty(target))
            SpeechManager.Instance.ChangeColor(target, "verde");
        else
            Debug.LogWarning($"Intent or target doesn't match: Intent={intent} - Target={target} ");
    }

    public void SetColorYellow()
    {
        if (intent == IntentEnum.ChangeColor && !string.IsNullOrEmpty(target))
            SpeechManager.Instance.ChangeColor(target, "giallo");
        else
            Debug.LogWarning($"Intent or target doesn't match: Intent={intent} - Target={target} ");
    }
    #endregion

    #region Direction Commands
    public void ToLeft()
    {
        direction = "sinistra";
        Debug.Log($"Set direction to {direction}");
        Move();
    }
    public void ToRight()
    {
        direction = "destra";
        Debug.Log($"Set direction to {direction}");
        Move();
    }
    private void Move()
    {
        if (intent == IntentEnum.Move && !string.IsNullOrEmpty(target) && !string.IsNullOrEmpty(direction))
            SpeechManager.Instance.Move(target, direction);
        else
            Debug.LogWarning($"Intent or target doesn't match: Intent={intent} - Target={target} ");
    }
    #endregion
}
