using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;
using LabBilling.UserControls;

namespace LabBilling.Library
{
    public static class InputDialogs
    {
        public static string SelectFinancialCode(string currentFin = null)
        {
            AccountRepository accountRepository = new AccountRepository(Helper.ConnVal);
            FinRepository finRepository = new FinRepository(Helper.ConnVal);

            Form frm = new Form()
            {
                Text = "Change Financial Code",
                DialogResult = DialogResult.OK,
                Width = 400,
                Height = 200
            };
            Label lbl1 = new Label()
            {
                Text = "Choose new financial class",
                Width = 150
            };
            ComboBox newFinCodeComboBox = new ComboBox()
            {
                Width = 200,
            };

            newFinCodeComboBox.DataSource = finRepository.GetAll();
            newFinCodeComboBox.DisplayMember = nameof(Fin.Description); 
            newFinCodeComboBox.ValueMember = nameof(Fin.FinCode); 

            Button okButton = new Button()
            {
                Text = "OK",
            };
            Button cancelButton = new Button()
            {
                Text = "Cancel",
            };

            okButton.Click += (o, s) =>
            {
                frm.DialogResult = DialogResult.OK;
                frm.Close();
            };
            cancelButton.Click += (o, s) =>
            {
                frm.DialogResult = DialogResult.Cancel;
                frm.Close();
            };

            frm.Controls.Add(newFinCodeComboBox);
            frm.Controls.Add(lbl1);
            frm.Controls.Add(okButton);
            frm.Controls.Add(cancelButton);

            frm.Load += (o, s) =>
            {
                lbl1.Top = 20;
                lbl1.Left = (frm.Width - lbl1.Width) / 2;
                newFinCodeComboBox.Top = lbl1.Top + lbl1.Height + 10;
                newFinCodeComboBox.Left = (frm.Width - newFinCodeComboBox.Width) / 2;

                okButton.Left = (frm.Width - okButton.Width - cancelButton.Width - 10) / 2;
                okButton.Top = newFinCodeComboBox.Bottom + 30;
                cancelButton.Left = okButton.Left + okButton.Width + 10;
                cancelButton.Top = okButton.Top;

                if (currentFin != null)
                    newFinCodeComboBox.SelectedValue = currentFin;
                else
                    newFinCodeComboBox.SelectedIndex = -1;
            };

            newFinCodeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                return newFinCodeComboBox.SelectedValue.ToString();
            }
            else
            {
                return null;
            }
        }

        public static (DateTime newDate, string reason) SelectDateOfService(DateTime currentDateOfService)
        {
            Form frm = new Form()
            {
                Text = "Change Date of Service",
                DialogResult = DialogResult.OK,
                Width = 400
            };
            Label dosLabel = new Label()
            {
                Text = "Choose the new date of service",
                Width = 130
            };
            DateTimePicker dateTimePicker = new DateTimePicker()
            {
                Name = "newDateFrm",
                Format = DateTimePickerFormat.Short,
                Width = 100
            };
            Label reasonLabel = new Label()
            {
                Text = "Enter Reason for date change",
                Width = 130
            };
            TextBox tbReason = new TextBox()
            {
                Width = 200,
                Multiline = true
            };
            Button okButton = new Button()
            {
                Text = "OK",
            };
            Button cancelButton = new Button()
            {
                Text = "Cancel",
            };

            frm.Load += (o, s) =>
            {

                dosLabel.Top = 20;
                dosLabel.Left = (frm.Width - dosLabel.Width) / 2;
                dateTimePicker.Top = dosLabel.Bottom + 10;
                dateTimePicker.Left = (frm.Width - dateTimePicker.Width) / 2;
                reasonLabel.Top = dateTimePicker.Bottom + 10;
                reasonLabel.Left = (frm.Width - reasonLabel.Width) / 2;
                tbReason.Top = reasonLabel.Bottom + 10;
                tbReason.Left = (frm.Width - tbReason.Width) / 2;

                okButton.Left = (frm.Width - okButton.Width - cancelButton.Width - 10) / 2;
                okButton.Top = tbReason.Bottom + 30;
                cancelButton.Left = okButton.Left + okButton.Width + 10;
                cancelButton.Top = okButton.Top;

                dateTimePicker.Value = currentDateOfService;
            };

            okButton.Click += (o, s) =>
            {
                if (string.IsNullOrEmpty(tbReason.Text))
                {
                    MessageBox.Show("Must enter a reason.");
                    return;
                }
                frm.DialogResult = DialogResult.OK;
                frm.Close();
            };
            cancelButton.Click += (o, s) =>
            {
                frm.DialogResult = DialogResult.Cancel;
                frm.Close();
            };

            frm.Controls.Add(dateTimePicker);
            frm.Controls.Add(tbReason);
            frm.Controls.Add(dosLabel);
            frm.Controls.Add(reasonLabel);
            frm.Controls.Add(okButton);
            frm.Controls.Add(cancelButton);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show($"New date is {dateTimePicker}. Reason is {tbReason.Text}");
                return (dateTimePicker.Value, tbReason.Text);
            }
            else
            {
                return (DateTime.MinValue, string.Empty);
            }

        }

        public static DateTime? SelectStatementBeginDate(DateTime defaultDate)
        {
            Form frm = new Form()
            {
                Text = "Generate Statement Begin Date",
                DialogResult = DialogResult.OK,
                Width = 400
            };
            Label dosLabel = new Label()
            {
                Text = "Choose the new date of service",
                Width = 130
            };
            DateTextBox dateTimePicker = new DateTextBox()
            {
                Name = "newDateFrm",
                Width = 100
            };
            Button okButton = new Button()
            {
                Text = "OK",
            };
            Button cancelButton = new Button()
            {
                Text = "Cancel",
            };

            frm.Load += (o, s) =>
            {

                dosLabel.Top = 20;
                dosLabel.Left = (frm.Width - dosLabel.Width) / 2;
                dateTimePicker.Top = dosLabel.Bottom + 10;
                dateTimePicker.Left = (frm.Width - dateTimePicker.Width) / 2;

                okButton.Left = (frm.Width - okButton.Width - cancelButton.Width - 10) / 2;
                okButton.Top = dateTimePicker.Bottom + 30;
                cancelButton.Left = okButton.Left + okButton.Width + 10;
                cancelButton.Top = okButton.Top;

                dateTimePicker.DateValue = defaultDate;
            };

            okButton.Click += (o, s) =>
            {
                frm.DialogResult = DialogResult.OK;
                frm.Close();
            };

            cancelButton.Click += (o, s) =>
            {
                frm.DialogResult = DialogResult.Cancel;
                frm.Close();
            };

            frm.Controls.Add(dateTimePicker);
            frm.Controls.Add(dosLabel);
            frm.Controls.Add(okButton);
            frm.Controls.Add(cancelButton);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show($"New date is {dateTimePicker}. Reason is {tbReason.Text}");
                return dateTimePicker.DateValue;
            }
            else
            {
                return null;
            }
        }

    }
}
