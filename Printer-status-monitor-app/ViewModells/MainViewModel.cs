using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Printer_status_monitor_app.Modells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Printer_status_monitor_app.ViewModells
{
    public partial class MainViewModel : ObservableObject
    {
        private Stack<PrinterAction> UndoStack = new();
        private Stack<PrinterAction> RedoStack = new();

        private const string JsonPath = "printers.json";


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UndoCommand))]
        private bool canUndo;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RedoCommand))]
        private bool canRedo;

        [ObservableProperty]
        private ObservableCollection<Printer> printers = new();

        public MainViewModel()
        {
            LoadFromFileCommand.Execute(null);
        }

        [RelayCommand]
        private void SaveToFile()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(JsonPath, JsonSerializer.Serialize(printers, options));
        }

        [RelayCommand]
        private void LoadFromFile()
        {
            if (File.Exists(JsonPath))
            {
                var json = File.ReadAllText(JsonPath);
                var loaded = JsonSerializer.Deserialize<ObservableCollection<Printer>>(json);
                if (loaded != null)
                    Printers = loaded;
            }
        }

        [RelayCommand]
        private void DeletePrinter(Printer printer)
        {
            if (Printers.Contains(printer))
            {
                int index = Printers.IndexOf(printer);
                Printers.Remove(printer);

                UndoStack.Push(new PrinterAction
                {
                    Type = ActionType.Delete,
                    PrinterSnapshot = Clone(printer),
                    Index = index
                });

                RedoStack.Clear();
                UpdateUndoRedoState();
            }
        }

        [RelayCommand]
        private void AddNewPrinter()
        {
            var printer = new Printer
            {
                DigitalProperty = new DigitalProperty
                {
                    Name = "New Printer",
                    Location = "-",
                    IpAddress = "0.0.0.0",
                    ModelType = "-"
                }
            };

            Printers.Add(printer);
            UndoStack.Push(new PrinterAction
            {
                Type = ActionType.Add,
                PrinterSnapshot = Clone(printer)
            });

            RedoStack.Clear();
            UpdateUndoRedoState();
        }

        [RelayCommand(CanExecute = nameof(CanUndo))]
        private void Undo()
        {
            if (UndoStack.Count == 0) return;

            var action = UndoStack.Pop();

            switch (action.Type)
            {
                case ActionType.Add:
                    Printers.Remove(Printers.Last());
                    RedoStack.Push(action);
                    break;

                case ActionType.Delete:
                    Printers.Insert(action.Index, Clone(action.PrinterSnapshot));
                    RedoStack.Push(action);
                    break;

                case ActionType.Edit:
                    //TODO: Implement
                    break;
            }

            UpdateUndoRedoState();
        }

        [RelayCommand(CanExecute = nameof(CanRedo))]
        private void Redo()
        {
            if (RedoStack.Count == 0) return;

            var action = RedoStack.Pop();

            switch (action.Type)
            {
                case ActionType.Add:
                    Printers.Add(Clone(action.PrinterSnapshot));
                    UndoStack.Push(action);
                    break;

                case ActionType.Delete:
                    Printers.RemoveAt(action.Index);
                    UndoStack.Push(action);
                    break;

                case ActionType.Edit:
                    //TODO: Implement
                    break;
            }

            UpdateUndoRedoState();
        }
        private void UpdateUndoRedoState()
        {
            CanUndo = UndoStack.Count > 0;
            CanRedo = RedoStack.Count > 0;
        }
        private Printer Clone(Printer original)
        {
            var json = JsonSerializer.Serialize(original);
            return JsonSerializer.Deserialize<Printer>(json);
        }
    }
}
