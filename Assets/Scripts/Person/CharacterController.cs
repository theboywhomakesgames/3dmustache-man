using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Inputs inputsManager;
    public Person character;

    public Transform indicator;

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

        CustomInput jump = inputsManager.GetInput("Jump");
        CustomInput down = inputsManager.GetInput("Down");

        jump.OnDown += character.Jump;
        down.OnDown += character.Down;

        CustomInput trigger = inputsManager.GetInput("Trigger");
        trigger.OnHold += character.Trigger;
    }

    private void Start()
    {
        BindInputs();
    }

    private void Update()
    {
        foreach(CustomInput inpt in inputsManager.inputs)
        {
            if (inpt.isMouseKey)
            {
                if (Input.GetMouseButton(inpt.btn))
                {
                    inpt.Hold();
                }

                if (Input.GetMouseButtonDown(inpt.btn))
                {
                    inpt.Up();
                }

                if (Input.GetMouseButtonUp(inpt.btn))
                {
                    inpt.Down();
                }
            }
            else
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


        Vector3 diff = indicator.position - transform.position;

        if (diff.x > 0 != character.isFacingRight)
        {
            character.Flip();
        }
    }
}
