using AlexeysShopService;
using AlexeysShopService.ImplementationsBD;
using AlexeysShopService.ImplementationsList;
using AlexeysShopService.Interfaces;
using System;
using System.Data.Entity;
using System.Windows.Forms;

namespace AlexeysShopView
{
	static class Program
	{
		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		[STAThread]
		static void Main()
		{
            APIClient.Connect();
            //MailClient.Connect();
            Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FormGeneral());
		}
	}
}
