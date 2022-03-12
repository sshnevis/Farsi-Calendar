using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AmirCalendar
{
    internal class FarsiCalendarDesigner : ControlDesigner
    {
        private DesignerActionListCollection _actionList;

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                return _actionList ?? (_actionList = new DesignerActionListCollection
                                                         {
                                                             new FarsiCalendarActionList(this.Component)
                                                         });
            }
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                return (SelectionRules.Visible | SelectionRules.Moveable | SelectionRules.RightSizeable |
                        SelectionRules.LeftSizeable);
            }
        }
    }

    internal class FarsiCalendarActionList : DesignerActionList
    {
        //private DesignerActionUIService _actionUiService;
        private readonly FarsiCalendar _farsiCalendar;

        public FarsiCalendarActionList(IComponent component) : base(component)
        {
            _farsiCalendar = (FarsiCalendar)component;
            //_actionUiService = (DesignerActionUIService)GetService(typeof(DesignerActionUIService));
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection
                            {
                                new DesignerActionHeaderItem("Properties"),
                                new DesignerActionPropertyItem("FarsiSelectedDate", "Selected Date"),
                                new DesignerActionPropertyItem("Mode", "Mode"),
                                new DesignerActionPropertyItem("Theme", "Theme"),
                                new DesignerActionPropertyItem("Format", "Format"),
                                new DesignerActionPropertyItem("RightToLeft", "RightToLeft")
                            };
            return items;
        }

        private void DynamicUpdate<T>(T entity, KeyValuePair<string, object> propertie)
        {
            var prop = TypeDescriptor.GetProperties(typeof (T)).Cast<PropertyDescriptor>().FirstOrDefault(p => String.CompareOrdinal(p.Name, propertie.Key) == 0);
            if (prop != null)
                prop.SetValue(entity, propertie.Value);

            if (String.CompareOrdinal(propertie.Key, "RightToLeft") == 0) return;
            prop = TypeDescriptor.GetProperties(typeof(FarsiCalendar)).Cast<PropertyDescriptor>().FirstOrDefault(p => String.CompareOrdinal(p.Name, "RightToLeft") == 0);
            if (prop != null)
                prop.SetValue(_farsiCalendar, _farsiCalendar.RightToLeft);
        }

        public RightToLeft RightToLeft
        {
            get { return _farsiCalendar.RightToLeft; }
            set
            {
                DynamicUpdate(_farsiCalendar, new KeyValuePair<string, object>("RightToLeft", value));
            }
        }

        public CalendarTheme Theme
        {
            get { return _farsiCalendar.Value.Theme; }
            set
            {
                DynamicUpdate(_farsiCalendar.Value, new KeyValuePair<string, object>("Theme", value));
            }
        }

        public DateFormat Format
        {
            get { return _farsiCalendar.Value.Format; }
            set
            {
                DynamicUpdate(_farsiCalendar.Value, new KeyValuePair<string, object>("Format", value));
            }
        }

        public ControlType Mode
        {
            get { return _farsiCalendar.Value.Mode; }
            set
            {
                DynamicUpdate(_farsiCalendar.Value, new KeyValuePair<string, object>("Mode", value));
            }
        }

        [Editor(typeof(TypeEditor), typeof(UITypeEditor)), RefreshProperties(RefreshProperties.All)]
        public string FarsiSelectedDate
        {
            get { return _farsiCalendar.Value.FarsiSelectedDate; }
            set
            {
                DynamicUpdate(_farsiCalendar.Value, new KeyValuePair<string, object>("FarsiSelectedDate", value));
            }
        }
    }

    internal class TypeEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (editorService != null)
            {
                var control = new FarsiCalendar
                                  {
                                      Width = 280, Height = 158, 
                                      Value =
                                          {
                                              FarsiSelectedDate = (string) value,
                                              Mode = ControlType.MonthCalendar,
                                              Theme = CalendarTheme.WhiteSmoke
                                          },
                                  };
                control.Value.DateChanged += (arg1, arg2) => editorService.CloseDropDown();
                editorService.DropDownControl(control);
                return control.Value.FarsiSelectedDate;
            }
            return base.EditValue(context, provider, RuntimeHelpers.GetObjectValue(value));
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
    }
}
