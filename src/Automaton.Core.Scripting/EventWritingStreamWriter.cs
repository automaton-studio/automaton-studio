using System.Text;

namespace Automaton.Core.Scripting;

public class EventRaisingStreamWriter : MemoryStream
{
    public event EventHandler<string>? NewText;

    public override void Write(byte[] buffer, int offset, int count)
    {
        NewText?.Invoke(this, Encoding.Default.GetString(buffer, offset, count));

        base.Write(buffer, offset, count);
    }

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        NewText?.Invoke(this, Encoding.Default.GetString(buffer, offset, count));

        return base.WriteAsync(buffer, offset, count, cancellationToken);
    }

    public override void WriteByte(byte value)
    {
        NewText?.Invoke(this, value.ToString());

        base.WriteByte(value);
    }

    public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
    {
        NewText?.Invoke(this, Encoding.Default.GetString(buffer, offset, count));

        return base.BeginWrite(buffer, offset, count, callback, state);
    }
}
