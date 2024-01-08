using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;
using System.Windows.Forms.DataVisualization.Charting;
using System.Net.Http;
using LabBilling.Logging;

namespace LabBilling.Forms
{
    public partial class DashboardForm : BaseForm
    {
        public DashboardForm()
        {
            InitializeComponent();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            LoadChart();

            LoadAnnouncementsWeb();
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

        private void LoadAnnouncementsWeb()
        {
            string url = Program.AppEnvironment.ApplicationParameters.DocumentationSiteUrl + "/" +
                Program.AppEnvironment.ApplicationParameters.LatestUpdatesUrl;

            LinkLabel hdrlbl = new()
            {
                Text = "Updates"
            };
            hdrlbl.Font = new Font(hdrlbl.Font.FontFamily, 16, FontStyle.Bold);
            hdrlbl.LinkArea = new LinkArea(0, 22);
            hdrlbl.LinkClicked += new LinkLabelLinkClickedEventHandler(Hdrlbl_LinkClicked);

            announcementLayoutPanel.Controls.Add(hdrlbl, 0, 0);
            announcementLayoutPanel.RowStyles[0].SizeType = SizeType.Absolute;
            announcementLayoutPanel.RowStyles[0].Height = 40;
            hdrlbl.Dock = DockStyle.Fill;

            var response = CallUrl(url).Result;

            WebBrowser tb = new()
            {
                DocumentText = ParseHtml(response),
                Dock = DockStyle.Fill
            };

            announcementLayoutPanel.RowCount++;
            announcementLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80));                

            announcementLayoutPanel.Controls.Add(tb, 0, 1);

            dashboardLayoutPanel.SetColumnSpan(announcementLayoutPanel, 2);
        }

        private void Hdrlbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = Program.AppEnvironment.ApplicationParameters.DocumentationSiteUrl + "/" +
                Program.AppEnvironment.ApplicationParameters.LatestUpdatesUrl;
            System.Diagnostics.Process.Start(url);
        }

        private static async Task<string> CallUrl(string fullUrl)
        {
            try
            {
                using HttpClient client = new HttpClient();
                var response = await client.GetAsync(fullUrl)
                    .ConfigureAwait(false);

                var content = response.Content;

                string result = await content.ReadAsStringAsync();

                return result;
            }
            catch(HttpRequestException e)
            {
                Log.Instance.Error(e);
                MessageBox.Show(e.Message);
                return "";
            }

        }

        private string ParseHtml(string html)
        {
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            htmlDoc.LoadHtml(html);

            var content = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='wikitext']");


            return content.OuterHtml;
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

            ReportingRepository reportingRepository = new ReportingRepository(Program.AppEnvironment.ConnectionString);
            string seriesName = "A/R Balance";

            var data = reportingRepository.GetARByFinCode();
            data.DefaultView.Sort = "Financial Class";
            data = data.DefaultView.ToTable();

            arChart.Palette = ChartColorPalette.Bright;
            arChart.Titles.Add("A/R Balance by Fin Code");
            arChart.DataSource = data;

            Series series1 = new()
            {
                Legend = legend1.Name,
                XValueMember = "Financial Class",
                YValueMembers = "Balance",
                Name = seriesName
            };

            arChart.Series.Add(series1);
            
            arChart.Text = "A/R Balance";

        }
    }
}
