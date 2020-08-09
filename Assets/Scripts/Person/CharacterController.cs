using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Inputs inputsManager;
    public Person character;

    public void BindInputs()
    {
        CustomInput right = inputsManager.GetInput("Right");
        right.OnDown += character.StartMovingRight;
        right.OnUp += character.StopMovingRight;
        right.OnHold += () => { character.Move(1); };

        CustomInput left = inputsManager.GetInput("Left");
        left.OnDown += character.StartMovingLeft;
        left.OnUp += character.StopMovingLeft;
        left.OnHold += () => { character.Move(-1); };

        CustomInput run = inputsManager.GetInput("Run");
        run.OnDown += character.ToggleRun;
        run.OnUp += character.ToggleRun;
    }

    private void Start()
    {
        BindInputs();
    }

    private void Update()
    {
        foreach(CustomInput inpt in inputsManager.inputs)
        {
            if (Input.GetKeyDown(inpt.key))
            {
                inpt.Down();
            }

            if (Input.GetKeyUp(inpt.key))
            {
                inpt.Up();
            }

            if (Input.GetKey(inpt.key))
            {
                inpt.Hold();
            }
        }
    }
}
