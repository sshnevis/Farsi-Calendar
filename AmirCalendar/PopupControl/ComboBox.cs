using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace AmirCalendar.PopupControl
{
    [ToolboxItem(false), DesignTimeVisible(false)]
    internal class ComboBox : System.Windows.Forms.ComboBox
    {
        private static Type _modalMenuFilter;
        private static MethodInfo _suspendMenuMode;
        private static MethodInfo _resumeMenuMode;
        private IContainer components;

        static Type modalMenuFilter
        {
            get
            {
                if (_modalMenuFilter == null)
                    _modalMenuFilter = Type.GetType("System.Windows.Forms.ToolStripManager+ModalMenuFilter");
                return _modalMenuFilter ??
                       (_modalMenuFilter = new List<Type>(typeof(ToolStripManager).Assembly.GetTypes()).Find(type => type.FullName == "System.Windows.Forms.ToolStripManager+ModalMenuFilter"));
            }
        }

        static MethodInfo suspendMenuMode
        {
            get
            {
                if (_suspendMenuMode == null)
                {
                    var modalMenuFilter = ComboBox.modalMenuFilter;
                    if (modalMenuFilter != null)
                        _suspendMenuMode = modalMenuFilter.GetMethod("SuspendMenuMode", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                }
                return _suspendMenuMode;
            }
        }

        static MethodInfo resumeMenuMode
        {
            get
            {
                if (_resumeMenuMode == null)
                {
                    var modalMenuFilter = ComboBox.modalMenuFilter;
                    if (modalMenuFilter != null)
                        _resumeMenuMode = modalMenuFilter.GetMethod("ResumeMenuMode", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                }
                return _resumeMenuMode;
            }
        }

        public ComboBox()
        {
            InitializeComponent();
        }

        private static void SuspendMenuMode()
        {
            var suspendMenuMode = ComboBox.suspendMenuMode;
            if (suspendMenuMode == null)
                return;
            suspendMenuMode.Invoke(null, null);
        }

        private static void ResumeMenuMode()
        {
            var resumeMenuMode = ComboBox.resumeMenuMode;
            if (resumeMenuMode == null)
                return;
            resumeMenuMode.Invoke(null, null);
        }

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
            SuspendMenuMode();
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            ResumeMenuMode();
            base.OnDropDownClosed(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            ResumeLayout(false);
        }
    }
}
