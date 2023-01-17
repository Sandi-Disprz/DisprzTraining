using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DisprzTraining.Models;

namespace DisprzTraining.CustomExceptions
{
   public class NotIdFoundException:ApplicationException{
        public int?StatusCode{get;set;}
        public string? ErrorMessage{get;set;}
    }
    public class DateTimeMisMatchException:ApplicationException{
        public int?StatusCode{get;set;}
        public string? ErrorMessage{get;set;}

    }
    public class MeetingOverLapException:ApplicationException{
        public int?StatusCode{get;set;}
        public string? ErrorMessage{get;set;}
    }
    public class NoDataFoundException:ApplicationException{
        public int?StatusCode{get;set;}
        public string? ErrorMessage{get;set;}
    }
}