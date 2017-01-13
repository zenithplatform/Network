using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Network.Api.Messages
{
    public class RequestHandlerResult
    {
        ReplyActions _preActions, _postActions = null;
        bool _handlerExecutedSuccessfully = false;

        public RequestHandlerResult()
        {
            _preActions = new ReplyActions();
            _postActions = new ReplyActions();
        }

        public HubResponseMessageBase ReplyMessage { get; set; }

        public ReplyActions PreReplyActions
        {
            get { return _preActions; }
        }

        public ReplyActions PostReplyActions
        {
            get { return _postActions; }
        }

        public bool ShouldReply
        {
            get;
            set;
        }

        public bool HandlerExecutedSuccessfully
        {
            get { return _handlerExecutedSuccessfully; }
            set { _handlerExecutedSuccessfully = value; }
        }

        public Exception ExceptionDetails
        {
            get;
            set;
        }
    }

    public class ReplyActions
    {
        List<Action> _innerList = null;

        public ReplyActions()
        {
            _innerList = new List<Action>();
        }

        public void Add(Action action)
        {
            _innerList.Add(action);
        }

        public void Clear()
        {
            _innerList.Clear();
        }

        public IEnumerable<Action> GetAll()
        {
            return _innerList;
        }

        public void ExecuteAll()
        {
            foreach (Action action in _innerList)
                action();
        }
    }

    //public class PreReplyActions<T> where T : class
    //{
    //    Queue<Action<T>> _innerQueue = null;

    //    public void Add(Action<T> action, T arg)
    //    {
    //        if (_innerQueue == null)
    //            _innerQueue = new Queue<Action<T>>();

    //        _innerQueue.Enqueue(action);
    //    }

    //    public Action<T> GetNext()
    //    {
    //        if(_innerQueue != null)
    //            return _innerQueue.Dequeue();

    //        return null;
    //    }

    //    public void Clear()
    //    {
    //        if (_innerQueue != null)
    //            _innerQueue.Clear();
    //    }
    //}

    //public class PostReplyAction<T> where T : class
    //{

    //}
}
