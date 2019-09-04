using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov
{
    interface InputHandler
    {
        void ProcessInput(ConsoleKeyInfo input);
    }
    sealed class ToyInputHandler : InputHandler
    {
        private int currentField;
        private StringBuilder buffer;
        public void ProcessInput(ConsoleKeyInfo input)
        {
            if (input.Key == ConsoleKey.Enter)
            {

            }
            else
            {
                if (buffer.Capacity == buffer.Length)
                {
                    buffer.EnsureCapacity(buffer.Length * 2);
                }
            }
        }

        public void SetFieldToEdit()
        {

        }
    }
    sealed class UserInputDispatcher
    {
        private InputHandler activeHandler;
        private ConsoleKey GoToNextFieldKey = ConsoleKey.N;
        private ConsoleKey GoToPrevFieldKey = ConsoleKey.P;
        public bool ProcessUserInput(ConsoleKeyInfo input)
        {
            if (input.Modifiers == ConsoleModifiers.Control)
            {
                ConsoleKey key = input.Key;
                if (key == GoToNextFieldKey)
                {

                }
                else if (key == GoToPrevFieldKey)
                {

                }
            }
            else
            {
                activeHandler.ProcessInput(input);
            }
            return true;
        }
    }
}
