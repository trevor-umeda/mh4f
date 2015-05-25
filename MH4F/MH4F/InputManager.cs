using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections;
namespace MH4F
{
    public class InputManager
    {

        InputQueue<Keys[]> inputs;
        KeyboardState lastKeyboardState;
        private ControlSetting controlSetting;

        private Keys[] keysPressed;

        readonly String[] DIRECTIONS = {"up","down","left","right" };
        readonly String[] ATTACKS = { "a", "b" };

        List<MoveInput> groundMoveList;
        List<MoveInput> dashList;
        public KeyboardState LastKeyboardState
        {
            get { return lastKeyboardState; }
        }
        public InputManager()
        {
            inputs = new InputQueue<Keys[]>();
            lastKeyboardState = new KeyboardState();
            groundMoveList = new List<MoveInput>();
            dashList = new List<MoveInput>();
            // TODO: this is a terrible place to put these. find a better spot
            //
            dashList.Add(new MoveInput("backstep", new List<string> { "4", "5", "4" }));
            dashList.Add(new MoveInput("dash", new List<string> { "6", "5", "6" }));
           
        }

        public String checkGroundMoves(Direction direction, KeyboardState newKeyboardState)
        {
          
            //inputs.Enqueue(newKeyboardState);
            enqueueState(newKeyboardState, controlSetting.Controls);
            // on a button press determine if a special move was inputted.
            //
            if (DetermineButtonPress(newKeyboardState))
            {

                foreach (MoveInput moveInput in groundMoveList)
                {
                    moveInput.resetCurrentInputCommandIndex();
                }
                
                foreach (Keys[] keyboardState in inputs)
                {
                    foreach (MoveInput moveInput in groundMoveList)
                    {
                        if (MoveInput.checkStringInputToKeyInput(moveInput.InputCommand[moveInput.CurrentInputCommandIndex], keyboardState, direction, controlSetting.Controls))
                        {
                           
                            moveInput.moveCurrentInputCommandIndex();
                            if (moveInput.CurrentInputCommandIndex >= moveInput.InputCommand.Count)
                            {
                               
                                lastKeyboardState = newKeyboardState;
                                System.Diagnostics.Debug.WriteLine("Activating " + moveInput.Name);
                                inputs.Reset();
                                return moveInput.Name;
                            }
                        }
                    }
                }
            }
            // Otherwise this is a movement special input
            //
            else
            {
                // Atm we only really care about reading a dash
                //
                foreach (MoveInput dashInput in dashList)
                {
                    dashInput.resetCurrentInputCommandIndex();
                }
                
                foreach (Keys[] keyboardState in inputs.GetReverseEnumerator)
                {
                    foreach (MoveInput dash in dashList)
                    {

                        if (MoveInput.checkStringInputToKeyInput(dash.InputCommand[dash.CurrentInputCommandIndex], keyboardState, direction, controlSetting.Controls))
                        {
                            dash.moveCurrentInputCommandIndex();
                            if (dash.CurrentInputCommandIndex >= dash.InputCommand.Count)
                            {
                                System.Diagnostics.Debug.WriteLine(dash.Name);
                                inputs.Reset();
                                return dash.Name;
                            }

                        }
                    }
                }
            }
            lastKeyboardState = newKeyboardState;
            return null;
        }

        public bool DetermineButtonPress(KeyboardState presentState)
        {
            //A attack button pressed
            if (MoveInput.KeyboardPressed(presentState, lastKeyboardState, controlSetting.Controls["a"]))
            {
                return true;
            }
            // B attack button pressed

            if (MoveInput.KeyboardPressed(presentState, lastKeyboardState, controlSetting.Controls["b"]))
            {
                System.Diagnostics.Debug.WriteLine("B button pressed");
                return true;
            }
            return false;
        }

        public void registerGroundMove(String name, List<String> input)
        {
           // input.Reverse();
            groundMoveList.Add(new MoveInput(name, input));
        }

        public void enqueueState(KeyboardState state, Dictionary<string, Keys> controls)
        {
            keysPressed = new Keys[state.GetPressedKeys().Length];
            int counter = 0;
          
            foreach (String attack in ATTACKS)
            {
                if (MoveInput.KeyboardPressed(state, lastKeyboardState, controls[attack]))
                {
                    keysPressed[counter] = controls[attack];
                    counter++;
                }
            }
            foreach(String direction in DIRECTIONS)
            {
                if (state.IsKeyDown(controls[direction]))
                {
                    keysPressed[counter] = controls[direction];
                    counter++;
                }
            }
            inputs.Enqueue(keysPressed);
        }
        
        public ControlSetting ControlSetting
        {
            get { return controlSetting; }
            set { this.controlSetting = value; }
        }
    }
}
