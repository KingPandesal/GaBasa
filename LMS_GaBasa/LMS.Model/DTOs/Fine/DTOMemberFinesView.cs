using System;
using System.Collections.Generic;
using LMS.Model.DTOs.Circulation;

namespace LMS.Model.DTOs.Fine
{
    /// <summary>
    /// DTO that aggregates member info and the fines for a presentation layer.
    /// </summary>
    public class DTOMemberFinesView
    {
        public DTOCirculationMemberInfo MemberInfo { get; set; }
        public List<DTOFineRecord> Fines { get; set; } = new List<DTOFineRecord>();

        /// <summary>
        /// Human-friendly account standing: Good, Restricted, Suspended.
        /// </summary>
        public string AccountStanding { get; set; }

        /// <summary>
        /// Formatted overdue-per-day string (example: " Overdue: ₱10.00 per day")
        /// </summary>
        public string OverduePerDayText { get; set; }

        public decimal TotalOutstanding => MemberInfo?.TotalUnpaidFines ?? 0m;
    }
}