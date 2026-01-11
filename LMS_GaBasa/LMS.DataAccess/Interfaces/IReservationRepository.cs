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
    }
}
