using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LabBilling.Core
{
    public class StateComboBox : ComboBox
    {
        private const int WM_PAINT = 0xF;
        private int buttonWidth = SystemInformation.HorizontalScrollBarArrowWidth;
        Color borderColor = Color.Blue;

        public StateComboBox()
        {
            this.DataSource = new BindingSource(stateSource, null);
            this.DisplayMember = "Value";
            this.ValueMember = "Key";
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_PAINT && DropDownStyle != ComboBoxStyle.Simple)
            {
                using (var g = Graphics.FromHwnd(Handle))
                {
                    using (var p = new Pen(BorderColor))
                    {
                        g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);

                        var d = FlatStyle == FlatStyle.Popup ? 1 : 0;
                        g.DrawLine(p, Width - buttonWidth - d,
                            0, Width - buttonWidth - d, Height);
                    }
                }
            }
        }

        private static Dictionary<string, string> stateSource = new Dictionary<string, string>
            {
                { "", "--Select--" },
                { "AL", "Alabama" },
                { "AK", "Alaska" },
                { "AS", "American Samoa" },
                { "AZ", "Arizona" },
                { "AR", "Arkansas" },
                { "CA", "California" },
                { "CO", "Colorado" },
                { "CT", "Connecticut" },
                { "DE", "Delaware" },
                { "DC", "District of Columbia" },
                { "FL", "Florida" },
                { "GA", "Georgia" },
                { "GU", "Guam" },
                { "HI", "Hawaii" },
                { "ID", "Idaho" },
                { "IL", "Illinois" },
                { "IN", "Indiana" },
                { "IA", "Iowa" },
                { "KS", "Kansas" },
                { "KY", "Kentucky" },
                { "LA", "Louisiana" },
                { "ME", "Maine" },
                { "MD", "Maryland" },
                { "MA", "Massachusetts" },
                { "MI", "Michigan" },
                { "MN", "Minnesota" },
                { "UM", "Minor Outlying Islands" },
                { "MS", "Mississippi" },
                { "MO", "Missouri" },
                { "MT", "Montana" },
                { "NE", "Nebraska" },
                { "NV", "Nevada" },
                { "NH", "New Hampshire" },
                { "NJ", "New Jersey" },
                { "NM", "New Mexico" },
                { "NY", "New York" },
                { "NC", "North Carolina" },
                { "ND", "North Dakota" },
                { "MP", "Northern Mariana Islands" },
                { "OH", "Ohio" },
                { "OK", "Oklahoma" },
                { "OR", "Oregon" },
                { "PA", "Pennsylvania" },
                { "PR", "Puerto Rico" },
                { "RI", "Rhode Island" },
                { "SC", "South Carolina" },
                { "SD", "South Dakota" },
                { "TN", "Tennessee" },
                { "TX", "Texas" },
                { "VI", "U.S. Virgin Islands" },
                { "UT", "Utah" },
                { "VT", "Vermont" },
                { "VA", "Virginia" },
                { "WA", "Washington" },
                { "WV", "West Virginia" },
                { "WI", "Wisconsin" },
                { "WY", "Wyoming" }
            };
    }
}
