using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services.Models;
using System;

namespace Automaton.MessageBox
{
    /// <summary>
    /// Writes a text string to a message box.
    /// </summary>
    [Action(
        Category = "Dialogs",
        Description = "Write text to a standard message box."
    )]
    public class MessageBox : Elsa.Services.Activity
    {
        public string? Text { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            System.Windows.MessageBox.Show($"{Text}");

            return Done();
        }
    }
}
