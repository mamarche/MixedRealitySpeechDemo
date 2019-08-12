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

    private Transform _transform;
    private MeshRenderer _renderer;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _renderer = GetComponent<MeshRenderer>();
    }

    public void ChangeColor(Color newColor)
    {
        _renderer.material.color = newColor;
    }

    public void Move(Vector3 movement)
    {
        _transform.position += movement;
    }

    // Update is called once per frame
    void Update()
    {
        switch (ObjectType)
        {
            case ObjectTypes.Cube:
                _transform.Rotate(0f, 50f * Time.deltaTime, 0f);
                break;
            case ObjectTypes.Cylinder:
                _transform.Rotate(50f * Time.deltaTime, 0f, 0f);
                break;
            case ObjectTypes.Sphere:

                break;
            default:
                break;
        }
    }
}
