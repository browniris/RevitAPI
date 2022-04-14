using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;




namespace ARCtools
{
    public partial class PlaceFamilyForm : System.Windows.Forms.Form
    {

        // Boiler Plate
        private UIApplication uiapp;
        private UIDocument uidoc;
        private Autodesk.Revit.ApplicationServices.Application app;
        public Document doc;

        public PlaceFamilyForm(ExternalCommandData commandData)
        {
            InitializeComponent();
            uiapp = commandData.Application;
            uidoc = uiapp.ActiveUIDocument;
            app = uiapp.Application;
            doc = uidoc.Document;
        }



        //varilable from main Program
        public AutoCompleteMode AutoCompleteMode { get; private set; }
        public double NS;
        public double EW;
        public string Radioctrlvalue = "projectpoint";
        public string selele;
        public double elev;
        public string familysymbol;




        private void button2_Click(object sender, EventArgs e)
        {
            //assign DialogResult as Cancel for button;
            button2.DialogResult = DialogResult.Cancel;
            //debug bookmark
            Debug.WriteLine("Cancel Clicked");
        }

        private void UpdateEnabled()
        {
            textBox1.Enabled = !radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked;
            textBox2.Enabled = !radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked;
            textBox3.Enabled = !radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked;
        }


        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                familysymbol = comboBox1.SelectedItem.ToString();
            }
            catch
            {
               TaskDialog.Show("Warning", "Select Category and Family");
            }

            //assign DialogResult as OK for button;
            button1.DialogResult = DialogResult.OK;

            //debug bookmark
            Debug.WriteLine("ok Clicked");
        }



        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
          //if projectbase point radio is checked and string variable store 
          Radioctrlvalue = "projectpoint";
          UpdateEnabled();

        }





        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //get NorthSouth Value from Textbox
            NS = 0;
            string nsstr = textBox1.Text.ToString();
            try
            {

                NS = Convert.ToDouble(nsstr);

            }
            catch
            {
                TaskDialog.Show("Warning", "entered value seems String please Enter Double");

            }

        }




        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //get EastWest Value from Textbox
            EW = 0;
            string ewstr = textBox2.Text.ToString();
            try
            {
                EW = Convert.ToDouble(ewstr);
            }
            catch
            {
                TaskDialog.Show("Warning", "entered value seems String please Enter Double");

            }
        }





        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //filterelement collector family types for combobox List
            var fec = new FilteredElementCollector(doc);
            fec.OfClass(typeof(FamilySymbol)).WhereElementIsElementType();

            //Add items to combobox list
            comboBox1.Items.Clear();
            foreach (FamilySymbol fa in fec)
            {
                
                try
                {
                    //check model category and selected category
                    if (fa.Category.CategoryType.Equals(CategoryType.Model) && fa.Category.Name.ToString().Equals(comboBox2.SelectedItem.ToString()) && (fa.Family.GetParameters("HostParameter") != null))
                    {
                        string faname = fa.Family.Name.ToString();
                        comboBox1.Items.Add(fa.Name.ToString());
                    }
                }
                catch
                {
                    TaskDialog.Show("Warning", "Select Category");
                    break;
                }

            }
         
        }




        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

            //if survey point radio is checked and string variable store 
            Radioctrlvalue = "surveypoint";

            UpdateEnabled();


        }



        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
         //if origin point radio is checked and string variable store 
            Radioctrlvalue = "originpoint";

            UpdateEnabled();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void textBox3_TextChanged(object sender, EventArgs e)
        {

            // Get Elevation value from Textbox
            elev = 0;
            string elevstr = textBox3.Text.ToString();
            try
            {
                elev = Convert.ToDouble(elevstr);
            }
            catch
            {
                TaskDialog.Show("Warning", "entered value seems String please Enter Double");
            }
           
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }




        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cat = new FilteredElementCollector(doc);

            cat.OfClass(typeof(FamilySymbol)).WhereElementIsElementType();
            List<string> catlist = new List<string>();

            foreach (FamilySymbol cups in cat)
            {
                if (cups.Category.CategoryType.Equals(CategoryType.Model) && (cups.Family.get_Parameter(BuiltInParameter.FAMILY_HOSTING_BEHAVIOR).AsInteger().Equals(0)) && (cups.Family.GetParameters("HostParameter") != null))
                {
                    if (catlist == null)
                    {
                        catlist.Add(cups.Category.Name.ToString());
                    }
                    else if (catlist.Contains(cups.Category.Name.ToString()))
                    {

                    }
                    else
                    {
                        catlist.Add(cups.Category.Name.ToString());
                    }
                }
            }

            comboBox2.Items.Clear();
            foreach(string categ in catlist)
            {
                comboBox2.Items.Add(categ.ToString());
            }
            
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

            Radioctrlvalue = "fromvalue";
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
                    
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}

