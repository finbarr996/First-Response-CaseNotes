using System;
using System.Drawing;
using System.Windows.Forms;

namespace FirstResponse.CaseNotes.Classes
{
    public class XCheckBox : CheckBox
    {
        public GuiController Gui;
        private Boolean dragInProgress = false;
        public Boolean en = true;
        int MouseDownX = 0;
        int MouseDownY = 0;

        public XCheckBox(GuiController gui)
        {
            Gui = gui;
            MouseDown += MDown;
            MouseUp += MUp;
            MouseMove += MMove;
            MouseEnter += MEnter;
            MouseLeave += MLeave;
        }

        protected override bool ShowFocusCues
        {
            get { return false; }
        }

        private void MDown(Object sender, MouseEventArgs e)
        {
            if (!dragInProgress)
            {
                dragInProgress = true;
                MouseDownX = e.X;
                MouseDownY = e.Y;
            }
        }

        private void MUp(Object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                dragInProgress = false;

            else if (e.Button == MouseButtons.Right)
            {
                Gui.Controller.Clear();
                foreach (Control c in Gui.ControlList)
                {
                    Gui.Controller.Add(c);
                }

                Gui.ControlList.Remove(sender as Control);
                Dispose(true);
            }
        }

        private bool Selected(Control sel)
        {
            sel = Gui.SelectedControl;
            return true;
        }

        private void MMove(Object sender, MouseEventArgs e)
        {
            if (dragInProgress)
            {
                var temp = new Point {X = Location.X + (e.X - MouseDownX), Y = Location.Y + (e.Y - MouseDownY)};
                Location = temp;
            }
        }

        private void MEnter(Object sender, EventArgs e)
        {
        }

        private void MLeave(Object sender, EventArgs e)
        {
        }
    }
}
