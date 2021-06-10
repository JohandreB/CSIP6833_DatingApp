using System;

namespace API.Helpers
{
    public class VisitParams : PaginationParams
    {
        public string visitPeriod {get; set; }
        public int UserId { get; set; }
        public string Predicate { get; set; }
    }
}