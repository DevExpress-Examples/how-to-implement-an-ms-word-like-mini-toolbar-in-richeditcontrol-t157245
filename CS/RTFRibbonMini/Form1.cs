﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Office.Utils;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraRichEdit.API.Native;

namespace RTFRibbonMini {
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm {
        public Form1() {
            InitializeComponent();
            richEditControl1.MouseUp += richEditControl1_MouseUp;
        }

        void richEditControl1_MouseUp(object sender, MouseEventArgs e) {
            if(richEditControl1.Document.Selection.Length != 0) {
                DocumentPosition documentPosition = richEditControl1.Document.CaretPosition;
                Rectangle documentRectangle = richEditControl1.GetBoundsFromPosition(documentPosition);
                Point documentPoint = new Point(documentRectangle.Left, documentRectangle.Bottom);
                Point clientPoint = Units.DocumentsToPixels(documentPoint, richEditControl1.DpiX, richEditControl1.DpiY);
                Point screenPoint = richEditControl1.PointToScreen(clientPoint);
                ribbonMiniToolbar1.Show(screenPoint);

            }
        }


        private int GetRibbonMiniToolbarHeight() {
            PropertyInfo p = typeof(RibbonMiniToolbar).GetProperty("Form", BindingFlags.NonPublic | BindingFlags.Instance);
            DevExpress.XtraBars.Ribbon.RibbonMiniToolbarPopupForm f = p.GetValue(ribbonMiniToolbar1, null) as DevExpress.XtraBars.Ribbon.RibbonMiniToolbarPopupForm;
            int height = f.Size.Height;
            return height;
        }
        private void richEditControl1_PopupMenuShowing(object sender, DevExpress.XtraRichEdit.PopupMenuShowingEventArgs e) {
            DevExpress.XtraRichEdit.Menu.RichEditPopupMenu defaultMenu = e.Menu;
            defaultMenu.CloseUp += defaultMenu_CloseUp;
            
             ribbonMiniToolbar1.OpacityOptions.AllowTransparency = false;
             int height = GetRibbonMiniToolbarHeight();
             ribbonMiniToolbar1.Show(new Point(Control.MousePosition.X, Control.MousePosition.Y - height));
        }

        void defaultMenu_CloseUp(object sender, EventArgs e) {
            DevExpress.XtraRichEdit.Menu.RichEditPopupMenu menu = sender as DevExpress.XtraRichEdit.Menu.RichEditPopupMenu;
            ribbonMiniToolbar1.OpacityOptions.AllowTransparency = true;
            menu.CloseUp -= defaultMenu_CloseUp;
        }
    }

}
