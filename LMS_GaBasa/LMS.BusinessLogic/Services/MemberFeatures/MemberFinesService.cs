using LMS.BusinessLogic.Managers;
using LMS.DataAccess.Repositories;
using LMS.Model.DTOs.Circulation;
using LMS.Model.DTOs.Fine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Services.MemberFeatures
{
    /// <summary>
    /// Service that prepares member fines data for presentation.
    /// Keeps UI logic out of the UserControl and follows single responsibility.
    /// </summary>
    public class MemberFinesService
    {
        private readonly CirculationManager _circulationManager;
        private readonly ReservationRepository _reservationRepository;

        public MemberFinesService()
        {
            var repo = new CirculationRepository();
            _circulationManager = new CirculationManager(repo);
            _reservationRepository = new ReservationRepository();
        }

        /// <summary>
        /// Returns a composed view model containing member info and fines for the specified memberId.
        /// </summary>
        public DTOMemberFinesView GetMemberFinesView(int memberId)
        {
            var view = new DTOMemberFinesView();

            if (memberId <= 0)
                return view;

            // Member info (use formatted id helper)
            DTOCirculationMemberInfo member = null;
            try
            {
                member = _circulationManager.GetMemberByFormattedId($"MEM-{memberId:D4}");
            }
            catch
            {
                member = null;
            }

            // Fines
            List<DTOFineRecord> fines = null;
            try
            {
                fines = _circulationManager.GetFinesByMemberId(memberId) ?? new List<DTOFineRecord>();
            }
            catch
            {
                fines = new List<DTOFineRecord>();
            }

            view.MemberInfo = member;
            view.Fines = fines;

            // Compose OverduePerDayText
            decimal rate = member?.FineRate ?? 0m;
            view.OverduePerDayText = $"• Overdue: ₱{rate:N2} per day";

            // Compute account standing
            view.AccountStanding = ComputeAccountStanding(member);

            return view;
        }

        private string ComputeAccountStanding(DTOCirculationMemberInfo member)
        {
            if (member == null)
                return "Unknown";

            bool hasFines = member.TotalUnpaidFines > 0m;
            bool hasOverdues = member.OverdueCount > 0;
            decimal maxCap = member.MaxFineCap;

            if (maxCap > 0m && member.TotalUnpaidFines >= maxCap)
                return "Suspended";

            if (hasFines || hasOverdues)
                return "Restricted";

            return "Good";
        }

        /// <summary>
        /// Filter fines by a search term that matches Status or FineType.
        /// Case-insensitive.
        /// </summary>
        public List<DTOFineRecord> FilterFines(IEnumerable<DTOFineRecord> fines, string search)
        {
            if (fines == null)
                return new List<DTOFineRecord>();

            if (string.IsNullOrWhiteSpace(search))
                return fines.ToList();

            string q = search.Trim();
            return fines.Where(f =>
                    (!string.IsNullOrWhiteSpace(f.Status) && f.Status.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                 || (!string.IsNullOrWhiteSpace(f.FineType) && f.FineType.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
            ).ToList();
        }

        /// <summary>
        /// Helper to estimate days late for an overdue fine using FineAmount / FineRate.
        /// Returns -1 if cannot compute.
        /// </summary>
        public int EstimateDaysLate(DTOFineRecord fine, DTOCirculationMemberInfo member)
        {
            if (fine == null || member == null) return -1;
            if (string.IsNullOrWhiteSpace(fine.FineType) || !fine.FineType.Equals("Overdue", StringComparison.OrdinalIgnoreCase))
                return -1;
            if (member.FineRate <= 0m) return -1;

            // Round up to nearest day
            decimal days = fine.FineAmount / member.FineRate;
            int d = (int)Math.Ceiling(days);
            return d < 0 ? 0 : d;
        }
    }
}
