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

        /// <summary>
        /// Activates the next reservation in the queue when a copy becomes available.
        /// Sets ExpirationDate for the first member in queue.
        /// Should be called when a book copy status changes to "Available".
        /// </summary>
        /// <param name="copyId">The book copy ID that became available.</param>
        /// <returns>The reservation that was activated, or null if no reservations in queue.</returns>
        Reservation ActivateNextReservationInQueue(int copyId);

        /// <summary>
        /// Checks if a member can borrow a specific copy based on reservation priority.
        /// Returns true if:
        /// - There are no active reservations for the book, OR
        /// - The member is the first in the reservation queue with a non-expired reservation
        /// </summary>
        /// <param name="copyId">The book copy ID.</param>
        /// <param name="memberId">The member ID attempting to borrow.</param>
        /// <returns>True if the member can borrow; otherwise false.</returns>
        bool CanMemberBorrowCopy(int copyId, int memberId);

        /// <summary>
        /// Marks a reservation as completed when the book is borrowed.
        /// </summary>
        /// <param name="reservationId">The reservation ID.</param>
        /// <returns>True if successfully completed.</returns>
        bool CompleteReservation(int reservationId);

        /// <summary>
        /// Gets the first active reservation for a book's copy.
        /// </summary>
        /// <param name="copyId">The copy ID.</param>
        /// <returns>The first reservation, or null if none.</returns>
        Reservation GetFirstReservationForCopy(int copyId);

        /// <summary>
        /// Checks if a member currently has an active (unreturned) borrow for any copy of a book.
        /// </summary>
        /// <param name="bookId">The book ID.</param>
        /// <param name="memberId">The member ID.</param>
        /// <returns>True if the member has an active borrow for this book.</returns>
        bool HasActiveBorrowForBook(int bookId, int memberId);
    }
}
