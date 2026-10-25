using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    /// <summary>
    /// Adds restrained, consistent interaction polish to existing module controls
    /// without replacing their colors, event handlers, layout, or business logic.
    /// </summary>
    internal static class PayrollVisualPolish
    {
        public static void Apply(Control root)
        {
            if (root == null || root.IsDisposed) return;
            ApplyRecursive(root);
        }

        private static void ApplyRecursive(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                Button button = control as Button;
                if (button != null)
                {
                    button.Cursor = Cursors.Hand;
                    button.UseVisualStyleBackColor = false;
                    button.FlatStyle = FlatStyle.Flat;
                    if (button.FlatAppearance.BorderSize == 0)
                        button.FlatAppearance.BorderSize = 1;
                    if (button.FlatAppearance.BorderColor == Color.Empty ||
                        button.FlatAppearance.BorderColor == SystemColors.Control)
                        button.FlatAppearance.BorderColor = Color.FromArgb(218, 226, 238);
                    if (button.FlatAppearance.MouseOverBackColor == Color.Empty ||
                        button.FlatAppearance.MouseOverBackColor == SystemColors.Control)
                        button.FlatAppearance.MouseOverBackColor = Lighten(button.BackColor);
                    if (button.FlatAppearance.MouseDownBackColor == Color.Empty ||
                        button.FlatAppearance.MouseDownBackColor == SystemColors.Control)
                        button.FlatAppearance.MouseDownBackColor = Darken(button.BackColor);
                    button.Padding = new Padding(
                        Math.Max(button.Padding.Left, 5),
                        button.Padding.Top,
                        Math.Max(button.Padding.Right, 5),
                        button.Padding.Bottom);
                }

                // Preserve existing grid styling and all input colors. The
                // cursor and font polish improves usability without changing data.
                TextBoxBase textBox = control as TextBoxBase;
                if (textBox != null && textBox.Font.Size < 9.5F)
                    textBox.Font = new Font("Segoe UI", 9.5F, textBox.Font.Style);

                ApplyRecursive(control);
            }
        }

        private static Color Lighten(Color color)
        {
            if (color.GetBrightness() > 0.75F)
                return ControlPaint.Light(color, 0.08F);
            return ControlPaint.Light(color, 0.18F);
        }

        private static Color Darken(Color color)
        {
            return ControlPaint.Dark(color, 0.08F);
        }
    }
}
