﻿using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.Models;
using System.Net.Http;
using LabBilling.Logging;
using LabBilling.Core.Services;
using LabBilling.Core.DataAccess;

namespace LabBilling.Forms;

public partial class DashboardForm : Form
{
    private DictionaryService dictionaryService;
    private TableLayoutPanel announcementLayoutPanel;

    public DashboardForm()
    {
        InitializeComponent();
        dictionaryService = new(Program.AppEnvironment);
        announcementLayoutPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true
        };
    }

    private void DashboardForm_Load(object sender, EventArgs e)
    {
        //LoadArChart();
        LoadAnnouncementsWeb();
    }

    private void LoadAnnouncements()
    {
        var announcements = dictionaryService.GetActiveAnnouncements();

        System.Windows.Forms.Label hdrlbl = new()
        {
            Text = "Announcements"
        };
        hdrlbl.Font = new Font(hdrlbl.Font.FontFamily, 16, System.Drawing.FontStyle.Bold);

        announcementLayoutPanel.Controls.Add(hdrlbl);
        hdrlbl.Dock = DockStyle.Fill;

        foreach (Announcement announcement in announcements)
        {
            System.Windows.Forms.Label lbl = new System.Windows.Forms.Label
            {
                Text = announcement.StartDate.ToShortDateString(),
                ForeColor = System.Drawing.Color.Blue
            };
            lbl.Font = new Font(lbl.Font.FontFamily, 14, System.Drawing.FontStyle.Bold);


            announcementLayoutPanel.Controls.Add(lbl);
            lbl.Dock = DockStyle.Fill;
            lbl.BorderStyle = BorderStyle.None;
            lbl.BackColor = System.Drawing.Color.LightBlue;

            RichTextBox tb = new();
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
        try
        {
            dashboardLayoutPanel.Controls.Add(announcementLayoutPanel, 0, 0);
            dashboardLayoutPanel.SetRowSpan(announcementLayoutPanel, 2);

            LinkLabel hdrlbl = new()
            {
                Text = "Updates"
            };
            hdrlbl.Font = new Font(hdrlbl.Font.FontFamily, 16, System.Drawing.FontStyle.Bold);
            hdrlbl.LinkArea = new LinkArea(0, 22);
            hdrlbl.LinkClicked += new LinkLabelLinkClickedEventHandler(Hdrlbl_LinkClicked);

            // Add rows and columns to announcementLayoutPanel
            announcementLayoutPanel.ColumnCount = 1;
            announcementLayoutPanel.RowCount = 2;
            announcementLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            announcementLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80));

            announcementLayoutPanel.Controls.Add(hdrlbl, 0, 0);
            hdrlbl.Dock = DockStyle.Fill;

            var response = CallUrl(url).Result;

            WebBrowser tb = new()
            {
                DocumentText = ParseHtml(response),
                Dock = DockStyle.Fill
            };

            announcementLayoutPanel.Controls.Add(tb, 0, 1);

            dashboardLayoutPanel.SetColumnSpan(announcementLayoutPanel, 2);
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            MessageBox.Show(ex.Message);
        }
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

    //private void LoadArChart()
    //{
    //    try
    //    {
    //        ReportingRepository reportingRepository = new(Program.AppEnvironment.ConnectionString);
    //        var data = reportingRepository.GetARByFinCodeList();

    //        int i = 1;
    //        Tick[] ticks = new Tick[data.Count];
    //        foreach (var dataItem in data)
    //        {
    //            formsPlot1.Plot.Add.Bar(i, dataItem.Balance);
    //            ticks[i - 1] = new Tick(i, dataItem.FinancialClass);
    //            i++;
    //        }

    //        formsPlot1.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
    //        formsPlot1.Plot.Axes.Bottom.MajorTickStyle.Length = 0;
    //        formsPlot1.Plot.HideGrid();
    //        formsPlot1.Plot.Axes.Margins(bottom: 0);
    //        formsPlot1.Plot.Axes.Bottom.Label.Text = "Financial Class";
    //        formsPlot1.Plot.FigureBackground.Color = ScottPlot.Color.FromHex("#ffffff");
    //        formsPlot1.Plot.Title("Accounts Receivable Balance by Financial Class");
    //    }
    //    catch(Exception ex)
    //    {
    //        Log.Instance.Error(ex);            
    //    }
    //}


    private void LoadChart()
    {
        //ReportingRepository reportingRepository = new(Program.AppEnvironment.ConnectionString);
        //string seriesName = "A/R Balance";
        //var data = reportingRepository.GetARByFinCode();
        //data.DefaultView.Sort = "Financial Class";
        //data = data.DefaultView.ToTable();

        //ChartArea chartArea1 = new();
        //Legend legend1 = new();

        //chartArea1.Name = "ChartArea1";
        //chartArea1.AxisX.Interval = 1;
        //chartArea1.AxisY.LabelStyle.Format = "##,#";
        //arChart.ChartAreas.Add(chartArea1);
        //legend1.Name = "Legend1";
        //arChart.Legends.Add(legend1);

        //arChart.Palette = ChartColorPalette.Bright;
        //arChart.Titles.Add("A/R Balance by Fin Code");
        //arChart.DataSource = data;

        //Series series1 = new()
        //{
        //    Legend = legend1.Name,
        //    XValueMember = "Financial Class",
        //    YValueMembers = "Balance",
        //    Name = seriesName
        //};

        //arChart.Series.Add(series1);
        
        //arChart.Text = "A/R Balance";

    }
}
