using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Payroll.ViewModels
{
    public class DatatablesRequest
    {
        public DatatablesRequest(List<InputRequest> inputRequest)
        {
            Draw = GetValueInt(inputRequest, "draw");
            Start = GetValueInt(inputRequest, "start");
            Length = GetValueInt(inputRequest, "length");
            Keyword = GetValueString(inputRequest, "search[value]");
            PageSize = Length != null ? Convert.ToInt32(Length) : 0;
            Skip = Start != null ? Start :0 ;
        }

        public int Draw { set; get; }
        public int Start { set; get; }
        public int Length { set; get; }
        public string Keyword { set; get; }
        public int PageSize { set; get; }
        public int Skip { set; get; }

        public int GetValueInt(List<InputRequest> inputRequest, string key)
        {
            return int.Parse(inputRequest.Where(column => column.Key == key).FirstOrDefault().Value);
             
        }
        public string GetValueString(List<InputRequest> inputRequest, string key)
        {
            return inputRequest.Where(column => column.Key == key).FirstOrDefault().Value;
        }
    }



}
