using System;

namespace ChatCommands
{
    public interface ICommandHandler
    {
        void Handle(string input);
        bool CanHandle(string input);
    }
}
