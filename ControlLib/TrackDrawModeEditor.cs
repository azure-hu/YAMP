using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Fusionbird.FusionToolkit.FusionTrackBar
{
    /// <summary>
    /// Adds some options under the OwnerDraw item in the designer
    /// </summary>
    public class TrackDrawModeEditor : UITypeEditor
    {
        #region Methods

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            TrackBarOwnerDrawParts parts = TrackBarOwnerDrawParts.None;
            if (!(value is TrackBarOwnerDrawParts) || (provider == null))
            {
                return value;
            }
            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service == null)
            {
                return value;
            }
            CheckedListBox control = new CheckedListBox();
            control.BorderStyle = BorderStyle.None;
            control.CheckOnClick = true;
            control.Items.Add("Ticks", (((FusionTrackBar)context.Instance).OwnerDrawParts & TrackBarOwnerDrawParts.Ticks) == TrackBarOwnerDrawParts.Ticks);
            control.Items.Add("Thumb", (((FusionTrackBar)context.Instance).OwnerDrawParts & TrackBarOwnerDrawParts.Thumb) == TrackBarOwnerDrawParts.Thumb);
            control.Items.Add("Channel", (((FusionTrackBar)context.Instance).OwnerDrawParts & TrackBarOwnerDrawParts.Channel) == TrackBarOwnerDrawParts.Channel);
            service.DropDownControl(control);
            IEnumerator enumerator = control.CheckedItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
                parts |= (TrackBarOwnerDrawParts)Enum.Parse(typeof(TrackBarOwnerDrawParts), objectValue.ToString());
            }
            control.Dispose();
            service.CloseDropDown();
            return parts;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        #endregion
    }

}
