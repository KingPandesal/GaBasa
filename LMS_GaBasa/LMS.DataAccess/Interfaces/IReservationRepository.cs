using System;
using System.Collections.Generic;
using LMS.Model.Models.Transactions;

namespace LMS.DataAccess.Interfaces
{
    /// <summary>
    /// Repository interface for Reservation data access.
    /// </summary>
    public interface IReservationRepository
    {
        /// <summary>
        /// Adds a new reservation to the database.
        /// </summary>
        /// <param name="reservation">The reservation to add.</param>
        /// <returns>The generated ReservationID.</returns>
        int Add(Reservation reservation);

        /// <summary>
        /// Gets a reservation by ID.
        /// </summary>
        /// <param name="reservationId">The reservation ID.</param>
        /// <returns>The Reservation object, or null if not found.</returns>
        Reservation GetById(int reservationId);

        /// <summary>
        /// Gets all active reservations for a member.
        /// </summary>
        /// <param name="memberId">The member ID.</param>
        /// <returns>List of active reservations.</returns>
        List<Reservation> GetActiveByMemberId(int memberId);

        /// <summary>
        /// Checks if a member has an active reservation for any copy of a book.
        /// </summary>
        /// <param name="bookId">The book ID.</param>
        /// <param name="memberId">The member ID.</param>
        /// <returns>True if an active reservation exists.</returns>
        bool HasActiveReservationForBook(int bookId, int memberId);

        /// <summary>
        /// Checks if a specific book copy has an active reservation.
        /// </summary>
        /// <param name="copyId">The copy ID.</param>
        /// <returns>True if the copy has an active reservation.</returns>
        bool HasActiveReservationForCopy(int copyId);

        /// <summary>
        /// Gets the active reservation for a specific copy, if any.
        /// </summary>
        /// <param name="copyId">The copy ID.</param>
        /// <returns>The active Reservation, or null if none exists.</returns>
        Reservation GetActiveReservationByCopyId(int copyId);

        /// <summary>
        /// Updates the status of a reservation.
        /// </summary>
        /// <param name="reservationId">The reservation ID.</param>
        /// <param name="status">The new status (Active, Cancelled, Completed, Expired).</param>
        /// <returns>True if updated successfully.</returns>
        bool UpdateStatus(int reservationId, string status);

        /// <summary>
        /// Updates all active reservations that have passed their expiration date to "Expired" status.
        /// </summary>
        /// <returns>Number of reservations updated.</returns>
        int ExpireOverdueReservations();

        /// <summary>
        /// Gets the member ID from a user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The member ID, or 0 if not found.</returns>
        int GetMemberIdByUserId(int userId);

        /// <summary>
        /// Gets an unavailable copy ID for a book (for reservation purposes).
        /// </summary>
        /// <param name="bookId">The book ID.</param>
        /// <returns>A CopyID that is not Available, or 0 if none found.</returns>
        int GetUnavailableCopyIdForBook(int bookId);

        /// <summary>
        /// Gets all reservations (for management view).
        /// </summary>
        /// <returns>List of all reservations.</returns>
        List<Reservation> GetAll();

        /// <summary>
        /// Gets all reservations with joined member and book info for display.
        /// </summary>
        /// <returns>List of reservations for display.</returns>
        List<LMS.Model.DTOs.Reservation.DTOReservationView> GetAllForDisplay();

        /// <summary>
        /// Gets the status of a book copy by its ID.
        /// </summary>
        /// <param name="copyId">The copy ID.</param>
        /// <returns>The status string (Available, Borrowed, Reserved, etc.), or null if not found.</returns>
        string GetBookCopyStatus(int copyId);

        /// <summary>
        /// Checks if a book has any copies in the library.
        /// </summary>
        /// <param name="bookId">The book ID.</param>
        /// <returns>True if at least one copy exists.</returns>
        bool HasAnyCopies(int bookId);

        /// <summary>
        /// Gets the first (highest priority) active reservation for a book copy, ordered by ReservationDate ASC.
        /// </summary>
        /// <param name="copyId">The book copy ID.</param>
        /// <returns>The first reservation in the queue, or null if none.</returns>
        Reservation GetFirstInQueueByCopyId(int copyId);

        /// <summary>
        /// Gets the first (highest priority) active reservation for a book (any copy), ordered by ReservationDate ASC.
        /// </summary>
        /// <param name="bookId">The book ID.</param>
        /// <returns>The first reservation in the queue, or null if none.</returns>
        Reservation GetFirstInQueueByBookId(int bookId);

        /// <summary>
        /// Sets the expiration date for a reservation.
        /// </summary>
        /// <param name="reservationId">The reservation ID.</param>
        /// <param name="expirationDate">The expiration date to set.</param>
        /// <returns>True if updated successfully.</returns>
        bool SetExpirationDate(int reservationId, DateTime expirationDate);

        /// <summary>
        /// Gets the BookID for a given CopyID.
        /// </summary>
        /// <param name="copyId">The copy ID.</param>
        /// <returns>The BookID, or 0 if not found.</returns>
        int GetBookIdByCopyId(int copyId);

        /// <summary>
        /// Activates the next reservation in the queue when a copy becomes available.
        /// Sets ExpirationDate for the first member in queue.
        /// </summary>
        /// <param name="copyId">The book copy ID that became available.</param>
        /// <param name="reservationPeriodDays">Number of days until the reservation expires.</param>
        /// <returns>The reservation that was activated, or null if no reservations in queue.</returns>
        Reservation ActivateNextReservationInQueue(int copyId, int reservationPeriodDays);

        /// <summary>
        /// Checks if a member currently has an active borrow for any copy of a book.
        /// </summary>
        /// <param name="bookId">The book ID.</param>
        /// <param name="memberId">The member ID.</param>
        /// <returns>True if the member has an active (unreturned) borrow for this book.</returns>
        bool HasActiveBorrowForBook(int bookId, int memberId);
    }
}
