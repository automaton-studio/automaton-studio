namespace Automaton.Studio.Domain;

public class CustomStepExecution : IDisposable
{
    private bool hasErrors;

    public Guid Id { get; set; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public CustomStepStatus Status { get; set; }

    public CustomStepExecution()
    {
        Id = Guid.NewGuid();
        Started = DateTime.UtcNow;
        Status = CustomStepStatus.Working;
    }

    public void HasErrors()
    {
        hasErrors = true;
    }

    public void Dispose()
    {
        Finished = DateTime.UtcNow;
        Status = hasErrors ? CustomStepStatus.Error : CustomStepStatus.Success;
    }
}
