using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace AmirCalendar.PopupControl
{
    [ToolboxItem(false), DesignTimeVisible(false)]
    internal class Popup : ToolStripDropDown
    {
        private bool focusOnOpen = true;
        private bool acceptAlt = true;
        private IContainer components;
        private Control content;
        private PopupAnimations showingAnimation;
        private PopupAnimations hidingAnimation;
        private int animationDuration;
        private Control opener;
        private Popup ownerPopup;
        private Popup childPopup;
        private bool resizableTop;
        private bool resizableLeft;
        private bool isChildPopupOpened;
        private bool resizable;
        private ToolStripControlHost host;
        private VisualStyleRenderer sizeGripRenderer;

        public Control Content
        {
            get
            {
                return this.content;
            }
        }

        public PopupAnimations ShowingAnimation
        {
            get
            {
                return this.showingAnimation;
            }
            set
            {
                if (this.showingAnimation == value)
                    return;
                this.showingAnimation = value;
            }
        }

        public PopupAnimations HidingAnimation
        {
            get
            {
                return this.hidingAnimation;
            }
            set
            {
                if (this.hidingAnimation == value)
                    return;
                this.hidingAnimation = value;
            }
        }

        public int AnimationDuration
        {
            get
            {
                return this.animationDuration;
            }
            set
            {
                if (this.animationDuration == value)
                    return;
                this.animationDuration = value;
            }
        }

        public bool FocusOnOpen
        {
            get
            {
                return this.focusOnOpen;
            }
            set
            {
                this.focusOnOpen = value;
            }
        }

        public bool AcceptAlt
        {
            get
            {
                return this.acceptAlt;
            }
            set
            {
                this.acceptAlt = value;
            }
        }

        public bool Resizable
        {
            get
            {
                if (this.resizable)
                    return !this.isChildPopupOpened;
                else
                    return false;
            }
            set
            {
                this.resizable = value;
            }
        }

        public new Size MinimumSize { get; set; }

        public new Size MaximumSize { get; set; }

        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 134217728;
                return createParams;
            }
        }

        public Popup(Control content)
        {
            var popup = this;
            if (content == null)
                throw new ArgumentNullException("content");
            this.content = content;
            this.showingAnimation = PopupAnimations.SystemDefault;
            this.hidingAnimation = PopupAnimations.None;
            this.animationDuration = 100;
            this.isChildPopupOpened = false;
            this.InitializeComponent();
            this.AutoSize = false;
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.host = new ToolStripControlHost(content);
            this.Padding = this.Margin = this.host.Padding = this.host.Margin = Padding.Empty;
            this.MinimumSize = content.MinimumSize;
            content.MinimumSize = content.Size;
            this.MaximumSize = content.MaximumSize;
            content.MaximumSize = content.Size;
            this.Size = content.Size;
            this.TabStop = content.TabStop = true;
            content.Location = Point.Empty;
            this.Items.Add((ToolStripItem)this.host);
            content.Disposed += (EventHandler)((sender, e) =>
            {
                content = (Control)null;
                popup.Dispose(true);
            });
            content.RegionChanged += (EventHandler)((sender, e) => this.UpdateRegion());
            content.Paint += (PaintEventHandler)((sender, e) => this.PaintSizeGrip(e));
            this.UpdateRegion();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                    this.components.Dispose();
                if (this.content != null)
                {
                    var control = this.content;
                    this.content = (Control)null;
                    control.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = (IContainer)new Container();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible && this.ShowingAnimation == PopupAnimations.None || !this.Visible && this.HidingAnimation == PopupAnimations.None)
                return;
            global::AmirCalendar.PopupControl.NativeMethods.AnimationFlags animationFlags = this.Visible ? global::AmirCalendar.PopupControl.NativeMethods.AnimationFlags.Roll : global::AmirCalendar.PopupControl.NativeMethods.AnimationFlags.Hide;
            var popupAnimations = this.Visible ? this.ShowingAnimation : this.HidingAnimation;
            if (popupAnimations == PopupAnimations.SystemDefault)
                popupAnimations = !SystemInformation.IsMenuAnimationEnabled ? PopupAnimations.None : (!SystemInformation.IsMenuFadeEnabled ? (PopupAnimations)(262144 | (this.Visible ? 4 : 8)) : PopupAnimations.Blend);
            if ((popupAnimations & (PopupAnimations.Center | PopupAnimations.Slide | PopupAnimations.Blend | PopupAnimations.Roll)) == PopupAnimations.None)
                return;
            if (this.resizableTop)
            {
                if ((popupAnimations & PopupAnimations.BottomToTop) != PopupAnimations.None)
                    popupAnimations = popupAnimations & ~PopupAnimations.BottomToTop | PopupAnimations.TopToBottom;
                else if ((popupAnimations & PopupAnimations.TopToBottom) != PopupAnimations.None)
                    popupAnimations = popupAnimations & ~PopupAnimations.TopToBottom | PopupAnimations.BottomToTop;
            }
            if (this.resizableLeft)
            {
                if ((popupAnimations & PopupAnimations.RightToLeft) != PopupAnimations.None)
                    popupAnimations = popupAnimations & ~PopupAnimations.RightToLeft | PopupAnimations.LeftToRight;
                else if ((popupAnimations & PopupAnimations.LeftToRight) != PopupAnimations.None)
                    popupAnimations = popupAnimations & ~PopupAnimations.LeftToRight | PopupAnimations.RightToLeft;
            }
            global::AmirCalendar.PopupControl.NativeMethods.AnimateWindow((Control)this, this.AnimationDuration, animationFlags | (global::AmirCalendar.PopupControl.NativeMethods.AnimationFlags)((PopupAnimations)1048575 & popupAnimations));
        }

        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (this.acceptAlt && (keyData & Keys.Alt) == Keys.Alt)
            {
                if ((keyData & Keys.F4) != Keys.F4)
                    return false;
                this.Close();
            }
            var flag = base.ProcessDialogKey(keyData);
            if (!flag && (keyData == Keys.Tab || keyData == (Keys.Tab | Keys.Shift)))
                this.Content.SelectNextControl((Control)null, (keyData & Keys.Shift) != Keys.Shift, true, true, true);
            return flag;
        }

        protected void UpdateRegion()
        {
            if (this.Region != null)
            {
                this.Region.Dispose();
                this.Region = (Region)null;
            }
            if (this.content.Region == null)
                return;
            this.Region = this.content.Region.Clone();
        }

        public void Show(Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control");
            this.Show(control, control.ClientRectangle);
        }

        public void Show(Control control, Rectangle area)
        {
            if (control == null)
                throw new ArgumentNullException("control");
            this.SetOwnerItem(control);
            this.resizableTop = this.resizableLeft = false;
            var point = control.PointToScreen(new Point(area.Left, area.Top + area.Height));
            var workingArea = Screen.FromControl(control).WorkingArea;
            if (point.X + this.Size.Width > workingArea.Left + workingArea.Width)
            {
                this.resizableLeft = true;
                point.X = workingArea.Left + workingArea.Width - this.Size.Width;
            }
            if (point.Y + this.Size.Height > workingArea.Top + workingArea.Height)
            {
                this.resizableTop = true;
                point.Y -= this.Size.Height + area.Height;
            }
            point = control.PointToClient(point);
            base.Show(control, point, ToolStripDropDownDirection.BelowRight);
        }

        private void SetOwnerItem(Control control)
        {
            if (control == null)
                return;
            if (control is Popup)
            {
                var popup = control as Popup;
                this.ownerPopup = popup;
                this.ownerPopup.childPopup = this;
                this.OwnerItem = popup.Items[0];
            }
            else
            {
                if (this.opener == null)
                    this.opener = control;
                if (control.Parent == null)
                    return;
                this.SetOwnerItem(control.Parent);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            this.content.MinimumSize = this.Size;
            this.content.MaximumSize = this.Size;
            this.content.Size = this.Size;
            this.content.Location = Point.Empty;
            base.OnSizeChanged(e);
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            if (this.content.IsDisposed || this.content.Disposing)
            {
                e.Cancel = true;
            }
            else
            {
                this.UpdateRegion();
                base.OnOpening(e);
            }
        }

        protected override void OnOpened(EventArgs e)
        {
            if (this.ownerPopup != null)
                this.ownerPopup.isChildPopupOpened = true;
            if (this.focusOnOpen)
                this.content.Focus();
            base.OnOpened(e);
        }

        protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
        {
            this.opener = (Control)null;
            if (this.ownerPopup != null)
                this.ownerPopup.isChildPopupOpened = false;
            base.OnClosed(e);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (this.InternalProcessResizing(ref m, false))
                return;
            base.WndProc(ref m);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool ProcessResizing(ref Message m)
        {
            return this.InternalProcessResizing(ref m, true);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool InternalProcessResizing(ref Message m, bool contentControl)
        {
            if (m.Msg == 134 && m.WParam != IntPtr.Zero && (this.childPopup != null && this.childPopup.Visible))
                this.childPopup.Hide();
            if (!this.Resizable)
                return false;
            if (m.Msg == 132)
                return this.OnNcHitTest(ref m, contentControl);
            if (m.Msg == 36)
                return this.OnGetMinMaxInfo(ref m);
            else
                return false;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool OnGetMinMaxInfo(ref Message m)
        {
            var minmaxinfo = (NativeMethods.MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(global::AmirCalendar.PopupControl.NativeMethods.MINMAXINFO));
            if (!this.MaximumSize.IsEmpty)
                minmaxinfo.maxTrackSize = this.MaximumSize;
            minmaxinfo.minTrackSize = this.MinimumSize;
            Marshal.StructureToPtr((object)minmaxinfo, m.LParam, false);
            return true;
        }

        private bool OnNcHitTest(ref Message m, bool contentControl)
        {
            var pt = this.PointToClient(new Point(NativeMethods.LOWORD(m.LParam), NativeMethods.HIWORD(m.LParam)));
            var gripBounds = new GripBounds(contentControl ? this.content.ClientRectangle : this.ClientRectangle);
            var num = new IntPtr(-1);
            if (this.resizableTop)
            {
                if (this.resizableLeft && gripBounds.TopLeft.Contains(pt))
                {
                    m.Result = contentControl ? num : (IntPtr)13;
                    return true;
                }
                else if (!this.resizableLeft && gripBounds.TopRight.Contains(pt))
                {
                    m.Result = contentControl ? num : (IntPtr)14;
                    return true;
                }
                else if (gripBounds.Top.Contains(pt))
                {
                    m.Result = contentControl ? num : (IntPtr)12;
                    return true;
                }
            }
            else if (this.resizableLeft && gripBounds.BottomLeft.Contains(pt))
            {
                m.Result = contentControl ? num : (IntPtr)16;
                return true;
            }
            else if (!this.resizableLeft && gripBounds.BottomRight.Contains(pt))
            {
                m.Result = contentControl ? num : (IntPtr)17;
                return true;
            }
            else if (gripBounds.Bottom.Contains(pt))
            {
                m.Result = contentControl ? num : (IntPtr)15;
                return true;
            }
            if (this.resizableLeft && gripBounds.Left.Contains(pt))
            {
                m.Result = contentControl ? num : (IntPtr)10;
                return true;
            }
            else
            {
                if (this.resizableLeft || !gripBounds.Right.Contains(pt))
                    return false;
                m.Result = contentControl ? num : (IntPtr)11;
                return true;
            }
        }

        public void PaintSizeGrip(PaintEventArgs e)
        {
            if (e == null || e.Graphics == null || !this.resizable)
                return;
            var clientSize = this.content.ClientSize;
            using (var bitmap = new Bitmap(16, 16))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    if (Application.RenderWithVisualStyles)
                    {
                        if (this.sizeGripRenderer == null)
                            this.sizeGripRenderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal);
                        this.sizeGripRenderer.DrawBackground((IDeviceContext)graphics, new Rectangle(0, 0, 16, 16));
                    }
                    else
                        ControlPaint.DrawSizeGrip(graphics, this.content.BackColor, 0, 0, 16, 16);
                }
                var gstate = e.Graphics.Save();
                e.Graphics.ResetTransform();
                if (this.resizableTop)
                {
                    if (this.resizableLeft)
                    {
                        e.Graphics.RotateTransform(180f);
                        e.Graphics.TranslateTransform((float)-clientSize.Width, (float)-clientSize.Height);
                    }
                    else
                    {
                        e.Graphics.ScaleTransform(1f, -1f);
                        e.Graphics.TranslateTransform(0.0f, (float)-clientSize.Height);
                    }
                }
                else if (this.resizableLeft)
                {
                    e.Graphics.ScaleTransform(-1f, 1f);
                    e.Graphics.TranslateTransform((float)-clientSize.Width, 0.0f);
                }
                e.Graphics.DrawImage((Image)bitmap, clientSize.Width - 16, clientSize.Height - 16 + 1, 16, 16);
                e.Graphics.Restore(gstate);
            }
        }
    }
}
