﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPAR.Runner
{
    public partial class DebugWindow : Form
    {
        public DebugWindow()
        {
            InitializeComponent();
        }

        public Object Data
        {
            get
            {
                return propertyGrid.SelectedObject;
            }
            set
            {
                propertyGrid.SelectedObject = value;
            }
        }
    }
}
