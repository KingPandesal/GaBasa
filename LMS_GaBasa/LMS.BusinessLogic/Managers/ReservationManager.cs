using System;
using System.Collections.Generic;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.Model.Models.Transactions;
using LMS.Model.DTOs.Reservation;

namespace LMS.BusinessLogic.Managers
{
    /// <summary>
    /// Manager for handling book reservations.
    /// </summary>
    public class ReservationManager : IReservationManager
    {
        private readonly IReservationRepository _reservationRepository;
        private const int DefaultReservationPeriodDays = 3;

        public ReservationManager() : this(new ReservationRepository()) { }

        public ReservationManager(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        }

        public Reservation CreateReservation(int bookId, int memberId)
        {
            if (bookId <= 0)
                throw new ArgumentException("Invalid book ID.", nameof(bookId));
            if (memberId <= 0)
                throw new ArgumentException("Invalid member ID.", nameof(memberId));

            if (HasActiveReservation(bookId, memberId))
                return null;

            int copyId = _reservationRepository.GetUnavailableCopyIdForBook(bookId);
            if (copyId <= 0)
                return null;

            var reservation = new Reservation
            {
                CopyID = copyId,
                MemberID = memberId,
                ReservationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(DefaultReservationPeriodDays),
                Status = "Active"
            };

            int reservationId = _reservationRepository.Add(reservation);
            if (reservationId <= 0)
                return null;

            reservation.ReservationID = reservationId;
            return reservation;
        }

        public bool HasActiveReservation(int bookId, int memberId)
        {
            if (bookId <= 0 || memberId <= 0)
                return false;
            return _reservationRepository.HasActiveReservationForBook(bookId, memberId);
        }

        public List<Reservation> GetActiveReservationsByMember(int memberId)
        {
            if (memberId <= 0)
                return new List<Reservation>();
            return _reservationRepository.GetActiveByMemberId(memberId);
        }

        public bool CancelReservation(int reservationId)
        {
            if (reservationId <= 0)
                return false;
            return _reservationRepository.UpdateStatus(reservationId, "Cancelled");
        }

        public int GetMemberIdByUserId(int userId)
        {
            if (userId <= 0)
                return 0;
            return _reservationRepository.GetMemberIdByUserId(userId);
        }

        public int ExpireOverdueReservations()
        {
            return _reservationRepository.ExpireOverdueReservations();
        }

        public List<Reservation> GetAllReservations()
        {
            return _reservationRepository.GetAll();
        }

        /// <summary>
        /// Gets all reservations with joined member and book info for display.
        /// </summary>
        public List<DTOReservationView> GetAllReservationsForDisplay()
        {
            return _reservationRepository.GetAllForDisplay();
        }
    }
}
