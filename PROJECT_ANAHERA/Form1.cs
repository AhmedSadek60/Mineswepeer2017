using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        PictureBox ArrowR = new PictureBox
        {
            Name = "pictureBox",
            Size = new Size(65, 65),
            Location = new Point(260, 450)
        };

        PictureBox ArrowL = new PictureBox
        {
            Name = "pictureBox",
            Size = new Size(65, 65),
            Location = new Point(100, 450)
        };

        PictureBox ArrowF = new PictureBox
        {
            Name = "pictureBox",
            Size = new Size(65, 65),
            Location = new Point(180, 400)
        };

        PictureBox ArrowB = new PictureBox
        {
            Name = "pictureBox",
            Size = new Size(65, 65),
            Location = new Point(180, 500)
        };

        PictureBox A = new PictureBox
        {
            Name = "pictureBox",
            Size = new Size(65, 65),
            Location = new Point(350, 400)
        };

        PictureBox D = new PictureBox
        {
            Name = "pictureBox",
            Size = new Size(65, 65),
            Location = new Point(350, 500)
        };

        public void Default_Image()
        {
            ArrowR.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowR.jpg";
            ArrowL.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowL.jpg";
            ArrowF.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowF.jpg";
            ArrowB.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowB.jpg";
            A.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowF.jpg";
            D.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowB.jpg";
        }


        public Form1()
        {
            this.MaximumSize = new System.Drawing.Size(1204, 650);


            this.Controls.Add(ArrowR);
            ArrowR.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowR.jpg";


            this.Controls.Add(ArrowL);
            ArrowL.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowL.jpg";


            this.Controls.Add(ArrowF);
            ArrowF.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowF.jpg";


            this.Controls.Add(ArrowB);
            ArrowB.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowB.jpg";

         /*   this.Controls.Add(A);
            A.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowF.jpg";

            this.Controls.Add(D);
            D.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowB.jpg";
*/
            buf = "";
            InitializeComponent();
            creatCheck(); //to creat an array of check boxs
            creatCheckz();

           
            box1.AutoSize = true;
            box1.Location = new Point(260, 250);
            box1.CheckState = CheckState.Unchecked;
            this.Controls.Add(box1);

            Label box1_text;
            box1_text = new Label();
            box1_text.Text = "Fast";
            box1_text.Location = new Point(280, 250);
            this.Controls.Add(box1_text);


          
       
            box2.AutoSize = true;
            box2.Location = new Point(260, 290);
            box2.CheckState = CheckState.Unchecked;
            this.Controls.Add(box2);

            Label box2_text;
            box2_text = new Label();
            box2_text.Text = "Too Fast";
            box2_text.Location = new Point(280, 290);
            this.Controls.Add(box2_text);
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

                    //dataLog.Items.Add(serialPort1.ReadLine());
                    // serialPort1.DiscardInBuffer();
                }

            }

        }

        private void Send_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                //serialPort1.WriteLine("u" + trackBar1.Value.ToString());
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
               // int u = toDecode.Length;
               // toDecode = toDecode.Substring(1, u-1);
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

                    }
                }
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //serialPort1.PortName = "COM9"; //editable
            //MessageBox.Show( comboBox1.SelectedItem.ToString());
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
                ArrowF.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowFR.jpg";
                if (serialPort1.IsOpen)
                {
                    if(box1.Checked==true && box2.Checked==false )
                       serialPort1.WriteLine("2");
                    if(box2.Checked==true && box1.Checked==false)
                         serialPort1.WriteLine("3");
                    else
                    serialPort1.WriteLine("1");
                }
              //  Default_Image();
                return true;
            }
            Default_Image();
            //capture down arrow key       
            if (keyData == Keys.Down)
            {  
               ArrowB.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowBR.jpg";
                if (serialPort1.IsOpen)
                {
                    serialPort1.WriteLine("B");   
                }
               // Default_Image();
                return true;
            }
           Default_Image();
            //capture left arrow key
            if (keyData == Keys.Left)
            {
                ArrowL.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowLR.jpg";
                if (serialPort1.IsOpen)
                {
                    serialPort1.WriteLine("L");
                }
                
            //  Default_Image();
                return true;
            }
           Default_Image();
            //capture right arrow key
           if (keyData == Keys.Right)
           {
               ArrowR.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowRR.jpg";
               if (serialPort1.IsOpen)
               {
                   serialPort1.WriteLine("R");
               }
               // Default_Image();
               return true;
           }
           else {
               if (serialPort1.IsOpen)
               {
                   serialPort1.WriteLine("S");

               }
           }
      /*     Default_Image();
            if (keyData == Keys.A)
            {
                A.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowFR.jpg";
                if (serialPort1.IsOpen)
                {
                    serialPort1.WriteLine("U");
                }
              //  Default_Image();
                return true;
            }*/
          /*  Default_Image();
            if (keyData == Keys.S)
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.WriteLine("S");
                }
               // Default_Image();
                return true;
            }*/
           
        /*    Default_Image();
            if (keyData == Keys.D)
            {
                D.ImageLocation = @"C:\Users\ahmed\Desktop\New folder\PROJECT_ANAHERA\Resources\ArrowBR.jpg";
                if (serialPort1.IsOpen)
                {
                    serialPort1.WriteLine("D");
                }
              //  Default_Image();
                return true;
            }
           */
            return base.ProcessCmdKey(ref msg, keyData);
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
