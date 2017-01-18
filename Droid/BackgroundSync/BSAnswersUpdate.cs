using System;
using Java.Lang;
using Android.App;
using System.Collections.Generic;
using Android.Content;
using BoaBeePCL;

namespace Leadbox
{
	public class BSAnswersUpdate : Thread
	{
		List<DBlocalContact> _list;
		List<DBOverviewContacts> contacts_overview;
		List<DBOverviewQuestionAnswer> answers_overview;
		public ConnectManager.StatusDownload aftersenserver { get; set; }
		public bool stopped;
		private static Activity _context;
		int increment = 0;

		public void SetStatusLog (ConnectManager.StatusDownload statuslog){
			aftersenserver = statuslog;
		}

		public void SetStopped (bool _stopped){
			stopped = _stopped;
		}

		public void SetContext (Activity context){
			_context = context;
		}

		public void SetStartStop(bool flag){
			
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

		void logHandlerFormsForUpdate(bool flag, string message)
		{
			Console.WriteLine(message);
			AfterSend();
		}

		public void AllNeedSync()
		{
			
			//_list = DBLocalDataStore.GetInstance ().GetLocalContactsNeedSetServer ();
			contacts_overview = DBLocalDataStore.GetInstance ().GetOverwievContacts (-1, "updateForAnswer");
			answers_overview = DBLocalDataStore.GetInstance().GetOverwievAnswers(-1, "update");
			if (answers_overview.Count != 0) {
				System.Threading.ThreadPool.QueueUserWorkItem (o => ConnectManager.GetInstance ().SetFormsForUpdate(logHandlerFormsForUpdate));
			}
		}

//		void AfterSend()
//		{
//			//contacts_overview = DBLocalDataStore.GetInstance ().GetOverwievContacts (-1, "updateForAnswer");
//			foreach (var cust in contacts_overview) {
//				cust.status = "server";
//				DBLocalDataStore.GetInstance ().UpdateOverwievContact (cust);
//<<<<<<< HEAD
//=======
//			}
//			stopped = true;
//
//>>>>>>> 40c1fe9b3e5e2c06e4f158dc364e53520a7ce465
//
//			}
//			if (aftersenserver != null)
//			{
//				aftersenserver(true, "All data send.");
//			}
//
//		}

		void AfterSend()
		{
			//contacts_overview = DBLocalDataStore.GetInstance ().GetOverwievContacts (-1, "updateForAnswer");
			foreach (var cust in contacts_overview) {
				cust.status = "server";
				DBLocalDataStore.GetInstance ().UpdateOverwievContact (cust);
			}



			if (aftersenserver != null)
				aftersenserver (true, "All data send.");
		}
	}
}

