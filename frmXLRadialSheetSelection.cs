//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmXLRadialSheetSelection              '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  14DEC18                                '
//                                                                              '
//===============================================================================
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BearingCAD22
{
    public partial class frmXLRadialSheetSelection : Form
    {
        //public frmXLRadialSheetSelection()
        //{
        //    InitializeComponent();
        //}

        #region "MEMBER VARIABLE DECLARATIONS"
        //************************************
            private List<string> mSheetName = new List<string>();

        #endregion

        #region "FORM CONSTRUCTOR RELATED ROUTINE"
        //****************************************

            public frmXLRadialSheetSelection(List<string> SheetName_In)
            //==========================================================
            {
                InitializeComponent();
                mSheetName = SheetName_In;
            }

        #endregion

        private void frmXLRadialSheetSelection_Load(object sender, EventArgs e)
        //======================================================================
        {
            cmbSheetName.Items.Clear();
            int pIndex = -1;
            for (int i= 0; i< mSheetName.Count; i++)
            {
                if (mSheetName[i].ToUpper() == modMain.gFiles.XLRadial_SheetName)
                {
                    pIndex = i;
                }
                cmbSheetName.Items.Add(mSheetName[i]);
            }
            if (mSheetName.Count > 0)
            {
                if (pIndex > -1)
                {
                    cmbSheetName.SelectedIndex = pIndex;
                }
                else
                {
                    cmbSheetName.SelectedIndex = 0;
                }
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        //==================================================
        {
            modMain.gFiles.XLRadial_SheetName = cmbSheetName.Text;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        //======================================================
        {
            modMain.gFiles.XLRadial_SheetName = "";
            this.Close();
        }
    }
}
