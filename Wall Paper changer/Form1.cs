using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wall_Paper_changer
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(
       uint action, uint uParam, string vParam, uint winIni);

        // Constants for the function
        public static readonly uint SPI_SETDESKWALLPAPER = 0x14;
        public static readonly uint SPIF_UPDATEINIFILE = 0x01;
        public static readonly uint SPIF_SENDCHANGE = 0x02;

        // Import SystemParametersInfo function
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SystemParametersInfo(uint uAction, uint uParam, StringBuilder lpvParam, uint init);

        // Constants for the function
        private const uint SPI_GETDESKWALLPAPER = 0x0073;
        private Point mouseLocation;



       
        public Form1()
        {
            InitializeComponent();
            StringBuilder wallpaperPath = new StringBuilder(200); // Buffer for wallpaper path
            bool success = SystemParametersInfo(SPI_GETDESKWALLPAPER, (uint)wallpaperPath.Capacity, wallpaperPath, 0);

            if (success)
            {
                pictureBox1.BackgroundImage = Image.FromFile(wallpaperPath.ToString());
                pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                MessageBox.Show("Failed to retrieve wallpaper path.", "Error");
            }
        }
        private void choose_Click(object sender, EventArgs e)
        {
            string filepath = "";
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg|GIF Files (*.gif)|*.gif";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    filepath = ofd.FileName;
                }
            }

            if (!string.IsNullOrEmpty(filepath))
            {
                pictureBox1.BackgroundImage = Image.FromFile(filepath);
                pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;

                // Show confirmation message box
                DialogResult result = MessageBox.Show("Do you want to set this image as your wallpaper?", "Confirm Wallpaper Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Check if the user clicked "Yes"
                if (result == DialogResult.Yes)
                {
                    // Set the wallpaper
                    SetWallpaper(filepath);
                    MessageBox.Show("Wallpaper changed successfully!", "Wallpaper Changer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public static void SetWallpaper(string path)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // Store the mouse position on mouse down
            mouseLocation = new Point(e.X, e.Y);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            // If left mouse button is held down, move the form
            if (e.Button == MouseButtons.Left)
            {
                // Calculate new form position
                this.Location = new Point(this.Location.X + e.X - mouseLocation.X,
                                          this.Location.Y + e.Y - mouseLocation.Y);
            }
        }
    }
}