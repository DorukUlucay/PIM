using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;

namespace PIM
{
    public partial class Blocks : Form
    {
        public Blocks()
        {
            InitializeComponent();
        }

        bool isBeingDragged = false;
        bool wasDragged = false;
        Point ptOffset;

        ContextMenu rightClickMenu = new ContextMenu();

        void B_MouseMove(object sender, MouseEventArgs e)
        {
            if (isBeingDragged)
            {
                Point newPoint = (sender as Button).PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(ptOffset);
                (sender as Button).Location = newPoint;
                wasDragged = true;
            }
        }

        void B_MouseUp(object sender, MouseEventArgs e)
        {
            isBeingDragged = false;

            SaveListIntoFile();
        }

        void B_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isBeingDragged = true;

                Point ptStartPosition = (sender as Button).PointToScreen(new Point(e.X, e.Y));

                ptOffset = new Point();
                ptOffset.X = (sender as Button).Location.X - ptStartPosition.X;
                ptOffset.Y = (sender as Button).Location.Y - ptStartPosition.Y;
            }
            else
            {
                isBeingDragged = false;

            }
        }

        string filepath;

        private void Blocks_DoubleClick(object sender, EventArgs e)
        {
            Point point = (e as MouseEventArgs).Location;

            BlocksDetail Bd = new BlocksDetail(currentTheme);
            Bd.ShowDialog("");

            if (Bd.action == "none")
            {
                return;
            }
            else if (Bd.action == "save")
            {
                PlaceButton(point, Bd.note);
                SaveListIntoFile();
            }

        }

        void B_Click(object sender, EventArgs e)
        {
            if (wasDragged)
            {
                wasDragged = false;
                return;
            }

            BlocksDetail Bd = new BlocksDetail(currentTheme);
            Bd.ShowDialog((sender as Button).Text);

            if (Bd.action == "none")
            {
                return;
            }
            else if (Bd.action == "save")
            {
                (sender as Button).Text = Bd.note;
                SaveListIntoFile();
            }
            else if (Bd.action == "delete")
            {
                this.Controls.Remove((sender as Button));
                SaveListIntoFile();
            }
        }

        void LoadListFromFile()
        {
            if (!File.Exists(filepath))
            {
                return;
            }
            string all;
            using (TextReader reader = File.OpenText(filepath))
            {
                all = reader.ReadToEnd();
                reader.Close();
            }

            Dictionary<string, Point> x = (Dictionary<string, Point>)JsonConvert.DeserializeObject(all, typeof(Dictionary<string, Point>));

            foreach (var item in x)
            {
                PlaceButton(item.Value, item.Key);
            }
        }

        public Theme currentTheme;

        public Size ButtonSize = new Size(50, 50);
        void PlaceButton(Point place, string Content)
        {
            Button B = new Button();
            B.Location = place;
            this.Controls.Add(B);
            B.Text = Content;
            B.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            B.FlatAppearance.BorderSize = currentTheme.BlockButtonBorderSize;
            B.ForeColor = currentTheme.BlockFontColor;
            B.BackColor = currentTheme.BlockBackColor;
            B.FlatAppearance.BorderColor = currentTheme.BlockBorderColor;
            B.Size = ButtonSize;
            B.MouseDown += B_MouseDown;
            B.MouseUp += B_MouseUp;
            B.MouseMove += B_MouseMove;
            B.Click += B_Click;
        }

        void SaveListIntoFile()
        {
            //yedek alıyoruz burada
            if (File.Exists(filepath))
            {
                File.Move(filepath, filepath.Replace(".txt", DateTime.Now.ToFileTime().ToString() + ".txt"));
            }


            Dictionary<string, Point> All = new Dictionary<string, Point>();

            foreach (Control item in this.Controls)
            {
                if (item is Button)
                {
                    All.Add(item.Text.ToString(), (Point)item.Location);
                }
            }


            string x = JsonConvert.SerializeObject(All);

            using (TextWriter writer = new StreamWriter(filepath))
            {

                writer.Write(x);

                writer.Flush();
                writer.Close();
            }
        }

        private void Blocks_Load(object sender, EventArgs e)
        {
            //GetIp();
            filepath = Application.StartupPath + "\\blocks.txt";
            LoadListFromFile();


            Theme Contrast = new Theme();
            Contrast.BlockBackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            Contrast.FormBackColor = System.Drawing.Color.Black;
            Contrast.FormFontColor = System.Drawing.Color.MediumVioletRed;
            Contrast.BlockFontColor = System.Drawing.Color.LimeGreen;
            Contrast.BlockBorderColor = System.Drawing.Color.CadetBlue;
            Contrast.BlockButtonBorderSize = 2;

            Theme Hot = new Theme();
            Hot.BlockBackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            Hot.FormBackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            Hot.FormFontColor = System.Drawing.Color.Firebrick;
            Hot.BlockFontColor = System.Drawing.Color.Firebrick;
            Hot.BlockBorderColor = System.Drawing.Color.Firebrick;
            Hot.BlockButtonBorderSize = 2;

            Theme Matrix = new Theme();
            Matrix.BlockBackColor = System.Drawing.Color.Black;
            Matrix.FormBackColor = System.Drawing.Color.Black;
            Matrix.FormFontColor = System.Drawing.Color.Green;
            Matrix.BlockFontColor = System.Drawing.Color.Green;
            Matrix.BlockBorderColor = System.Drawing.Color.Green;
            Matrix.BlockButtonBorderSize = 2;

            ApplySkin(Hot, this);
        }

        void themesToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //MessageBox.Show((sender as ToolStripMenuItem).
        }

        private void Blocks_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveListIntoFile();
        }

        public void ApplySkin(Theme theme, Form form)
        {

            currentTheme = theme;
            form.BackColor = theme.FormBackColor;

            foreach (Control item in form.Controls)
            {
                if (item is Button)
                {
                    (item as Button).FlatStyle = FlatStyle.Flat;
                    (item as Button).FlatAppearance.BorderColor = theme.BlockBorderColor;
                    (item as Button).BackColor = theme.BlockBackColor;
                    (item as Button).ForeColor = theme.BlockFontColor;
                    (item as Button).FlatAppearance.BorderSize = theme.BlockButtonBorderSize;
                }
                else
                {
                    item.BackColor = theme.FormBackColor;
                    item.ForeColor = theme.FormFontColor;
                }
            }
        }


        public struct Theme
        {
            public System.Drawing.Color FormBackColor;
            public System.Drawing.Color FormFontColor;
            public System.Drawing.Color BlockBorderColor;
            public System.Drawing.Color BlockBackColor;
            public System.Drawing.Color BlockFontColor;
            public int BlockButtonBorderSize;
        }


        public void GetIp()
        {
            WebClient wc = new WebClient();
            var ip = wc.DownloadString("https://api.ipify.org");
            MessageBox.Show(ip);           
        }
    }
}