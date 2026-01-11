using System;
using System.Collections.Generic;
using LMS.Model.Models.Transactions;
using LMS.Model.DTOs.Reservation;

namespace LMS.BusinessLogic.Managers.Interfaces
{
    /// <summary>
    /// Interface for managing book reservations.
    /// </summary>
    public interface IReservationManager
    {
        /// <summary>
        /// Creates a reservation for a book by finding an unavailable copy to reserve.
        /// </summary>
        /// <param name="bookId">The book ID to reserve.</param>
        /// <param name="memberId">The member ID making the reservation.</param>
        /// <returns>The created Reservation object, or null if reservation failed.</returns>
        Reservation CreateReservation(int bookId, int memberId);

        /// <summary>
        /// Checks if a member already has an active reservation for a specific book.
        /// </summary>
        /// <param name="bookId">The book ID.</param>
        /// <param name="memberId">The member ID.</param>
        /// <returns>True if the member already has an active reservation for this book.</returns>
        bool HasActiveReservation(int bookId, int memberId);

        /// <summary>
        /// Gets all active reservations for a member.
        /// </summary>
        /// <param name="memberId">The member ID.</param>
        /// <returns>List of active reservations.</returns>
        List<Reservation> GetActiveReservationsByMember(int memberId);

        /// <summary>
        /// Cancels a reservation by ID.
        /// </summary>
        /// <param name="reservationId">The reservation ID.</param>
        /// <returns>True if successfully cancelled.</returns>
        bool CancelReservation(int reservationId);

        /// <summary>
        /// Gets the member ID from a user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The member ID, or 0 if not found.</returns>
        int GetMemberIdByUserId(int userId);

        /// <summary>
        /// Updates all active reservations that have passed their expiration date to "Expired" status.
        /// Should be called on application startup or when viewing reservations.
        /// </summary>
        /// <returns>Number of reservations that were expired.</returns>
        int ExpireOverdueReservations();

        /// <summary>
        /// Gets all reservations (for management view).
        /// </summary>
        /// <returns>List of all reservations.</returns>
        List<Reservation> GetAllReservations();

        /// <summary>
        /// Gets all reservations with joined member and book info for display.
        /// </summary>
        List<DTOReservationView> GetAllReservationsForDisplay();
    }
}
