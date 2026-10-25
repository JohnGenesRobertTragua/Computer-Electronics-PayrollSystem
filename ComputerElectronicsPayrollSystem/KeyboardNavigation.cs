using System;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    public static class KeyboardNavigation
    {
        // ==========================================================
        // ENABLE KEYBOARD NAVIGATION
        // ==========================================================

        public static void Enable(Form form)
        {
            form.KeyPreview = true;
            form.KeyDown += Form_KeyDown;
        }

        // ==========================================================
        // KEYBOARD EVENT
        // ==========================================================

        private static void Form_KeyDown(object? sender, KeyEventArgs e)
        {
            if (sender is not Form form)
                return;

            // Find the actual control that currently has focus.
            Control? current = FindFocusedControl(form);

            if (current == null)
                return;

            // ======================================================
            // ENTER = NEXT CONTROL
            // ======================================================

            if (e.KeyCode == Keys.Enter)
            {
                // Allow Enter to work normally in multiline TextBoxes.
                if (current is TextBox textBox && textBox.Multiline)
                    return;

                MoveNext(form, current);

                e.Handled = true;
                e.SuppressKeyPress = true;

                return;
            }

            // ======================================================
            // DOWN = NEXT CONTROL
            // ======================================================

            if (e.KeyCode == Keys.Down)
            {
                // ComboBox uses Up/Down to change its selection.
                if (current is ComboBox)
                    return;

                // DataGridView uses Up/Down to move between rows.
                if (current is DataGridView)
                    return;

                MoveNext(form, current);

                e.Handled = true;
                e.SuppressKeyPress = true;

                return;
            }

            // ======================================================
            // UP = PREVIOUS CONTROL
            // ======================================================

            if (e.KeyCode == Keys.Up)
            {
                // ComboBox uses Up/Down to change its selection.
                if (current is ComboBox)
                    return;

                // DataGridView uses Up/Down to move between rows.
                if (current is DataGridView)
                    return;

                MovePrevious(form, current);

                e.Handled = true;
                e.SuppressKeyPress = true;

                return;
            }

            // ======================================================
            // LEFT / RIGHT
            // ======================================================
            //
            // Intentionally not handled.
            //
            // TextBoxes need these keys for cursor movement.
            // ComboBoxes and other controls also use them normally.
        }

        // ==========================================================
        // FIND THE ACTUAL CONTROL WITH FOCUS
        // ==========================================================

        private static Control? FindFocusedControl(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control.Focused)
                {
                    return control;
                }

                if (control.ContainsFocus)
                {
                    Control? focusedChild =
                        FindFocusedControl(control);

                    if (focusedChild != null)
                        return focusedChild;
                }
            }

            return null;
        }

        // ==========================================================
        // MOVE TO NEXT CONTROL
        // ==========================================================

        private static void MoveNext(Form form, Control current)
        {
            form.SelectNextControl(
                current,
                true,
                true,
                true,
                true
            );
        }

        // ==========================================================
        // MOVE TO PREVIOUS CONTROL
        // ==========================================================

        private static void MovePrevious(Form form, Control current)
        {
            form.SelectNextControl(
                current,
                false,
                true,
                true,
                true
            );
        }
    }
}