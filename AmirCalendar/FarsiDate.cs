using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace AmirCalendar
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FarsiDate
    {
        internal event Action<string, string> DateChanged;
        internal event Action FormatChanged;
        internal event Action ThemeChanged;
        internal event Action ModeChanged;
        private string _pSelectedDate;
        private DateFormat _format;
        private CalendarTheme _theme;
        private ControlType _mode;

        private void OnDateChanged(string newFarsiDate, string oldFarsiDate)
        {
            if (DateChanged != null) DateChanged(newFarsiDate, oldFarsiDate);
        }

        private void OnFormatChanged()
        {
            if (FormatChanged != null) FormatChanged();
        }

        private void OnThemeChanged()
        {
            if (ThemeChanged != null) ThemeChanged();
        }

        private void OnModeChanged()
        {
            if (ModeChanged != null) ModeChanged();
        }

        public FarsiDate()
        {
            _format = DateFormat.Long;
            _mode = ControlType.DatePicker;
            _pSelectedDate = FarsiDateHelper.GetShortFarsiDate(DateTime.Now);
            _theme = CalendarTheme.WhiteSmoke;
        }

        /// <summary>
        /// Gets or sets mode of control. MonthCalendar or DatePicker
        /// </summary>
        [DefaultValue(typeof(ControlType), "DatePicker"), Description("Gets or sets mode of control. MonthCalendar or DatePicker")]
        public ControlType Mode
        {
            get { return _mode; }
            set
            {
                if (value == _mode) return;
                _mode = value;
                OnModeChanged();
            }
        }

        /// <summary>
        /// Gets or sets the theme assigned to the control.
        /// </summary>
        [DefaultValue(typeof(CalendarTheme), "WhiteSmoke"), Description("Gets or sets the theme assigned to the control.")]
        public CalendarTheme Theme
        {
            get { return _theme; }
            set
            {
                if (value == _theme) return;
                _theme = value;
                OnThemeChanged();
            }
        }

        /// <summary>
        /// Gets or sets format of the date represented by this instance.
        /// </summary>
        [DefaultValue(typeof(DateFormat), "Long"), NotifyParentProperty(true), Description("Gets or sets format of the date represented by this instance.")]
        public DateFormat Format
        {
            get { return _format; }
            set
            {
                if (_format == value) return;
                _format = value;
                OnFormatChanged();
            }
        }

        /// <summary>
        /// Gets or sets the persian date value assigned to the control.
        /// </summary>
        [NotifyParentProperty(true), Editor(typeof(TypeEditor), typeof(UITypeEditor)), RefreshProperties(RefreshProperties.All), Description("Gets or sets the persian date value assigned to the control.")]
        public string FarsiSelectedDate
        {
            get
            {
                if (string.IsNullOrEmpty(_pSelectedDate))
                    return FarsiDateHelper.GetShortFarsiDate(DateTime.Now);
                if (!FarsiDateHelper.ValidateFarsiDate(_pSelectedDate))
                    throw new Exception("Incorrect Persian Date.");
                return _pSelectedDate;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = FarsiDateHelper.GetShortFarsiDate(DateTime.Now);
                else if (!FarsiDateHelper.ValidateFarsiDate(value))
                    throw new Exception("Incorrect Persian Date.");
                if (_pSelectedDate == value) return;
                var oldValue = _pSelectedDate;
                _pSelectedDate = value;
                OnDateChanged(value, oldValue);
            }
        }

        /// <summary>
        /// Gets or sets the gregorian date value assigned to the control.
        /// </summary>
        [Browsable(false)]
        public DateTime GregorianSelectedDate
        {
            get { return FarsiDateHelper.GetGregorianDate(FarsiSelectedDate); }
        }

        /// <summary>
        /// Gets the persian year component of the date represented by this instance.
        /// </summary>
        [Browsable(false)]
        public int FarsiYear
        {
            get { return FarsiDateHelper.GetSectionOfDate(GregorianSelectedDate, true, SectionOfDate.Year); }
        }

        /// <summary>
        /// Gets the persian month component of the date represented by this instance.
        /// </summary>
        [Browsable(false)]
        public int FarsiMonth
        {
            get { return FarsiDateHelper.GetSectionOfDate(GregorianSelectedDate, true, SectionOfDate.Month); }
        }

        /// <summary>
        /// Gets the day of the persian month represented by this instance.
        /// </summary>
        [Browsable(false)]
        public int FarsiDay
        {
            get { return FarsiDateHelper.GetSectionOfDate(GregorianSelectedDate, true, SectionOfDate.Day); }
        }

        /// <summary>
        /// Gets the number of days in the specified month and year of the current persian selected date.
        /// </summary>
        [Browsable(false)]
        public int NumberOfDaysInFarsiSelectedMonth
        {
            get { return FarsiDateHelper.GetNumberOfDaysInFarsiMonth(FarsiSelectedDate); }
        }

        public override string ToString()
        {
            return Format == DateFormat.Short
                                     ? FarsiDateHelper.ToFarsiDigit(FarsiDateHelper.GetShortFarsiDate(GregorianSelectedDate))
                                     : FarsiDateHelper.ToFarsiDigit(FarsiDateHelper.GetLongFarsiDate(GregorianSelectedDate));
        }
    }
}
