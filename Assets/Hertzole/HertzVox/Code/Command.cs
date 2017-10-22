namespace Hertzole.HertzVox.Commands
{
    public class Command
    {
        private string commandName = "";
        public string CommandName { get { return commandName; } }

        public Command() { }

        public Command(string commandName)
        {
            this.commandName = commandName;
        }

        public virtual void Execute(string[] arguments) { }
    }
}
