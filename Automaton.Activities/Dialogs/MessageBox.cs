using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using System;

namespace Automaton.Activities
{
    /// <summary>
    /// Writes a text string to a message box.
    /// </summary>
    [Action(
        Type = "MessageBox",
        DisplayName = "Message Box",
        Description = "Write text to a message box.",
        Category = "Dialogs",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class MessageBox : Activity
    {
        [ActivityInput(Hint = "Message box text.", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public string? Text { get; set; }

        [ActivityInput(Hint = "Message box caption.", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public string? Caption { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            System.Windows.MessageBox.Show(Text, Caption);

            return Done();
        }
    }
}
