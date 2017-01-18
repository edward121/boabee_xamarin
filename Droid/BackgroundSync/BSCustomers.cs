using System;
using Java.Lang;
using Android.App;
using System.Collections.Generic;
using Android.Content;
using BoaBeePCL;

namespace Leadbox
{
	public class BSCustomers : Thread
	{
		List<DBlocalContact> _list;
		List<DBOverviewContacts> contacts_overview;
		public ConnectManager.StatusDownload aftersenserver { get; set; }
		public bool stopped;
		private static Activity _context;
		int increment = 0;

//		private static BSCustomers _instance;
//		private static object _lockCreate = new object();
//
//
//		public static BSCustomers GetInstance()
//		{
//			lock (_lockCreate)
//			{
//				if (_instance == null)
//				{
//					_instance = new BSCustomers();
//				}
//				return _instance;
//			}
//		}

		public void SetStatusLog (ConnectManager.StatusDownload statuslog){
			aftersenserver = statuslog;
		}

		public void SetStopped (bool _stopped){
			stopped = _stopped;
		}

		public void SetContext (Activity context){
			_context = context;
			//stopped = true;
			//System.Threading.ThreadPool.QueueUserWorkItem(o => SetAllContactsToServer ());
		}

		public void SetStartStop(bool flag){
			//System.Threading.Thread.QueueUserWorkItem (o => Run ());

//			if (stopped) {
//				stopped = false;
//				System.Threading.ThreadPool.QueueUserWorkItem (o => Run ());
//			} else {
//				stopped = true;
//				System.Threading.ThreadPool.QueueUserWorkItem (o => Run ());
//			}
		}

		public bool GetStatusStopped(){
			return stopped;
		}

		public override void Run ()
		{
			//base.Run ();
			SetAllContactsToServer ();
		}

		public void SetAllContactsToServer()
		{
			try{
				while(!stopped){
					Console.WriteLine("stopped = " + stopped);
					
					_context.RunOnUiThread(() => {
						Console.WriteLine(DateTime.Now + " update set customers");

						if (ConnectManager.GetInstance().StateNet())
							AllNeedSync();
						else{
							//stopped = true;
							Console.WriteLine("else stopped = " + stopped);
						}
						
					});
					try{
						Thread.Sleep(300000);//edite

						Console.WriteLine("after thread, stopped = " + stopped);
						//stopped = false;

						//SetAllContactsToServer();
					}
					catch (Java.Lang.Exception ex){
						Console.WriteLine(ex.Message);
					}

				}
			}
			catch(Java.Lang.Exception ex)
			{
				Console.WriteLine (ex.Message);
			}
		}

		void logs_order(bool flag, string message){
			if (flag) {
				AfterSend ();
				//stopped = false;
				Console.WriteLine (message);
			} else {
				Console.WriteLine (message);
			}
		}

		void logg(bool flag, string message){
			if (flag) {
				Console.WriteLine (message);
				var list_order = DBLocalDataStore.GetInstance ().GetOverwievFiles (-1, "new");
				if (list_order.Count != 0) {
					System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetOrdersContext (true, logs_order));
					return;
				}
				System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetOrdersContext (false, logs_order));
				//AfterSend ();
				//System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetOrdersContext (logs));
			} else {
				Console.WriteLine (message);
			}
		}

		void logs(bool flag, string message){
			if (flag) {
				//stopped = false;
				Console.WriteLine (message);
				var list_form = DBLocalDataStore.GetInstance ().GetOverwievAnswers (-1, "new");
				if (list_form.Count != 0) {
					System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetForms (logg));
					return;
				} 
				else {
					var list_order = DBLocalDataStore.GetInstance ().GetOverwievFiles (-1, "new");
					if (list_order.Count != 0) {
						System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetOrdersContext (true, logs_order));
						return;
					}
				}
				System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetOrdersContext (false, logs_order));
				//AfterSend ();
			} else {
				Console.WriteLine (message);
			}
		}

		public void AllNeedSync()
		{
			stopped = true;
			//_list = DBLocalDataStore.GetInstance ().GetLocalContactsNeedSetServer ();
			contacts_overview = DBLocalDataStore.GetInstance ().GetOverwievContacts (-1, "new");
			if (contacts_overview.Count != 0) {
				System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetAllContactsToServer (contacts_overview, logs));
			} 
			//else {
//				var list_form = DBLocalDataStore.GetInstance ().GetOverwievAnswers (-1, "new");
//				if (list_form.Count != 0) {
//					System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetForms (logg));
//				} else {
//					var list_order = DBLocalDataStore.GetInstance ().GetOverwievFiles (-1, "new");
//					//if (list_order.Count != 0)
//					System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetOrdersContext (logs_order));
//				}
//			}
			
		}

//		public void AllNeedSync()
//		{
//			stopped = true;
//			_list = DBLocalDataStore.GetInstance ().GetLocalContactsNeedSetServer ();
//			if (_list.Count != 0) {
//				System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetAllContactsToServer (_list, log_return));
//				System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetForms (log_return));
//				System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetOrdersContext (log_return));
//			} else {
//				System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetForms (log_return));
//				System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetOrdersContext (log_return));
//			}
//		}

//		void log_return(bool flag, string message){
//			increment++;
//			Console.WriteLine (increment % 3);
//			if (increment % 3 == 0) {
//				AfterSend ();
//			}
//		}

		void AfterSend()
		{
			contacts_overview = DBLocalDataStore.GetInstance ().GetOverwievContacts (-1, "new_send");
			foreach (var cust in contacts_overview) {
				cust.status = "server";
				DBLocalDataStore.GetInstance ().UpdateOverwievContact (cust);
			}

			if (aftersenserver != null)
				aftersenserver (true, "All data send.");
		}

	}
}

