
namespace HeadcountAllocation.Services
{
    [Serializable]
    ///<summary>This class extends<c>Response</c> and represents the result of a call to a non-void function.
    ///In addition to the behavior of <c>Response</c>, the class holds the value of the returned value in the variable <c>Value</c>.</summary>
    /// <typeparam name="T"></typeparam>

    public class Response<T> : Response
    {
        public T Value{get;}
        private Response(T value, string msg) : base(msg)
        {
            this.Value = value;
        }

        public static Response<T> FromValue(T value)
        {
            return new Response<T>(value, null);
        }

        public static Response<T> FromError(string msg)
        {
            return new Response<T>(default(T), msg);
        }
    }
}