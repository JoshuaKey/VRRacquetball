using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileInput : InputField
{
    protected override void Start()
    {
        keyboardType = (TouchScreenKeyboardType)(-1);
        base.Start();
    }
}