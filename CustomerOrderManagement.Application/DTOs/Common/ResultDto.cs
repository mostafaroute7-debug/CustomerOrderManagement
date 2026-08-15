using System.Collections.Generic;

namespace CustomerOrderManagement.Application.Results
{
    public class ResultDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        public ResultDto()
        {
            Errors = new List<string>();
        }
    }
}
