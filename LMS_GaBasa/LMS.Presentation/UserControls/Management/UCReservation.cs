using LMS.BusinessLogic.Managers;
using LMS.BusinessLogic.Managers.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.Presentation.Popup.Reservation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS.Presentation.UserControls.Management
{
    public partial class UCReservation : UserControl
    {
        private readonly IReservationManager _reservationManager;

        public UCReservation()
        {
            InitializeComponent();

            // Initialize the reservation manager
            _reservationManager = new ReservationManager();

            // Wire up Load event
            this.Load += UCReservation_Load;
        }

        private void UCReservation_Load(object sender, EventArgs e)
        {
            // Expire any overdue reservations (those past their expiration date)
            try
            {
                int expiredCount = _reservationManager.ExpireOverdueReservations();
                if (expiredCount > 0)
                {
                    Debug.WriteLine($"Expired {expiredCount} overdue reservation(s).");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to expire overdue reservations: {ex.Message}");
            }

            // Load and display reservations
            LoadReservations();
        }

        private void LoadReservations()
        {
            try
            {
                // TODO: Implement loading reservations into DgwInventory
                // This would typically involve getting all reservations and binding to the DataGridView
                // var reservations = _reservationManager.GetAllReservations();
                // DgwInventory.DataSource = reservations;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load reservations: {ex.Message}");
            }
        }

        private void BtnAddReservation_Click(object sender, EventArgs e)
        {
            AddReservation addReservation = new AddReservation();
            addReservation.ShowDialog();

            // Refresh the list after adding
            LoadReservations();
        }

        // end code
    }
}
