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
                throw new InvalidOperationException("You already have an active reservation for this book.");

            // Check if the book has any copies at all
            if (!_reservationRepository.HasAnyCopies(bookId))
                throw new InvalidOperationException("This book cannot be reserved because there are no copies available in the library.");

            int copyId = _reservationRepository.GetUnavailableCopyIdForBook(bookId);
            if (copyId <= 0)
                throw new InvalidOperationException("All copies of this book are currently available. You can borrow it directly.");

            // For queue-based reservations, never set ExpirationDate at creation time.
            // ExpirationDate will be set when the book becomes available and it's this member's turn.
            var reservation = new Reservation
            {
                CopyID = copyId,
                MemberID = memberId,
                ReservationDate = DateTime.Now,
                ExpirationDate = null, // Will be set when book becomes available
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

        /// <summary>
        /// Activates the next reservation in the queue when a copy becomes available.
        /// Sets ExpirationDate for the first member in queue.
        /// </summary>
        public Reservation ActivateNextReservationInQueue(int copyId)
        {
            if (copyId <= 0)
                return null;

            return _reservationRepository.ActivateNextReservationInQueue(copyId, DefaultReservationPeriodDays);
        }

        /// <summary>
        /// Checks if a member can borrow a specific copy based on reservation priority.
        /// </summary>
        public bool CanMemberBorrowCopy(int copyId, int memberId)
        {
            if (copyId <= 0 || memberId <= 0)
                return false;

            // Get the BookID for this copy
            int bookId = _reservationRepository.GetBookIdByCopyId(copyId);
            if (bookId <= 0)
                return true; // If we can't find the book, allow borrowing

            // Get the first reservation in queue for this book
            var firstReservation = _reservationRepository.GetFirstInQueueByBookId(bookId);
            
            // If no reservations, anyone can borrow
            if (firstReservation == null)
                return true;

            // Check if this member is the first in queue
            if (firstReservation.MemberID == memberId)
            {
                // Member is first in queue - check if reservation has expired
                if (firstReservation.ExpirationDate.HasValue && firstReservation.ExpirationDate.Value < DateTime.Now)
                {
                    // Reservation has expired - anyone can borrow
                    // (The ExpireOverdueReservations job should have marked this as Expired)
                    return true;
                }
                
                // Member is first in queue with valid (or not yet activated) reservation
                return true;
            }

            // Member is not first in queue
            // Check if the first reservation has an expiration date set and has expired
            if (firstReservation.ExpirationDate.HasValue && firstReservation.ExpirationDate.Value < DateTime.Now)
            {
                // First reservation has expired - allow borrowing
                return true;
            }

            // If first reservation has no expiration date, book is not yet available for anyone
            // If first reservation has expiration date but not expired, only that member can borrow
            if (!firstReservation.ExpirationDate.HasValue)
            {
                // Book is still reserved and not yet ready for pickup
                return false;
            }

            // First reservation is active and not expired - only that member can borrow
            return false;
        }

        /// <summary>
        /// Marks a reservation as completed when the book is borrowed.
        /// </summary>
        public bool CompleteReservation(int reservationId)
        {
            if (reservationId <= 0)
                return false;
            return _reservationRepository.UpdateStatus(reservationId, "Completed");
        }

        /// <summary>
        /// Gets the first active reservation for a book's copy.
        /// </summary>
        public Reservation GetFirstReservationForCopy(int copyId)
        {
            if (copyId <= 0)
                return null;

            int bookId = _reservationRepository.GetBookIdByCopyId(copyId);
            if (bookId <= 0)
                return null;

            return _reservationRepository.GetFirstInQueueByBookId(bookId);
        }
    }
}
