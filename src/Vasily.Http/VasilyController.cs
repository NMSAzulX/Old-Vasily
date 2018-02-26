using System.Linq;
using Vasily.Driver;

namespace Microsoft.AspNetCore.Mvc
{
    public class VasilyController<T> : Controller where T: class
    {
        protected VasilyDapper<T> SqlHandler;
        private ReturnResult _result;

        public VasilyController()
        {
            
        }

        public VasilyController(string key)
        {
            _result.Status = 200;
            SqlHandler = new VasilyDapper<T>(key);
        }

        protected ReturnResult Result()
        {
            var item = ModelState.Values.Take(1).SingleOrDefault();
            string msg = item.Errors.Where(b => !string.IsNullOrWhiteSpace(b.ErrorMessage)).Take(1).SingleOrDefault().ErrorMessage;
            _result.Msg = msg;
            _result.Status = 2;
            return _result;
        }
        protected ReturnResult Result(bool value,string succeed="操作成功！",string faild = "操作失败！")
        {
            if (value)
            {
                _result.Msg = succeed;
            }
            else
            {
                _result.Status = 1;
                _result.Msg = faild;
            }
            return _result;
        }

        protected ReturnResult Result(object value,string msg="数据为空！")
        {
            if (value != null)
            {
                _result.Data = value;
            }
            else
            {
                _result.Status = 1;
                _result.Msg = msg;
            }
            return _result;

        }

        //protected ReturnResult CorrectMsg(string msg)
        //{
        //    _result.Status = 0;
        //    _result.Msg = msg;
        //    return _result;
        //}

        //protected ReturnResult CorrectObject(object value)
        //{
        //    _result.Status = 0;
        //    _result.Data = value;
        //    return _result;
        //}
        protected ReturnResult Msg(string value, int status=200)
        {
            _result.Msg = value;
            _result.Status = status;
            return _result;
        }
    }

    public struct ReturnResult
    {
        public string Msg;
        public object Data;
        public int Status;
    }
}
