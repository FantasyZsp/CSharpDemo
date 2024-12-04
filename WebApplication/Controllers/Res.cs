using DotNetCommon.Data;

namespace YW.ModelManagement.Common
{
    public class Res<T>
    {
        public ResState State { get; set; }
        public string Code { get; set; }
        public string Msg { get; set; }
        public virtual object Payload { get; set; }
        public static implicit operator Res<T>(Result<T> res)
        {
            var newRes = new Res<T>();
            if (res.Success) newRes.State = ResState.OK;
            else newRes.State = ResState.BadRequest; // 参数校验调整为400
            newRes.Msg = res.Message;
            newRes.Code = res.Code.ToString();
            newRes.Payload = res.Data;
            return newRes;
        }
        public static implicit operator Res<T>(Res res)
        {
            return new Res<T>
            {
                Code = res.Code,
                Msg = res.Msg,
                Payload = res.Payload,
                State = res.State
            };
        }
    }

    public class Res : Res<object>
    {
        public static implicit operator Res(Result res)
        {
            var newRes = new Res();
            if (res.Success) newRes.State = ResState.OK;
            else newRes.State = ResState.BadRequest;
            newRes.Msg = res.Message;
            newRes.Code = res.Code.ToString();
            newRes.Payload = res.Data;
            return newRes;
        }

        public static Res NotOk(string message)
        {
            return new Res { Code = string.Empty, Msg = message, State = ResState.BadRequest };
        }
        public static Res Ok(object payload)
        {
            return new Res {Code = "0", Msg = null, State = ResState.OK, Payload = payload};
        }
        public static Res NotLogin(string message)
        {
            return new Res { Code = string.Empty, Msg = message, State = ResState.Unauthorized };
        }
        public static Res NotOk(string message, string code)
        {
            return new Res { Code = code, Msg = message, State = ResState.BadRequest };
        }

    }

    public class ResPage<T> : Res<Page<T>>
    {
        public static implicit operator ResPage<T>(ResultPage<T> res)
        {
            var newRes = new ResPage<T>();
            if (res.Success) newRes.State = ResState.OK;
            else newRes.State = ResState.BadRequest;
            newRes.Msg = res.Message;
            newRes.Code = res.Code.ToString();
            newRes.Payload = res.Data;
            return newRes;
        }
    }
    
    public class PageExt<T>
    {
        public Page<T> Page { get; set; }
        public object ExtData { get; set; }
    }

    public enum ResState
    {
        OK = 200,
        NoContent = 204,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        InternalServerError = 500
    }
}