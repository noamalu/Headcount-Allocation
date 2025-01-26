namespace HeadcountAllocation.Services
{
    [Serializable]
    ///<summary>Class <c>Response</c> represents the result of a call to a void function. 
    ///If an exception was thrown, <c>ErrorOccured = true</c> and <c>ErrorMessage != null</c>. 
    ///Otherwise, <c>ErrorOccured = false</c> and <c>ErrorMessage = null</c>.</summary>
    public class Response
    {
        public string ErrorMessage{get;}
        public bool ErrorOccured { get => ErrorMessage != null; }
        public Response() { }
        public Response(string msg)
        {
            this.ErrorMessage = msg;
        }
    }
}