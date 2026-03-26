using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ExpensesManager.CustomControls;

public class CustomWatermarkedDatePicker : DatePicker
{
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        DatePickerTextBox? box = base.GetTemplateChild("PART_TextBox") as DatePickerTextBox;
        if (box == null) return;
        
        box.ApplyTemplate();

        if (box.Template == null) return;

        ContentControl? watermark = box.Template.FindName("PART_Watermark", box) as ContentControl;
        if (watermark == null) return;
        
        watermark?.Content = "Pick a date..";
    }
}