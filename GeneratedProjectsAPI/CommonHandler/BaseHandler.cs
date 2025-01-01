using GeneratedProjectsAPI.CommonHandler.Models;

namespace GeneratedProjectsAPI.CommonHandler
{
    public abstract class BaseHandler : IHandler
    {
        private IHandler _nextHandler;

        public IHandler SetNext(IHandler nextHandler)
        {
            _nextHandler = nextHandler;
            return nextHandler;
        }

        public virtual void Handle(RequestContext context)
        {
            _nextHandler?.Handle(context);
        }
    }
}
