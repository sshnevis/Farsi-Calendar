using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AmirCalendar
{
    public class DataGridViewFarsiDatePickerColumn : DataGridViewColumn
    {
        private readonly DataGridViewFarsiDatePickerCell _cellTemplate;
        private DateFormat _dateFormat;
        private CalendarTheme _theme;
        private bool _showFarsiDigitInCell;
        public delegate void FarsiDatePickerColumnDateChangedHandler(DataGridViewFarsiDatePickerCell cell, FarsiDatePickerEventArgs e);
        public event FarsiDatePickerColumnDateChangedHandler CellDateChanged;
        protected virtual void OnCellDateChanged(DataGridViewFarsiDatePickerCell cell, FarsiDatePickerEventArgs e)
        {
            if (CellDateChanged != null) CellDateChanged(cell, e);
        }

        public DataGridViewFarsiDatePickerColumn()
        {
            _dateFormat = DateFormat.Short;
            _theme = CalendarTheme.WhiteSmoke;
            _showFarsiDigitInCell = false;
            _cellTemplate = new DataGridViewFarsiDatePickerCell();
            _cellTemplate.CellDateChanged += OnCellDateChanged;
            CellTemplate = _cellTemplate;
        }

        public override object Clone()
        {
            var column = (DataGridViewFarsiDatePickerColumn)base.Clone();
            if (column != null)
            {
                column.DateFormat = DateFormat;
                column.Theme = Theme;
                column.ShowFarsiDigitInCell = ShowFarsiDigitInCell;
            }
            return column;
        }

        /// <summary>
        /// Gets or sets format of the date represented by this instance.
        /// </summary>
        [Description("Gets or sets format of the date represented by this instance.")]
        public DateFormat DateFormat
        {
            get { return _dateFormat; }
            set
            {
                _dateFormat = value;
                _cellTemplate.DateFormat = value;
            }
        }

        /// <summary>
        /// Show persian digit in cell.
        /// </summary>
        [Description("Show persian digit in cell.")]
        public bool ShowFarsiDigitInCell
        {
            get { return _showFarsiDigitInCell; }
            set
            {
                _showFarsiDigitInCell = value;
                _cellTemplate.ShowFarsiDigitInCell = value;
            }
        }

        /// <summary>
        /// Gets or sets the theme assigned to the control.
        /// </summary>
        [Description("Gets or sets the theme assigned to the control.")]
        public CalendarTheme Theme
        {
            get { return _theme; }
            set
            {
                _theme = value;
                _cellTemplate.Theme = value;
            }
        }
    }

    public class DataGridViewFarsiDatePickerCell : DataGridViewTextBoxCell
    {
        private DateFormat _dateFormat;
        private CalendarTheme _theme;
        private bool _showFarsiDigitInCell;
        internal event Action<DataGridViewFarsiDatePickerCell, FarsiDatePickerEventArgs> CellDateChanged;
        internal void OnCellDateChanged(DataGridViewFarsiDatePickerCell arg1, FarsiDatePickerEventArgs arg2)
        {
            if (CellDateChanged != null) CellDateChanged(arg1, arg2);
        }

        public override object Clone()
        {
            var cell = (DataGridViewFarsiDatePickerCell)base.Clone();
            cell.DateFormat = DateFormat;
            cell.Theme = Theme;
            cell.ShowFarsiDigitInCell = ShowFarsiDigitInCell;
            cell.CellDateChanged = OnCellDateChanged;
            return cell;
        }

        /// <summary>
        /// Gets or sets format of the date represented by this instance.
        /// </summary>
        [Description("Gets or sets format of the date represented by this instance.")]
        public DateFormat DateFormat
        {
            get { return _dateFormat; }
            set
            {
                _dateFormat = value;
                if (DataGridView != null)
                    DataGridView.InvalidateCell(this);
            }
        }

        /// <summary>
        /// Gets or sets the theme assigned to the control.
        /// </summary>
        [Description("Gets or sets the theme assigned to the control.")]
        public CalendarTheme Theme
        {
            get { return _theme; }
            set
            {
                _theme = value;
                if (DataGridView != null)
                    DataGridView.InvalidateCell(this);
            }
        }

        /// <summary>
        /// Show persian digit in cell.
        /// </summary>
        [Description("Show persian digit in cell.")]
        public bool ShowFarsiDigitInCell
        {
            get { return _showFarsiDigitInCell; }
            set
            {
                _showFarsiDigitInCell = value;
                if (DataGridView != null)
                    DataGridView.InvalidateCell(this);
            }
        }

        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            var val = (string)value;
            if (FarsiDateHelper.ValidateFarsiDate(val))
            {
                if (DateFormat == DateFormat.Long)
                    val = FarsiDateHelper.GetLongFarsiDate(FarsiDateHelper.GetGregorianDate(val));
                if (ShowFarsiDigitInCell)
                    val = FarsiDateHelper.ToFarsiDigit(val);
            }
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, val, errorText, cellStyle, advancedBorderStyle, paintParts);
        }

        private FarsiDatePickerEditingControl _editControl = null;
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            _editControl = DataGridView.EditingControl as FarsiDatePickerEditingControl;
            if (_editControl == null || initialFormattedValue == null) return;
            _editControl.Value.Format = DateFormat;
            _editControl.Value.Theme = Theme;
            _editControl.CancelDateChanged = true;
            _editControl.Value.FarsiSelectedDate = (string)initialFormattedValue;
            _editControl.CancelDateChanged = false;
            _editControl.DateChanged += ctl_DateChanged;
        }

        void ctl_DateChanged(object sender, FarsiDatePickerEventArgs e)
        {
            // do you want remove datepicker after changed date? uncomment below line
            //_editControl.DateChanged -= ctl_DateChanged;
            OnCellDateChanged(this, e);
            //DataGridView.EndEdit();
        }

        public override void DetachEditingControl()
        {
            _editControl.DateChanged -= ctl_DateChanged;
            base.DetachEditingControl();
        }

        public override Type EditType
        {
            get { return typeof(FarsiDatePickerEditingControl); }
        }

        public override Type ValueType
        {
            get { return typeof(object); }
        }

        public override object DefaultNewRowValue
        {
            get { return string.Empty; }
        }
    }

    class FarsiDatePickerEditingControl : FarsiCalendar, IDataGridViewEditingControl
    {
        private bool _valueChanged = false;

        public FarsiDatePickerEditingControl()
        {

        }

        public object EditingControlFormattedValue
        {
            get { return this.Value.FarsiSelectedDate; }
            set { this.Value.FarsiSelectedDate = (string)value; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {

        }

        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
        {
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {

        }

        public int EditingControlRowIndex { get; set; }

        public DataGridView EditingControlDataGridView { get; set; }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        public bool EditingControlValueChanged
        {
            get { return _valueChanged; }
            set { _valueChanged = value; }
        }

        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }

        internal bool CancelDateChanged { private get; set; }

        protected override void OnDateChanged(FarsiDatePickerEventArgs e)
        {
            if (CancelDateChanged) return;
            _valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnDateChanged(e);
        }
    }
}
