using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class DynamicFOV : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float maxFOV = 80f;
    [SerializeField] float fovChangeSpeed = 1.4f;
    [SerializeField] float fovThreshold = 0.2f;

    private Camera _cam;
    private float _defaultFieldOfView = 0;
    private float directionDelta = 0;
    private float lastDirectionMagnitude;

    void Start()
    {
        _cam = Camera.main;
        _defaultFieldOfView = _cam.fieldOfView;
    }

    private bool InRangeWithZero(float x)
    {
        return x >= 0 && x <= fovThreshold;
    }
    // Update is called once per frame
    void Update()
    {
        directionDelta = player.transform.position.magnitude - lastDirectionMagnitude;
        var fovChange = Time.deltaTime * fovChangeSpeed;

        if (InRangeWithZero(directionDelta))
        {
            if (_cam.fieldOfView >= _defaultFieldOfView)
            {
                _cam.fieldOfView -= fovChange;
            }
            else
            {
                _cam.fieldOfView += fovChange;
            }
        }
        else
        {
            if (_cam.fieldOfView <= maxFOV)
            {
                _cam.fieldOfView += fovChange;
            }
            else
            {
                _cam.fieldOfView -= fovChange;
            }
        }

        lastDirectionMagnitude = player.transform.position.magnitude;
    }
}

