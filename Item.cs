using System;
using System.Collections.Generic;

namespace HH_RU_Parser.Models
{
    public class Item
    {
        public string id { get; set; }
        public bool premium { get; set; }
        public string name { get; set; }
        public object department { get; set; }
        public bool has_test { get; set; }
        public bool response_letter_required { get; set; }
        public Area area { get; set; }
        public Salary salary { get; set; }
        public Type type { get; set; }
        public Address address { get; set; }
        public object response_url { get; set; }
        public object sort_point_distance { get; set; }
        public string published_at { get; set; }
        public string created_at { get; set; }
        public bool archived { get; set; }
        public string apply_alternate_url { get; set; }
        public object insider_interview { get; set; }
        public string url { get; set; }
        public string alternate_url { get; set; }
        public List<object> relations { get; set; }
        public Employer employer { get; set; }
        public Snippet snippet { get; set; }
        public object contacts { get; set; }
        public Schedule schedule { get; set; }
        public List<object> working_days { get; set; }
        public List<WorkingTimeInterval> working_time_intervals { get; set; }
        public List<object> working_time_modes { get; set; }
        public bool accept_temporary { get; set; }

        //public bool IsEqualVacancy(Item otherItem)
        //{
        //    if (this.id == otherItem.id)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
