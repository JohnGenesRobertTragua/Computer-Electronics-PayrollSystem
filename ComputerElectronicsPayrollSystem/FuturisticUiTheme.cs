using System.Drawing;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    /// <summary>
    /// Restrained visual consistency helper. It deliberately does not alter
    /// control positions, data sources, columns, sorting modes, or event handlers.
    /// </summary>
    internal static class FuturisticUiTheme
    {
        private static readonly Color Navy = Color.FromArgb(20, 34, 56);
        private static readonly Color Ink = Color.FromArgb(31, 45, 66);
        private static readonly Color Blue = Color.FromArgb(43, 108, 232);
        private static readonly Color Page = Color.FromArgb(245, 248, 253);
        private static readonly Color Line = Color.FromArgb(218, 228, 241);

        // Backward-compatible entry point used by existing forms.
        // This keeps calls such as FuturisticUiTheme.Apply(this) compiling
        // without changing the form layout or control behavior.
        public static void Apply(Control root)
        {
            if (root == null || root.IsDisposed) return;
            ApplyRecursive(root, true);
        }

        public static void ApplyGrids(Control root)
        {
            if (root == null || root.IsDisposed) return;
            ApplyRecursive(root, false);
        }

        private static void ApplyRecursive(Control parent, bool polishControls)
        {
            foreach (Control control in parent.Controls)
            {
                DataGridView grid = control as DataGridView;
                if (grid != null)
                {
                    ApplyGrid(grid);
                }
                else if (polishControls)
                {
                    ApplyControl(control);
                }

                if (control.HasChildren) ApplyRecursive(control, polishControls);
            }
        }

        private static void ApplyControl(Control control)
        {
            // Keep all existing text, icons, colors, bounds and event handlers.
            // This is intentionally a light touch: no blanket flat styling and
            // no recoloring of labels or panels that could reduce contrast.
            Button button = control as Button;
            if (button != null)
            {
                button.Cursor = Cursors.Hand;
                if (button.FlatStyle == FlatStyle.Flat)
                {
                    // Do not introduce the heavy outlines that made earlier
                    // versions look cluttered. Existing accent colors are kept.
                    button.FlatAppearance.BorderSize = 0;
                    button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(button.BackColor, 0.08f);
                    button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(button.BackColor, 0.08f);
                }
                return;
            }

            TextBoxBase textBox = control as TextBoxBase;
            if (textBox != null)
            {
                textBox.Font = new Font("Segoe UI", Math.Max(9.5f, textBox.Font.Size), textBox.Font.Style);
                return;
            }

            ComboBox combo = control as ComboBox;
            if (combo != null)
            {
                // Standard preserves the native visible border and dropdown arrow.
                combo.FlatStyle = FlatStyle.Standard;
                combo.Font = new Font("Segoe UI", Math.Max(9.5f, combo.Font.Size), combo.Font.Style);
                return;
            }

            DateTimePicker datePicker = control as DateTimePicker;
            if (datePicker != null)
            {
                datePicker.Font = new Font("Segoe UI", Math.Max(9.5f, datePicker.Font.Size), datePicker.Font.Style);
            }
        }

        private static void ApplyGrid(DataGridView grid)
        {
            // Keep the existing sizing, columns, sorting, data, and selection behavior.
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Navy;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Navy;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.DefaultCellStyle.ForeColor = Ink;
            grid.DefaultCellStyle.SelectionBackColor = Blue;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Page;
            grid.GridColor = Line;
        }
    }
}
