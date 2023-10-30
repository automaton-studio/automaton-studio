namespace Automaton.Studio.Services
{
    public class KeyboardService
    {
        public IList<string> KeysDown { get; private set; } = new List<string>();

        public bool IsKeyDown(string key)
        {
            return KeysDown.Contains(key);
        }

        public void KeyDown(string key)
        {
            if (KeysDown.Contains(key))
                return;

            KeysDown.Add(key);
        }

        public void KeyUp(string key)
        {
            if (!KeysDown.Contains(key))
                return;

            KeysDown.Remove(key);
        }

        public bool ControlDown()
        {
            return IsKeyDown("ControlLeft") || IsKeyDown("ControlRight");
        }
    }
}
