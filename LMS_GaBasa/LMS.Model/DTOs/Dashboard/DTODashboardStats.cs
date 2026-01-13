using System;
using System.Collections.Generic;

namespace LMS.Model.DTOs.Dashboard
{
    /// <summary>
    /// Data transfer object containing all dashboard statistics.
    /// </summary>
    public class DTODashboardStats
    {
        // Summary cards
        public int TotalBooks { get; set; }
        public int BorrowedBooks { get; set; }
        public decimal BorrowedBooksPercentChange { get; set; }
        public int ReservedBooks { get; set; }
        public decimal ReservedBooksPercentChange { get; set; }
        public int OverdueBooks { get; set; }
        public decimal OverdueBooksPercentChange { get; set; }
        public decimal TotalFines { get; set; }
        public DateTime FinesLastUpdated { get; set; }

        // Charts data
        // Replaced CategoryDistribution with MostUse: username -> last login timestamp
        public Dictionary<string, DateTime> MostUse { get; set; } = new Dictionary<string, DateTime>();
        public Dictionary<string, int> BorrowingTrend { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> BorrowingByUserType { get; set; } = new Dictionary<string, int>();

        // Welcome message
        public string UserDisplayName { get; set; }
    }
}
