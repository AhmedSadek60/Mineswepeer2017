using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROJECT_ANAHERA
{
    public partial class Form1 : Form
    {
        string buf;
        Mine[,] cb = new Mine[19, 19]; //creat a 6x6 check boxs
        CheckBox[,] cbz = new CheckBox[19, 19]; //creat a 6x6 check boxs
        string thePort = "";
        int mx = 0; int my = 0;
        CheckBox box1=new CheckBox();
        CheckBox box2=new CheckBox();
        string sent_char, last_char="R";
        public string X_reading, Y_reading, State_reading, lines;
        public string path,completepath;
        PictureBox ArrowR = new PictureBox
        {
            Name = "pictureBox",
            Size = new Size(65, 65),
            Location = new Point(240, 450)
        };

        PictureBox ArrowL = new PictureBox
        {
            Name = "pictureBox",
            Size = new Size(65, 65),
            Location = new Point(80, 450)
        };

        PictureBox ArrowF = new PictureBox
        {
            Name = "pictureBox",
            Size = new Size(65, 65),
            Location = new Point(160, 400)
        };

        PictureBox ArrowB = new PictureBox
        {
            Name = "pictureBox",
            Size = new Size(65, 65),
            Location = new Point(160, 500)
        };

        PictureBox A = new PictureBox
        {
            Name = "pictureBox",
            Size = new Size(65, 65),
            Location = new Point(420, 400)
        };

        PictureBox D = new PictureBox
        {
            Name = "pictureBox",
            Size = new Size(65, 65),
            Location = new Point(420, 500)
        };

        public void Default_Image()
        {
            ArrowR.Image = Properties.Resources.ArrowR;
            ArrowL.Image = Properties.Resources.ArrowL;
            ArrowF.Image = Properties.Resources.ArrowF;
            ArrowB.Image = Properties.Resources.ArrowB;
          
        }


        public Form1()
        {
            this.MaximumSize = new System.Drawing.Size(1204, 650);


            this.Controls.Add(ArrowR);
            ArrowR.Image = Properties.Resources.ArrowR;


            this.Controls.Add(ArrowL);
            ArrowL.Image = Properties.Resources.ArrowL;


            this.Controls.Add(ArrowF);
            ArrowF.Image = Properties.Resources.ArrowF;


            this.Controls.Add(ArrowB);
            ArrowB.Image = Properties.Resources.ArrowB;

            buf = "";
            InitializeComponent();
            creatCheck(); //to creat an array of check boxs
            creatCheckz();

           
          

           


          
   

           
        }
        void creatCheck()
        {
            for (int x = 0; x < 19; x++)
            {

                for (int y = 0; y < 19; y++)
                {
                    Mine box = new Mine(x, y);
                    cb[x, y] = box;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //  tableLayoutPanel1.
            Size = new Size(1204, 780);
            foreach (string comPorts in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(comPorts);

            }

            //
        }
        private void creatCheckz()
        {
            int dy = 400;
            for (int x = 0; x < 19; x++)
            {
                int dx = 620;
                dy -= 20;
                for (int y = 0; y < 19; y++)
                {
                    CheckBox box;
                    box = new CheckBox();
                    box.AutoSize = true;
                    box.Location = new Point(dx, dy);
                    box.CheckState = CheckState.Unchecked;
                    cbz[y, x] = box;
                    this.Controls.Add(box);
                    dx += 30;
                }
            }
        }
        private void connect_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                connect.Text = "Connect";
                connect.BackColor = Color.FromArgb(128, 255, 128);
                serialPort1.Close();
            }
            else
            {
                connect.Text = "End";
                connect.BackColor = Color.Red;
            }
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                serialPort1.PortName = thePort;//comboBox1.SelectedItem.ToString(); //editable

                serialPort1.Open();
            }
            catch (Exception z)
            {

                MessageBox.Show(z.ToString());
            }
            while (serialPort1.IsOpen)
            {
                if (serialPort1.ReadBufferSize > 0)
                {
                    try
                    {
                        buf = serialPort1.ReadLine();
                        serialPort1.DiscardInBuffer();
                    }
                    catch
                    {

                       
                    }
                }

            }

        }

        private void Send_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.WriteLine(textBox1.Text);
            }
            else
            {
                MessageBox.Show("DEBUG ERROR");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (buf != "")
            {
                dataLog.Items.Add(buf);
                try
                {
                    showData(buf);
                }
                catch
                {
                }
                buf = "";
                dataLog.SelectedIndex = dataLog.Items.Count - 1;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPort1.Close();
            
        }


        double[] da = new double[3]; // a global variable where all the data are stored such that the GUI can read/write to or from it
        void decodeData(string toDecode)
        {
            try
            {
               toDecode = toDecode.Trim('"');
                da = Array.ConvertAll(toDecode.Split(','), double.Parse);
            }
            catch
            {
              //  MessageBox.Show("No data found!", "Recieving Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showData(textBox2.Text);
        }
        private void showData(string data)
        {
            decodeData(data);
            mx = (int)da[0];
            my = (int)(da[1]);
            var state = da[2].ToString();
            cb[mx, my].setState(state);
            cbz[mx, my].CheckState = (state == "1" ? CheckState.Checked : cbz[mx, my].CheckState);
            cbz[mx, my].CheckState = (state == "2" ? CheckState.Indeterminate : cbz[mx, my].CheckState);

            update();
        }
        private void update()
        {
            listBox1.Items.Clear();
            
            for (int x = 0; x < 19; x++)
            {
                for (int y = 0; y < 19; y++)
                {
                    if (cb[x, y].state != "clear")
                    {
                        listBox1.Items.Add("Mine at : " + x.ToString() + "," + y.ToString() + " state : " + cb[x, y].state);
                        X_reading = x.ToString();
                         Y_reading = y.ToString();
                        State_reading = cb[x, y].state;
                         lines = "Mine at : "+ X_reading +" , "+Y_reading+" state: "+State_reading ;
                         System.IO.File.AppendAllText(completepath, string.Empty);
                         TextWriter tw = new StreamWriter(completepath, true);
                         tw.WriteLine(lines);
                         tw.Close();
                        
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            thePort = comboBox1.SelectedItem.ToString();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            
            //capture up arrow key
           
        
            Default_Image();
            if (keyData == Keys.Up)
            {
                ArrowF.Image= Properties.Resources.ArrowFR;
                if (serialPort1.IsOpen)
                {
                    if (checkBox4.Checked == true && checkBox5.Checked == false)
                    {
                        if (last_char != "2")
                        {
                            sent_char = "2";
                            serialPort1.WriteLine(sent_char);
                            last_char = sent_char;
                        }
                    }
                    else if (checkBox5.Checked == true && checkBox4.Checked == false)
                    {
                        if (last_char != "3")
                        {
                            sent_char = "3";
                            serialPort1.WriteLine(sent_char);
                            last_char = sent_char;
                        }
                    }
                    else
                    {
                        if (last_char != "1")
                        {
                            sent_char = "1";
                            serialPort1.WriteLine(sent_char);
                            last_char = sent_char;
                        }
                    }
                }
              //  Default_Image();
                return true;
            }
            Default_Image();
            //capture down arrow key       
            if (keyData == Keys.Down)
            {
                ArrowB.Image = Properties.Resources.ArrowBR;
                if (serialPort1.IsOpen)
                {
                    if (last_char != "B")
                    {
                        sent_char = "B";
                        serialPort1.WriteLine(sent_char);
                        last_char = sent_char;
                    }
                   
                }
               // Default_Image();
                return true;
            }
           Default_Image();
            //capture left arrow key
             if (keyData == Keys.Left)
            {
                ArrowL.Image = Properties.Resources.ArrowLR;
                if (serialPort1.IsOpen)
                {
                    if (last_char != "L")
                    {
                        sent_char = "L";
                        serialPort1.WriteLine(sent_char);
                        last_char = sent_char;

                    }
                }
                
            //  Default_Image();
                return true;
            }
           Default_Image();
            //capture right arrow key
           if (keyData == Keys.Right)
           {
               ArrowR.Image = Properties.Resources.ArrowRR;
               if (serialPort1.IsOpen)
               {
                   if (last_char != "R")
                   {
                       sent_char = "R";
                       serialPort1.WriteLine(sent_char);
                       last_char = sent_char;
                   }
               }
               // Default_Image();
               return true;
           }
           if(keyData==Keys.S) {
               if (serialPort1.IsOpen)
               {
                   if (last_char != "S")
                   {
                       sent_char = "S";
                       serialPort1.WriteLine(sent_char);
                       last_char = sent_char;
                   }
               }
           }
   
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public bool SuppressKeyPress { get; set; }

        private void button2_Click(object sender, EventArgs e)
        {
            path = textBox3.Text;
            completepath = Path.Combine(path,"Mine.txt");
            if (!File.Exists(completepath))
            {
                using (File.Create(completepath));
                if (File.Exists(completepath))
                    MessageBox.Show("File Created", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Failed to create file", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (File.Exists(completepath))
            {
                MessageBox.Show("File Exists", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FileStream fileStream = File.Open(completepath, FileMode.Open);
            fileStream.SetLength(0);
            MessageBox.Show("File Cleared", "Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
            fileStream.Close();
        }
    }

    class Mine
    {
        int x, y;
        public string state="clear";
        public Mine(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
        public void setState(string x)
        {
            switch (x)
            {
                case "2":
                    state = "surface";
                    break;
                case "1":
                    state = "buried";
                    break;
                default:
                   state = state == "clear" ? x:state;
                    break;
            }
        }
    }

  
}
