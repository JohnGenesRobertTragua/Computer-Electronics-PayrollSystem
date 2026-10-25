using System;
using System.Windows.Forms;

namespace ComputerElectronicsPayrollSystem
{
    public class ReadOnlyDateTimePicker : DateTimePicker
    {
        public ReadOnlyDateTimePicker()
        {
            ShowCheckBox = true;
            TabStop = false;
        }

        // ============================================================
        // SET TIME FROM PROGRAM
        // ============================================================

        public void SetTime(DateTime value)
        {
            bool currentChecked = base.Checked;

            base.Value = value;

            // Changing Value can cause the native DateTimePicker
            // to become checked, so restore the previous state.
            base.Checked = currentChecked;

            Invalidate();
            Update();
            Refresh();
        }

        // ============================================================
        // SET CHECKBOX FROM PROGRAM
        // ============================================================

        public void SetCheckedState(bool value)
        {
            base.Checked = value;

            Invalidate();
            Update();
            Refresh();
        }

        // ============================================================
        // PREVENT DIRECT USER MOUSE EDITING
        // ============================================================

        protected override void WndProc(ref Message m)
        {
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_LBUTTONUP = 0x0202;
            const int WM_LBUTTONDBLCLK = 0x0203;
            const int WM_RBUTTONDOWN = 0x0204;
            const int WM_RBUTTONUP = 0x0205;
            const int WM_MOUSEWHEEL = 0x020A;

            // Block direct mouse editing.
            // The buttons in AttendanceControl control the state.
            if (m.Msg == WM_LBUTTONDOWN ||
                m.Msg == WM_LBUTTONUP ||
                m.Msg == WM_LBUTTONDBLCLK ||
                m.Msg == WM_RBUTTONDOWN ||
                m.Msg == WM_RBUTTONUP ||
                m.Msg == WM_MOUSEWHEEL)
            {
                return;
            }

            base.WndProc(ref m);
        }

        // ============================================================
        // PREVENT KEYBOARD EDITING
        // ============================================================

        protected override bool ProcessCmdKey(
            ref Message msg,
            Keys keyData)
        {
            Keys key =
                keyData & Keys.KeyCode;

            if (key == Keys.Space ||
                key == Keys.Up ||
                key == Keys.Down ||
                key == Keys.Left ||
                key == Keys.Right ||
                key == Keys.Home ||
                key == Keys.End ||
                key == Keys.PageUp ||
                key == Keys.PageDown)
            {
                return true;
            }

            return base.ProcessCmdKey(
                ref msg,
                keyData);
        }
    }
}