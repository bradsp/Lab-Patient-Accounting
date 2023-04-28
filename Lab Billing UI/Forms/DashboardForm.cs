using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PetaPoco;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;
using System.Windows.Forms.DataVisualization.Charting;

namespace LabBilling.Forms
{
    public partial class DashboardForm : Form
    {

        public DashboardForm()
        {
            InitializeComponent();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            LoadChart();

            LoadAnnouncements();
        }

        private void LoadAnnouncements()
        {
            AnnouncementRepository announcementRepository = new AnnouncementRepository(Program.AppEnvironment);

            var announcements = announcementRepository.GetActive();

            Label hdrlbl = new Label();
            hdrlbl.Text = "Announcements";
            hdrlbl.Font = new Font(hdrlbl.Font.FontFamily, 16, FontStyle.Bold);

            announcementLayoutPanel.Controls.Add(hdrlbl);
            hdrlbl.Dock = DockStyle.Fill;

            foreach (Announcement announcement in announcements)
            {
                Label lbl = new Label();
                lbl.Text = announcement.StartDate.ToShortDateString();
                lbl.ForeColor = Color.Blue;
                lbl.Font = new Font(lbl.Font.FontFamily, 14, FontStyle.Bold);


                announcementLayoutPanel.Controls.Add(lbl);
                lbl.Dock = DockStyle.Fill;
                lbl.BorderStyle = BorderStyle.None;
                lbl.BackColor = Color.LightBlue;

                RichTextBox tb = new RichTextBox();
                tb.ContentsResized += AnnouncementContentResized;
                tb.LinkClicked += AnnouncementBox_LinkClicked;

                if (announcement.IsRtf)
                    tb.Rtf = announcement.MessageText;
                else
                    tb.Text = announcement.MessageText;

                announcementLayoutPanel.Controls.Add(tb);
                tb.Dock = DockStyle.Fill;
                tb.BorderStyle = BorderStyle.None;
                tb.ReadOnly = true;
                tb.DetectUrls = true;
            }

        }

        private void AnnouncementBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void AnnouncementContentResized(object sender, ContentsResizedEventArgs e)
        {
            var richTextBox = (RichTextBox)sender;
            richTextBox.Width = e.NewRectangle.Width;
            richTextBox.Height = e.NewRectangle.Height;
            richTextBox.Width += richTextBox.Margin.Horizontal + SystemInformation.HorizontalResizeBorderThickness;
        }

        private void LoadChart()
        {
            ChartArea chartArea1 = new ChartArea();
            Legend legend1 = new Legend();


            chartArea1.Name = "ChartArea1";
            chartArea1.AxisX.Interval = 1;
            chartArea1.AxisY.LabelStyle.Format = "##,#";
            arChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            arChart.Legends.Add(legend1);

            ReportingRepository reportingRepository = new ReportingRepository(Helper.ConnVal);
            string seriesName = "A/R Balance";

            var data = reportingRepository.GetARByFinCode();
            data.DefaultView.Sort = "Financial Class";
            data = data.DefaultView.ToTable();

            arChart.Palette = ChartColorPalette.Bright;
            arChart.Titles.Add("A/R Balance by Fin Code");
            arChart.DataSource = data;

            Series series1 = new Series();
            series1.Legend = legend1.Name;
            series1.XValueMember = "Financial Class";
            series1.YValueMembers = "Balance";
            series1.Name = seriesName;

            arChart.Series.Add(series1);
            
            arChart.Text = "A/R Balance";

        }
    }
}
