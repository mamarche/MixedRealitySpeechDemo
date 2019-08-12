using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectTypes
{
    Cube,
    Cylinder,
    Sphere
}

public class ObjectBehaviour : MonoBehaviour
{
    public ObjectTypes ObjectType;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void ChangeColor(Color newColor)
    {
        _renderer.material.color = newColor;
    }

    public void Move(Vector3 movement)
    {
        transform.position += movement;
    }

    // Update is called once per frame
    void Update()
    {
        switch (ObjectType)
        {
            case ObjectTypes.Cube:
                transform.Rotate(0f, 50f * Time.deltaTime, 0f);
                break;
            case ObjectTypes.Cylinder:
                transform.Rotate(50f * Time.deltaTime, 0f, 0f);
                break;
            case ObjectTypes.Sphere:

                break;
            default:
                break;
        }
    }
}
