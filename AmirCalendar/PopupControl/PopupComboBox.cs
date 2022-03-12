using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace AmirCalendar.PopupControl
{
    [ToolboxItem(false), DesignTimeVisible(false)]
    internal class PopupComboBox : ComboBox
    {
        private IContainer components;
        private Popup dropDown;
        private Control dropDownControl;
        private DateTime dropDownHideTime;
        private EventHandler _DropDown;
        private EventHandler _DropDownClosed;

        public Control DropDownControl
        {
            get
            {
                return this.dropDownControl;
            }
            set
            {
                if (this.dropDownControl == value)
                    return;
                this.dropDownControl = value;
                if (this.dropDown != null)
                {
                    this.dropDown.Closed -= new ToolStripDropDownClosedEventHandler(this.dropDown_Closed);
                    this.dropDown.Dispose();
                }
                this.dropDown = new Popup(value);
                this.dropDown.Closed += new ToolStripDropDownClosedEventHandler(this.dropDown_Closed);
            }
        }

        public new bool DroppedDown
        {
            get
            {
                return this.dropDown.Visible;
            }
            set
            {
                if (this.DroppedDown)
                    this.HideDropDown();
                else
                    this.ShowDropDown();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new int DropDownWidth
        {
            get
            {
                return base.DropDownWidth;
            }
            set
            {
                base.DropDownWidth = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public new int DropDownHeight
        {
            get
            {
                return base.DropDownHeight;
            }
            set
            {
                base.DropDownHeight = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public new bool IntegralHeight
        {
            get
            {
                return base.IntegralHeight;
            }
            set
            {
                base.IntegralHeight = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public new System.Windows.Forms.ComboBox.ObjectCollection Items
        {
            get
            {
                return base.Items;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int ItemHeight
        {
            get
            {
                return base.ItemHeight;
            }
            set
            {
                base.ItemHeight = value;
            }
        }

        public new event EventHandler DropDown
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this._DropDown = this._DropDown + value;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this._DropDown = this._DropDown - value;
            }
        }

        public new event EventHandler DropDownClosed
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this._DropDownClosed = this._DropDownClosed + value;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this._DropDownClosed = this._DropDownClosed - value;
            }
        }

        public PopupComboBox()
        {
            this.dropDownHideTime = DateTime.UtcNow;
            this.InitializeComponent();
            base.DropDownHeight = base.DropDownWidth = 1;
            base.IntegralHeight = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                    this.components.Dispose();
                if (this.dropDown != null)
                    this.dropDown.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }

        private void dropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            this.dropDownHideTime = DateTime.UtcNow;
        }

        public void ShowDropDown()
        {
            if (this.dropDown == null)
                return;
            if ((DateTime.UtcNow - this.dropDownHideTime).TotalSeconds > 0.5)
            {
                if (this._DropDown != null)
                    this._DropDown((object)this, EventArgs.Empty);
                this.dropDown.Show((Control)this);
            }
            else
            {
                this.dropDownHideTime = DateTime.UtcNow.Subtract(new TimeSpan(0, 0, 1));
                this.Focus();
            }
        }

        public void HideDropDown()
        {
            if (this.dropDown == null)
                return;
            this.dropDown.Hide();
            if (this._DropDownClosed == null)
                return;
            this._DropDownClosed((object)this, EventArgs.Empty);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 8465 && NativeMethods.HIWORD(m.WParam) == 7)
                this.ShowDropDown();
            else
                base.WndProc(ref m);
        }
    }
}
