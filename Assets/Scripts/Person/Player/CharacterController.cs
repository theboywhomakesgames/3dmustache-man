using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeTweaker))]
public class CharacterController : MonoBehaviour
{
    public Inputs inputsManager;
    public Person character_;

    public Person character
    {
        get
        {
            if(character_ == null)
            {
                character_ = GetComponent<Person>();
            }

            return character_;
        }

        set
        {
            character_ = value;
        }
    }

    public Transform indicator;

    private int _actionsCount = 0;
    private float _actionTime = 0.1f;
    private TimeTweaker _timeTweaker;

    public void BindInputs()
    {
        inputsManager.ClearAll();

        CustomInput right = inputsManager.GetInput("Right");
        right.OnDown += character.StartMovingRight;
        right.OnDown += StartAction;
        right.OnUp += character.StopMovingRight;
        right.OnUp += StopAction;
        right.OnHold += () => { character.Move(1); };

        CustomInput left = inputsManager.GetInput("Left");
        left.OnDown += character.StartMovingLeft;
        left.OnDown += StartAction;
        left.OnUp += character.StopMovingLeft;
        left.OnUp += StopAction;
        left.OnHold += () => { character.Move(-1); };

        CustomInput run = inputsManager.GetInput("Run");
        run.OnDown += character.ToggleRun;
        run.OnUp += character.ToggleRun;

        CustomInput jump = inputsManager.GetInput("Jump");
        CustomInput down = inputsManager.GetInput("Down");

        jump.OnDown += character.Jump;
        jump.OnDown += StartTimedAction;
        down.OnDown += character.Down;
        down.OnDown += StartTimedAction;

        CustomInput trigger = inputsManager.GetInput("Trigger");
        trigger.OnHold += character.Trigger;
        trigger.OnDown += StartAction;
        trigger.OnUp += StopAction;
    }

    private void StartAction()
    {
        _actionsCount++;
        _timeTweaker.FastenUp();
    }

    private void StartTimedAction()
    {
        StartAction();
        Invoke(nameof(StopAction), _actionTime);
    }

    private void StopAction()
    {
        _actionsCount--;
        if (_actionsCount <= 0)
        {
            _timeTweaker.SlowDown();
        }
    }

    private void Start()
    {
        _timeTweaker = GetComponent<TimeTweaker>();
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
                    inpt.Down();
                }

                if (Input.GetMouseButtonUp(inpt.btn))
                {
                    inpt.Up();
                }
            }
            else
            {
                if (Input.GetKey(inpt.key))
                {
                    inpt.Hold();
                }

                if (Input.GetKeyDown(inpt.key))
                {
                    inpt.Down();
                }

                if (Input.GetKeyUp(inpt.key))
                {
                    inpt.Up();
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
