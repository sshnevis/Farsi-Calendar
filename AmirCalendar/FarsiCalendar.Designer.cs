using System.Drawing;
using System.Windows.Forms;

namespace AmirCalendar
{
    partial class FarsiCalendar
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FarsiCalendar));
            this.lblToday = new System.Windows.Forms.Label();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.numYear = new System.Windows.Forms.NumericUpDown();
            this.numMonth = new System.Windows.Forms.NumericUpDown();
            this.lblEvent = new System.Windows.Forms.Label();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.eventPanel = new System.Windows.Forms.Panel();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.popupDatePicker = new AmirCalendar.PopupControl.PopupComboBox();
            this.table = new AmirCalendar.MyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table)).BeginInit();
            this.SuspendLayout();
            // 
            // lblToday
            // 
            this.lblToday.BackColor = System.Drawing.Color.Transparent;
            this.lblToday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblToday.Font = new System.Drawing.Font("Tahoma", 6.5F);
            this.lblToday.Location = new System.Drawing.Point(0, 0);
            this.lblToday.Name = "lblToday";
            this.lblToday.Size = new System.Drawing.Size(185, 14);
            this.lblToday.TabIndex = 0;
            this.lblToday.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblToday.Click += new System.EventHandler(this.lblToday_Click);
            // 
            // lblCurrent
            // 
            this.lblCurrent.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblCurrent.Font = new System.Drawing.Font("Tahoma", 6.5F);
            this.lblCurrent.Location = new System.Drawing.Point(0, 0);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(185, 14);
            this.lblCurrent.TabIndex = 0;
            this.lblCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCurrent.Click += new System.EventHandler(this.lblCurrent_Click);
            // 
            // numYear
            // 
            this.numYear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numYear.Location = new System.Drawing.Point(214, 165);
            this.numYear.Maximum = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this.numYear.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numYear.Name = "numYear";
            this.numYear.Size = new System.Drawing.Size(50, 18);
            this.numYear.TabIndex = 1;
            this.numYear.Tag = "";
            this.numYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numYear.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numYear.Value = new decimal(new int[] {
            1392,
            0,
            0,
            0});
            this.numYear.ValueChanged += new System.EventHandler(this.num_ValueChanged);
            // 
            // numMonth
            // 
            this.numMonth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numMonth.Location = new System.Drawing.Point(214, 189);
            this.numMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMonth.Name = "numMonth";
            this.numMonth.Size = new System.Drawing.Size(50, 18);
            this.numMonth.TabIndex = 2;
            this.numMonth.Tag = "";
            this.numMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numMonth.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMonth.ValueChanged += new System.EventHandler(this.num_ValueChanged);
            // 
            // lblEvent
            // 
            this.lblEvent.BackColor = System.Drawing.Color.Transparent;
            this.lblEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEvent.Font = new System.Drawing.Font("Tahoma", 6.5F);
            this.lblEvent.Location = new System.Drawing.Point(0, 0);
            this.lblEvent.Margin = new System.Windows.Forms.Padding(0);
            this.lblEvent.Name = "lblEvent";
            this.lblEvent.Padding = new System.Windows.Forms.Padding(2);
            this.lblEvent.Size = new System.Drawing.Size(100, 23);
            this.lblEvent.TabIndex = 0;
            // 
            // controlPanel
            // 
            this.controlPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("controlPanel.BackgroundImage")));
            this.controlPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.controlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.controlPanel.Size = new System.Drawing.Size(280, 41);
            this.controlPanel.TabIndex = 0;
            // 
            // eventPanel
            // 
            this.eventPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("eventPanel.BackgroundImage")));
            this.eventPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.eventPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.eventPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.eventPanel.Location = new System.Drawing.Point(0, 0);
            this.eventPanel.Name = "eventPanel";
            this.eventPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.eventPanel.Size = new System.Drawing.Size(280, 41);
            this.eventPanel.TabIndex = 0;
            this.eventPanel.Visible = false;
            // 
            // contentPanel
            // 
            this.contentPanel.BackColor = System.Drawing.Color.Transparent;
            this.contentPanel.Font = new System.Drawing.Font("Tahoma", 6.75F);
            this.contentPanel.Location = new System.Drawing.Point(0, 0);
            this.contentPanel.MaximumSize = new System.Drawing.Size(280, 155);
            this.contentPanel.MinimumSize = new System.Drawing.Size(280, 155);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.contentPanel.Size = new System.Drawing.Size(280, 155);
            this.contentPanel.TabIndex = 0;
            // 
            // popupDatePicker
            // 
            this.popupDatePicker.DropDownControl = null;
            this.popupDatePicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.popupDatePicker.FormattingEnabled = true;
            this.popupDatePicker.Location = new System.Drawing.Point(0, 0);
            this.popupDatePicker.Name = "popupDatePicker";
            this.popupDatePicker.Size = new System.Drawing.Size(121, 21);
            this.popupDatePicker.TabIndex = 0;
            // 
            // table
            // 
            this.table.AllowUserToAddRows = false;
            this.table.AllowUserToDeleteRows = false;
            this.table.AllowUserToResizeColumns = false;
            this.table.AllowUserToResizeRows = false;
            this.table.AutoGenerateColumns = false;
            this.table.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.table.ColorOne = System.Drawing.Color.Lavender;
            this.table.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(197)))), ((int)(((byte)(232)))));
            this.table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.table.Dock = System.Windows.Forms.DockStyle.Top;
            this.table.Location = new System.Drawing.Point(0, 0);
            this.table.MultiSelect = false;
            this.table.Name = "table";
            this.table.ReadOnly = true;
            this.table.RowHeadersVisible = false;
            this.table.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.table.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.table.Size = new System.Drawing.Size(280, 115);
            this.table.TabIndex = 0;
            this.table.TabStop = false;
            // 
            // FarsiCalendar
            // 
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(280, 20);
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MyGrid table;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Label lblToday;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.NumericUpDown numYear;
        private System.Windows.Forms.NumericUpDown numMonth;
        private System.Windows.Forms.Panel eventPanel;
        private System.Windows.Forms.Label lblEvent;
        private PopupControl.PopupComboBox popupDatePicker;
        private Panel contentPanel;
    }
}
