using System;
using System.Collections.Generic;

namespace SmartHomeAdvanced
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    public class NoCommand : ICommand
    {
        public void Execute() => Console.WriteLine("Команда не назначена!");
        public void Undo() => Console.WriteLine("Отменять нечего!");
    }

    public class Light
    {
        public void On() => Console.WriteLine("Свет включен");
        public void Off() => Console.WriteLine("Свет выключен");
    }

    public class TV
    {
        public void On() => Console.WriteLine("Телевизор включен");
        public void Off() => Console.WriteLine("Телевизор выключен");
    }

    public class AirConditioner
    {
        public void On() => Console.WriteLine("Кондиционер включен");
        public void Off() => Console.WriteLine("Кондиционер выключен");
    }

    public class Curtains
    {
        public void Open() => Console.WriteLine("Шторы открыты");
        public void Close() => Console.WriteLine("Шторы закрыты");
    }

    public class LightOnCommand : ICommand
    {
        private Light light;
        public LightOnCommand(Light l) => light = l;
        public void Execute() => light.On();
        public void Undo() => light.Off();
    }

    public class LightOffCommand : ICommand
    {
        private Light light;
        public LightOffCommand(Light l) => light = l;
        public void Execute() => light.Off();
        public void Undo() => light.On();
    }

    public class TVOnCommand : ICommand
    {
        private TV tv;
        public TVOnCommand(TV t) => tv = t;
        public void Execute() => tv.On();
        public void Undo() => tv.Off();
    }

    public class AirOnCommand : ICommand
    {
        private AirConditioner ac;
        public AirOnCommand(AirConditioner a) => ac = a;
        public void Execute() => ac.On();
        public void Undo() => ac.Off();
    }

    public class CurtainsOpenCommand : ICommand
    {
        private Curtains curtains;
        public CurtainsOpenCommand(Curtains c) => curtains = c;
        public void Execute() => curtains.Open();
        public void Undo() => curtains.Close();
    }

    public class MacroCommand : ICommand
    {
        private List<ICommand> commands;
        public MacroCommand(List<ICommand> cmds) => commands = cmds;

        public void Execute()
        {
            foreach (var cmd in commands)
                cmd.Execute();
        }

        public void Undo()
        {
            for (int i = commands.Count - 1; i >= 0; i--)
                commands[i].Undo();
        }
    }

    public class RemoteControl
    {
        private ICommand[] slots = new ICommand[5];
        private Stack<ICommand> undoStack = new Stack<ICommand>();
        private Stack<ICommand> redoStack = new Stack<ICommand>();

        public RemoteControl()
        {
            for (int i = 0; i < slots.Length; i++)
                slots[i] = new NoCommand();
        }

        public void SetCommand(int slot, ICommand command)
        {
            if (slot < 0 || slot >= slots.Length)
            {
                Console.WriteLine("Неверный слот!");
                return;
            }
            slots[slot] = command;
        }

        public void PressButton(int slot)
        {
            slots[slot].Execute();
            undoStack.Push(slots[slot]);
            redoStack.Clear();
        }

        public void Undo()
        {
            if (undoStack.Count == 0)
            {
                Console.WriteLine("Нет команд для отмены!");
                return;
            }
            var cmd = undoStack.Pop();
            cmd.Undo();
            redoStack.Push(cmd);
        }

        public void Redo()
        {
            if (redoStack.Count == 0)
            {
                Console.WriteLine("Нет команд для повтора!");
                return;
            }
            var cmd = redoStack.Pop();
            cmd.Execute();
            undoStack.Push(cmd);
        }
    }

    class Program
    {
        static void Main()
        {
            var light = new Light();
            var tv = new TV();
            var ac = new AirConditioner();
            var curtains = new Curtains();

            var remote = new RemoteControl();

            remote.SetCommand(0, new LightOnCommand(light));
            remote.SetCommand(1, new TVOnCommand(tv));
            remote.SetCommand(2, new AirOnCommand(ac));
            remote.SetCommand(3, new CurtainsOpenCommand(curtains));
]
            var morningMacro = new MacroCommand(new List<ICommand>
            {
                new CurtainsOpenCommand(curtains),
                new LightOnCommand(light),
                new AirOnCommand(ac)
            });

            remote.SetCommand(4, morningMacro);

            remote.PressButton(0);
            remote.PressButton(4);

            Console.WriteLine("\nUndo:");
            remote.Undo();

            Console.WriteLine("\nRedo:");
            remote.Redo();
        }
    }
}
