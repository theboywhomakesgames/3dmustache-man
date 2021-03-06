﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class CustomInput
{
    public delegate void InputHandler();

    public string name;
    public KeyCode key;
    public int btn;
    public bool isMouseKey;

    public event InputHandler OnHold, OnDown, OnUp;

    public void Down()
    {
        OnDown?.Invoke();
    }

    public void Hold()
    {
        OnHold?.Invoke();
    }

    public void Up()
    {
        OnUp?.Invoke();
    }

    public void ClearEvents()
    {
        OnHold = null;
        OnDown = null;
        OnUp = null;
    }
}

[CreateAssetMenu(menuName = "SO/InputManager")]
public class Inputs : ScriptableObject
{
    public CustomInput[] inputs;

    public CustomInput GetInput(string name)
    {
        foreach(CustomInput inpt in inputs)
        {
            if(inpt.name == name)
            {
                return inpt;
            }
        }

        return null;
    }

    public void ClearAll()
    {
        foreach(CustomInput i in inputs)
        {
            i.ClearEvents();
        }
    }
}
