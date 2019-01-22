
//===============================================================================
//                                                                              '
//                          SOFTWARE  :  "BearingCAD"                           '
//                       Form MODULE  :  frmSplash                              '
//                        VERSION NO  :  2.2                                    '
//                      DEVELOPED BY  :  AdvEnSoft, Inc.                        '
//                     LAST MODIFIED  :  31OCT18                                '
//                                                                              '
//===============================================================================
//
//Routines:
//---------
//   METHODS:
//   -------
//       Private Sub     frmSplash_Load      ()
//       Private Sub     cmdButtons_Click    ()
//
//================================================================================
//....Designer changed SB 03AUG09.

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
    public partial class frmSplash : Form
    {
        public frmSplash()
        //================
        {
            InitializeComponent();
        }

     //*******************************************************************************
     //*                       FORM EVENT ROUTINES - BEGIN                           *
     //*******************************************************************************

        private void frmSplash_Load(object sender, EventArgs e)
        //=====================================================
        {
         modMain.LoadImageLogo(imgLogo);
        }

     //*******************************************************************************
     //*                       FORM EVENT ROUTINES - END                             *
     //*******************************************************************************


    //*******************************************************************************
    //*                    COMMAND BUTTON EVENT ROUTINE - BEGIN                     *
    //*******************************************************************************
        
        private void cmdButtons_Click(object sender, EventArgs e)
        //=======================================================
        {
            Button pCmdBtn = (Button)sender;

            switch (pCmdBtn.Name)
            {
                case "cmdMainForm":
                     this.Hide();
                     MessageBox.Show("All open Word, Excel and Inventor files \nwill be closed automatically.\nPlease save before proceeding.", "Warning: Open Files!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     modMain.gfrmMain.Show();    
                     break;

                case "cmdExit":
                     System.Environment.Exit(0);    
                     break;
            }
        }

        //*******************************************************************************
        //*                    COMMAND BUTTON EVENT ROUTINE - END                       *
        //*******************************************************************************
    }
}
