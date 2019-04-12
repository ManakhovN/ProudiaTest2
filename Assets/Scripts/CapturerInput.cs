using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturerInput : MonoBehaviour, ICapturerInput {
    [SerializeField] KeyCode choosePath = KeyCode.Space;
    [SerializeField] KeyCode saveScreenshot = KeyCode.Return;
    public event Action OnChoosePathClick = delegate { };
    public event Action OnSaveScreenshot = delegate { };
    public void Update()
    {
        if (Input.GetKeyDown(choosePath))
            OnChoosePathClick();
        if (Input.GetKeyDown(saveScreenshot))
            OnSaveScreenshot();
    }
}
