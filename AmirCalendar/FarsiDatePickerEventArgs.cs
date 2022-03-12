using System;

namespace AmirCalendar
{
    public class FarsiDatePickerEventArgs : EventArgs
    {
        public FarsiDatePickerEventArgs(string newFarsiDate, string oldFarsiDate)
        {
            if (!FarsiDateHelper.ValidateFarsiDate(newFarsiDate) || (!string.IsNullOrEmpty(oldFarsiDate) && !FarsiDateHelper.ValidateFarsiDate(oldFarsiDate)))
                throw new Exception("Incorrect Persian Date.");
            NewFarsiDate = newFarsiDate;
            OldFarsiDate = oldFarsiDate;
        }

        public string NewFarsiDate { get; private set; }
        public string OldFarsiDate { get; private set; }
    }
}
