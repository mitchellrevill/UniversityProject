using System;

namespace UniversityProject.Model
{
    internal class LeaveRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int HoursUsed { get; set; }
        public bool IsApproved { get; private set; }

        public LeaveRequest()
        {
            IsApproved = false;
        }

        public int CalculateTotalDays()
        {
            return (EndDate - StartDate).Days + 1;
        }

        public bool IsLeaveOverlapping(LeaveRequest otherLeave)
        {
            return StartDate < otherLeave.EndDate && EndDate > otherLeave.StartDate;
        }

        public void ApproveLeave()
        {
            IsApproved = true;
        }

        public void RejectLeave()
        {
            IsApproved = false;
        }

        public void ExtendLeave(int additionalDays)
        {
            EndDate = EndDate.AddDays(additionalDays);
        }

        public void ShortenLeave(int fewerDays)
        {
            EndDate = EndDate.AddDays(-fewerDays);
        }
    }
}
