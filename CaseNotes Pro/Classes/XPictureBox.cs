using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace FirstResponse.CaseNotes.Classes
{
    public class XPictureBox : PictureBox
    {
        public GuiController Gui;
        private Boolean dragInProgress = false;
        public Boolean en = true;
        int MouseDownX = 0;
        int MouseDownY = 0;

        public XPictureBox(GuiController gui)
        {
            Gui = gui;
            MouseDown += MDown;
            MouseUp += MUp;
            MouseMove += MMove;
            Click += BClick;
            MouseEnter += MEnter;
            MouseLeave += MLeave;
            Image = Properties.Resources.placeholder;
            SizeMode = PictureBoxSizeMode.AutoSize;
        }

        private void BClick(object sender, EventArgs e)
        {
        }

        private void DBClick(object sender, EventArgs e)
        {
        }

        protected override bool ShowFocusCues
        {
            get { return false; }
        }

        private void MDown(Object sender, MouseEventArgs e)
        {
            if (!dragInProgress)
            {
                Focus();
                dragInProgress = true;
                MouseDownX = e.X;
                MouseDownY = e.Y;
            }
        }

        private bool Selected(Control sel)
        {
            sel = Gui.SelectedControl;
            return true;
        }

        private void MUp(Object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                dragInProgress = false;
            else if (e.Button == MouseButtons.Right)
            {
                Gui.Controller.Clear();
                foreach (Control cont in Gui.ControlList)
                {
                    Gui.Controller.Add(cont);
                }

                Gui.ControlList.Remove(sender as Control);
                Dispose(true);
            }
        }

        private void MMove(Object sender, MouseEventArgs e)
        {
            if (dragInProgress)
            {
                var temp = new Point { X = Location.X + (e.X - MouseDownX), Y = Location.Y + (e.Y - MouseDownY) };
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
