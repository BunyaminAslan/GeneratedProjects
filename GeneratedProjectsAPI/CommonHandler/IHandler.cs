using GeneratedProjectsAPI.CommonHandler.Models;

namespace GeneratedProjectsAPI.CommonHandler
{
    public interface IHandler
    {
        void Handle(RequestContext context);
        IHandler SetNext(IHandler nextHandler);
    }
}
