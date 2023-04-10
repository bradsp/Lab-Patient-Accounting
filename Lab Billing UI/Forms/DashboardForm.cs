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

            //LoadAnnouncements();
        }

        private void LoadAnnouncements()
        {
            AnnouncementRepository announcementRepository = new AnnouncementRepository(Helper.ConnVal);

            var announcements = announcementRepository.GetActive();

            string text = @"{\rtf1\ansi\deff0";
            text += @"{\fonttbl{\f0\fswiss Arial;}}";
            text += @"{\colortbl;\red0\green0\blue0;\red255\green0\blue0;}";

            foreach (Announcement announcement in announcements)
            {
                text += @"\cf2 ";
                text += @"\b " + announcement.StartDate.ToShortDateString() + @"\b0 ";

                text += @"\cf1 ";
                text += @"\par " + announcement.MessageText + @"\par ";
            }

            text += "}";

            announcementsTextBox.Rtf = text;

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
