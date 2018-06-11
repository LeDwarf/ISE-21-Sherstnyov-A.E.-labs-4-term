﻿using AlexeysShopService.BindingModels;
using AlexeysShopService.ViewModels;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AlexeysShopView
{
	public partial class FormCustomerContracts : Form
	{
		public FormCustomerContracts()
		{
			InitializeComponent();
		}

		private void buttonMake_Click(object sender, EventArgs e)
		{
			if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
			{
				MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			try
			{
				ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
											"c " + dateTimePickerFrom.Value.ToShortDateString() +
											" по " + dateTimePickerTo.Value.ToShortDateString());
				reportViewer.LocalReport.SetParameters(parameter);

				var response = APIClient.PostRequest("api/Report/GetCustomerContracts", new ReportBindingModel
				{
					DateFrom = dateTimePickerFrom.Value,
					DateTo = dateTimePickerTo.Value
				});
				if (response.Result.IsSuccessStatusCode)
				{
					var dataSource = APIClient.GetElement<List<CustomerContractsModel>>(response);
					ReportDataSource source = new ReportDataSource("DataSetContracts", dataSource);
					reportViewer.LocalReport.DataSources.Add(source);
				}
				else
				{
					throw new Exception(APIClient.GetError(response));
				}

				reportViewer.RefreshReport();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void buttonToPdf_Click(object sender, EventArgs e)
		{
			if (dateTimePickerFrom.Value.Date >= dateTimePickerTo.Value.Date)
			{
				MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			SaveFileDialog sfd = new SaveFileDialog
			{
				Filter = "pdf|*.pdf"
			};
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					var response = APIClient.PostRequest("api/Report/SaveCustomerContracts", new ReportBindingModel
					{
						FileName = sfd.FileName,
						DateFrom = dateTimePickerFrom.Value,
						DateTo = dateTimePickerTo.Value
					});
					if (response.Result.IsSuccessStatusCode)
					{
						MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					else
					{
						throw new Exception(APIClient.GetError(response));
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}
