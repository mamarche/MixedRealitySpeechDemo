using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class SpeechManager : Singleton<SpeechManager>
{
    [SerializeField]
    private ObjectBehaviour[] Targets;

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
        switch (direction)
        {
            case "sinistra": return Vector3.left;
            case "destra": return Vector3.right;
            default: return Vector3.zero;
        }
    }
    #endregion
}
