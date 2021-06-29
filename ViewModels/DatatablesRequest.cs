
using Microsoft.AspNetCore.Mvc;
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
            OrderDirection = GetValueString(inputRequest, "order[0][dir]");
            string column = GetValueString(inputRequest, "order[0][column]");
            OrderBy = GetValueString(inputRequest, $"columns[{column}][data]") ;
        }

        public int Draw { set; get; }
        public int Start { set; get; }
        public int Length { set; get; }
        public string Keyword { set; get; }
        public int PageSize { set; get; }
        public int Skip { set; get; }
        [DefaultValue("asc")]
        public string OrderDirection { set; get; }
        public string OrderBy { set; get; }
 
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
