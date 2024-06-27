using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace FirstResponse.CaseNotes
{
    internal class TextRuler : UserControl
    {
        #region Variables
        RectangleF _me = new RectangleF();
        RectangleF _drawZone = new RectangleF();
        RectangleF _workArea = new RectangleF();
        List<RectangleF> items = new List<RectangleF>();
        List<RectangleF> tabs = new List<RectangleF>();
        Pen p = new Pen(Color.Transparent);        
        private int lMargin = 0, rMargin = 20, llIndent = 0, luIndent = 0, rIndent = 20;        
        Color _strokeColor = Color.Black;
        Color _baseColor = Color.White;
        int pos = -1;
        bool mCaptured = false;
        bool noMargins = false;
        int capObject = -1, capTab = -1;
        bool _tabsEnabled = false;
        readonly float dotsPermm;

        internal enum ControlItems
        {
            LeftIndent,
            LeftHangingIndent,
            RightIndent,
            LeftMargin,
            RightMargin
        }

        #region Events declarations
        public delegate void IndentChangedEventHandler(int newValue);
        public delegate void MultiIndentChangedEventHandler(int leftIndent, int hangingIndent);
        public delegate void MarginChangedEventHandler(int newValue);
        public delegate void TabChangedEventHandler(TabEventArgs args);

        public event IndentChangedEventHandler LeftHangingIndentChanging;
        public event IndentChangedEventHandler LeftIndentChanging;
        public event IndentChangedEventHandler RightIndentChanging;
        public event MultiIndentChangedEventHandler BothLeftIndentsChanged;

        public event MarginChangedEventHandler LeftMarginChanging;
        public event MarginChangedEventHandler RightMarginChanging;

        public event TabChangedEventHandler TabAdded;
        public event TabChangedEventHandler TabRemoved;
        public event TabChangedEventHandler TabChanged;

        #endregion

        #endregion

        #region Constructor
        private void InitializeComponent()
        {
            SuspendLayout();
            Name = "TextRuler";
            Size = new Size(100, 20);
            ResumeLayout(false);
        }

        public TextRuler()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Font = new Font("Arial", 7.25f);

            tabs.Clear();

            //margins and indents               
            items.Add(new RectangleF());     // items[0] - left margin
            items.Add(new RectangleF());     // items[1] - right margin
            items.Add(new RectangleF());     // items[2] - left indent upper mark
            items.Add(new RectangleF());     // items[3] - left indent lower mark (picture region)
            items.Add(new RectangleF());     // items[4] - right indent mark
            items.Add(new RectangleF());     // items[5] - left indent mark (self-moving region)
            items.Add(new RectangleF());     // items[6] - left indent mark (all-moving region)

            using (Graphics g = Graphics.FromHwnd(Handle))
            {
                dotsPermm = g.DpiX / 25.4f;
            }
        }
        #endregion

        #region Painting
        private void DrawBackGround(Graphics g)
        {            
            p.Color = Color.Transparent;
            g.FillRectangle(p.Brush, _me);
            
            p.Color = _baseColor;
            g.FillRectangle(p.Brush, _drawZone);            
        }

        private void DrawMargins(Graphics g)
        {            
            items[0] = new RectangleF(0f, 3f, lMargin * dotsPermm, 14f);
            items[1] = new RectangleF(_drawZone.Width - ((float)rMargin * dotsPermm) + 1f, 3f, rMargin * dotsPermm + 5f, 14f);
            p.Color = Color.DarkGray;
            g.FillRectangle(p.Brush, items[0]);
            g.FillRectangle(p.Brush, items[1]);

            g.PixelOffsetMode = PixelOffsetMode.None;
            p.Color = _strokeColor;
            g.DrawRectangle(p, 0, 3, _me.Width - 1, 14);            
        }

        private void DrawTextAndMarks(Graphics g)
        {            
            int points = (int)(_drawZone.Width / dotsPermm) / 10;
            float range = 5 * dotsPermm;
            p.Color = Color.Black;
            SizeF sz;
            for (var i = 0; i <= points * 2 + 1; i++)
            {
                if (i % 2 == 0 && i != 0)
                {
                    sz = g.MeasureString((Convert.ToInt32(i / 2)).ToString(), Font);
                    g.DrawString((Convert.ToInt32(i / 2)).ToString(), Font, p.Brush, new PointF((float)(i * range - (float)(sz.Width / 2)), (float)(_me.Height / 2) - (float)(sz.Height / 2)));
                }
                else
                {
                    g.DrawLine(p, (float)(i * range), 7f, (float)(i * range), 12f);
                }
            }
            g.PixelOffsetMode = PixelOffsetMode.Half;
        }

        private void DrawIndents(Graphics g)
        {
            items[2] = new RectangleF((float)luIndent * dotsPermm - 4.5f, 0f, 9f, 8f);
            items[3] = new RectangleF((float)llIndent * dotsPermm - 4.5f, 8.2f, 9f, 11.8f);
            items[4] = new RectangleF((float)(_drawZone.Width - ((float)rIndent * dotsPermm - 4.5f) - 7f), 11f, 9f, 8f);
            
            items[5] = new RectangleF((float)llIndent * dotsPermm - 4.5f, 8.2f, 9f, 5.9f);
            items[6] = new RectangleF((float)llIndent * dotsPermm - 4.5f, 14.1f, 9f, 5.9f);

            g.DrawImage(Properties.Resources.l_indet_pos_upper, items[2]);
            g.DrawImage(Properties.Resources.l_indent_pos_lower, items[3]);
            g.DrawImage(Properties.Resources.r_indent_pos, items[4]);
        }

        private void DrawTabs(Graphics g)
        {
            if (_tabsEnabled == false)
                return;

            if (tabs.Count == 0)
                return;

            for (var i = 0; i <= tabs.Count - 1; i++)
            {
                g.DrawImage(Properties.Resources.tab_pos, tabs[i]);
            }            
        }
        #endregion

        #region Actions
        private void AddTab(float pos)
        {
            var rect = new RectangleF(pos, 10f, 8f, 8f);
            tabs.Add(rect);
            if (TabAdded != null)
                TabAdded.Invoke(CreateTabArgs(pos));
        }

        /// <summary>
        /// Returns List which contains positions of the tabs converted to millimeters.
        /// </summary>
        public List<int> TabPositions
        {
            get
            {
                var lst = new List<int>();
                for (var i = 0; i <= tabs.Count - 1; i++)
                {
                    lst.Add((int)(tabs[i].X / dotsPermm));
                }
                lst.Sort();
                return lst;
            }
        }

        /// <summary>
        /// Returns List which contains positions of the tabs in pixels.
        /// </summary>
        public List<int> TabPositionsInPixels
        {
            get
            {
                var lst = new List<int>();
                for (var i = 0; i <= tabs.Count - 1; i++)
                {                    
                    lst.Add((int)(tabs[i].X));
                }
                lst.Sort();
                return lst;
            }
        }

        /// <summary>
        /// Sets positions for tabs. It uses positions represented in pixels.
        /// </summary>
        /// <param name="positions"></param>
        public void SetTabPositionsInPixels(int[] positions)
        {
            if (positions == null)
                tabs.Clear();
            else
            {
                tabs.Clear();             
                for (var i = 0; i <= positions.Length - 1; i++)
                {                    
                    var rect = new RectangleF(Convert.ToSingle(positions[i]), 10f, 8f, 8f);
                    tabs.Add(rect);                    
                }                
            }
            Refresh();
        }

        /// <summary>
        /// Sets positions for tabs. It uses positions represented in millemeters.
        /// </summary>
        /// <param name="positions"></param>
        public void SetTabPositionsInMillimeters(int[] positions)
        {
            if (positions == null)
                tabs.Clear();
            else
            {
                tabs.Clear();
                for (var i = 0; i <= positions.Length - 1; i++)
                {
                    if (positions[i] != 0)
                    {
                        var rect = new RectangleF(positions[i] * dotsPermm, 10f, 8f, 8f);
                        tabs.Add(rect);
                    }
                }
                Refresh();
            }
        }
        
        internal int GetValueInPixels(ControlItems item)
        {
            switch (item)
            {
                case ControlItems.LeftIndent:
                    return (int)(luIndent * dotsPermm);
                    
                case ControlItems.LeftHangingIndent:
                    return (int)(llIndent * dotsPermm);
                    
                case ControlItems.RightIndent:
                    return (int)(rIndent * dotsPermm);
                    
                case ControlItems.LeftMargin:
                    return (int)(lMargin * dotsPermm);
                    
                case ControlItems.RightMargin:
                    return (int)(rMargin * dotsPermm);
                    
                default:
                    return 0;
                    
            }
        }

        public float DotsPerMillimeter
        {
            get { return dotsPermm; }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets color for the border
        /// </summary>
        [DefaultValue(typeof(Color), "Black")]
        [Description("Color of the border drawn on the bounds of control.")]
        public Color BorderColor
        {
            get { return _strokeColor; }
            set { _strokeColor = value; Refresh(); }
        }

        /// <summary>
        /// Gets or sets color for the background
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        [Description("Base color for the control.")]        
        public Color BaseColor
        {
            get
            {
                return _baseColor;
            }
            set
            {
                _baseColor = value;
            }
        }

        /// <summary>
        /// Enables or disables usage of the margins. If disabled, margins values are set to 1.
        /// </summary>
        [Category("Margins")]
        [Description("If true Margins are disabled, otherwise, false.")]
        [DefaultValue(false)]
        public bool NoMargins
        {
            get { return noMargins; }
            set 
            { 
                noMargins = value;
                if (value == true)
                {
                    lMargin = 1;
                    rMargin = 1;
                }
                Refresh(); 
            }
        }

        /// <summary>
        /// Specifies left margin
        /// </summary>
        [Category("Margins")]
        [Description("Gets or sets left margin. This value is in millimeters.")]
        [DefaultValue(20)]
        public int LeftMargin
        {
            get { return lMargin; }
            set 
            {
                if (noMargins != true)
                {
                    lMargin = value;
                }
                Refresh(); 
            }
        }

        /// <summary>
        /// Specifies right margin
        /// </summary>
        [Category("Margins")]
        [Description("Gets or sets right margin. This value is in millimeters.")]
        [DefaultValue(15)]
        public int RightMargin
        {
            get { return rMargin; }
            set 
            {
                if (noMargins != true)
                {
                    rMargin = value;
                }
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets indentation of the first line of the paragraph
        /// </summary>
        [Category("Indents")]
        [Description("Gets or sets left hanging indent. This value is in millimeters.")]
        [DefaultValue(20)]
        public int LeftHangingIndent
        {
            get { return llIndent - 1; }
            set 
            {
                llIndent = value + 1;
                Refresh(); 
            }
        }

        /// <summary>
        /// Gets or sets indentation from the left of the base text of the paragraph
        /// </summary>
        [Category("Indents")]
        [Description("Gets or sets left indent. This value is in millimeters.")]
        [DefaultValue(20)]
        public int LeftIndent
        {
            get { return luIndent - 1; }
            set 
            {
                luIndent = value + 1;
                Refresh(); 
            }
        }

        /// <summary>
        /// Gets or sets right indentation of the paragraph
        /// </summary>
        [Category("Indents")]
        [Description("Gets or sets right indent. This value is in millimeters.")]
        [DefaultValue(15)]
        public int RightIndent
        {
            get { return rIndent - 1; }
            set 
            {
                rIndent = value + 1; 
                Refresh();
            }
        }

        [Category("Tabulation")]
        [Description("True to display tab stops, otherwise, False")]
        [DefaultValue(false)]
        public bool TabsEnabled
        {
            get { return _tabsEnabled; }
            set { _tabsEnabled = value; Refresh(); }
        }
        #endregion

        #region Overriders
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            
            _me = new RectangleF(0f, 0f, (float)Width, (float)Height);
            _drawZone = new RectangleF(1f, 3f, _me.Width - 2f, 14f);
            _workArea = new RectangleF((float)lMargin * dotsPermm, 3f, _drawZone.Width - ((float)rMargin * dotsPermm) - _drawZone.X * 2, 14f);            

            DrawBackGround(e.Graphics);
            DrawMargins(e.Graphics);
            DrawTextAndMarks(e.Graphics);
            DrawIndents(e.Graphics);
            DrawTabs(e.Graphics);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 20;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mCaptured = false;

            if (e.Button != MouseButtons.Left)
                return;

            for (var i = 0; i <= 6; i++)
            {
                if (items[i].Contains(e.Location) && i != 3)
                {
                    if (noMargins && (i == 0 || i == 1))
                        break;

                    capObject = i;
                    mCaptured = true;
                    break;
                }
            }

            if (mCaptured)
                return;

            if (tabs.Count == 0)
                return;

            if (_tabsEnabled == false)
                return;

            for (var i = 0; i <= tabs.Count - 1; i++)
            {
                if (tabs[i].Contains(e.Location))
                {
                    capTab = i;
                    pos = (int)(tabs[i].X / dotsPermm);
                    mCaptured = true;
                    break;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button != MouseButtons.Left)
                return;

            if (_workArea.Contains(e.Location) == false)
            {
                if (mCaptured && capTab != -1 && _tabsEnabled)
                {
                    try
                    {
                        var pos = tabs[capTab].X * dotsPermm;
                        tabs.RemoveAt(capTab);
                        if (TabRemoved != null)
                            TabRemoved.Invoke(CreateTabArgs(pos));
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else if (_workArea.Contains(e.Location))
            {
                if (mCaptured != true && _tabsEnabled)
                {
                    AddTab((float)e.Location.X);
                }
                else if (mCaptured && capTab != -1)
                {
                    if (TabChanged != null && _tabsEnabled)
                        TabChanged.Invoke(CreateTabArgs(e.Location.X));
                }
            }

            capTab = -1;
            mCaptured = false;
            capObject = -1;
            Refresh();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (mCaptured && capObject != -1)
            {
                switch (capObject)
                {
                    case 0:
                        if (noMargins)
                            return;
                        if (e.Location.X <= (int)(_me.Width - rMargin * dotsPermm - 35f))
                        {
                            lMargin = (int)(e.Location.X / dotsPermm);
                            if (lMargin < 1)
                                lMargin = 1;
                            if (LeftMarginChanging != null)
                                LeftMarginChanging.Invoke(lMargin);
                            Refresh();
                        }
                        break;

                    case 1:
                        if (noMargins)
                            return;
                        if (e.Location.X >= (int)(lMargin * dotsPermm + 35f))
                        {
                            rMargin = (int)((_drawZone.Width / dotsPermm) - (int)(e.Location.X / dotsPermm));
                            if (rMargin < 1)
                                rMargin = 1;
                            if (RightMarginChanging != null)
                                RightMarginChanging.Invoke(rMargin);
                            Refresh();
                        }
                        break;

                    case 2:
                        if (e.Location.X <= (int)(_me.Width - rIndent * dotsPermm - 35f))
                        {
                            luIndent = (int)(e.Location.X / dotsPermm);
                            if (luIndent < 1)
                                luIndent = 1;
                            if (LeftIndentChanging != null)
                                LeftIndentChanging.Invoke(luIndent - 1);
                            Refresh();
                        }
                        break;
                    

                    case 4:
                        if (e.Location.X >= (int)(Math.Max(llIndent, luIndent) * dotsPermm + 35f))
                        {
                            rIndent = (int)((_me.Width / dotsPermm) - (int)(e.Location.X / dotsPermm));
                            if (rIndent < 1)
                                rIndent = 1;
                            if (RightIndentChanging != null)
                                RightIndentChanging.Invoke(rIndent - 1);
                            Refresh();
                        }
                        break;

                    case 5:
                        if (e.Location.X <= (int)(_drawZone.Width - rIndent * dotsPermm - 35f))
                        {
                            llIndent = (int)(e.Location.X / dotsPermm);
                            if (llIndent < 1)
                                llIndent = 1;
                            if (LeftHangingIndentChanging != null)
                                LeftHangingIndentChanging.Invoke(llIndent - 1);
                            Refresh();
                        }
                        break;

                    case 6:
                        if (e.Location.X <= (int)(_drawZone.Width - rIndent * dotsPermm - 35f))
                        {                            
                            luIndent = luIndent + (int)(e.Location.X / dotsPermm) - llIndent;
                            llIndent = (int)(e.Location.X / dotsPermm);
                            if (llIndent < 1)
                                llIndent = 1;
                            if (luIndent < 1)
                                luIndent = 1;
                            if (BothLeftIndentsChanged != null)
                                BothLeftIndentsChanged.Invoke(luIndent - 1, llIndent - 1);
                            Refresh();
                        }
                        break;

                }
            }
            else if (mCaptured && capTab != -1)
            {
                if (_workArea.Contains(e.Location))
                {
                    tabs[capTab] = new RectangleF((float)e.Location.X, tabs[capTab].Y, tabs[capTab].Width, tabs[capTab].Height);
                    Refresh();
                }
            }
            else
            {
                for (var i = 0; i <= 4; i++)
                {
                    if (items[i].Contains(e.Location) == true)
                    {
                        switch (i)
                        {
                            case 0:
                                if (noMargins) return;
                                Cursor = Cursors.SizeWE;
                                break;

                            case 1:
                                if (noMargins) return;
                                Cursor = Cursors.SizeWE;
                                break;
                        }
                        break;
                    }
                    Cursor = Cursors.Default;
                }
            }
        }
        #endregion

        #region Events classes
        internal class TabEventArgs : EventArgs
        {
            private int _posN = -1;
            private int _posO = -1;

            internal int NewPosition
            {
                get { return _posN; }
                set { _posN = value; }
            }

            internal int OldPosition
            {
                get { return _posO; }
                set { _posO = value; }
            }
        }

        private TabEventArgs CreateTabArgs(float newPos)
        {
            var tae = new TabEventArgs();
            tae.NewPosition = (int)(newPos / dotsPermm);            
            tae.OldPosition = pos;
            return tae;
        }

        #endregion
    }
}
