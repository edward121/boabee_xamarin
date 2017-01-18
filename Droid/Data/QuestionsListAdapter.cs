using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text.Method;
using Android.Views;
using Android.Widget;

using Android.Views.Animations;
using Android.Graphics;
using System.Globalization;
using Android.Text;
using BoaBeePCL;
using SQLite;
using Org.Xmlpull.V1.Sax2;
using Org.Xml.Sax.Helpers;
using Android.InputMethodServices;
using Android.Content.Res;
using Java.Sql;
using System.Runtime.Remoting.Messaging;
using Java.Interop;
using Android.Views.InputMethods;

namespace Leadbox
{
	public class QuestionsListAdapter : BaseAdapter<DBQuestion>
    {
		private List<DBQuestion> _items;
        private Activity _context;
		View pendingView;
		List<DBQuestion> items_false;
		List<DBOverviewQuestionAnswer> list_answers;
		DBlocalContact contact;
		DBQuestionFontSize  _questionFontSize;
		DBAnswerFontSize _answerFontSize;
		public DBColor _questinoFontColor;
		public DBColor _questinoBackgroundColor;
		public DBColor	_answerFontColor;
		public DBColor	_answerBackgroundColor;
		public string name;
		bool checkIsThere = false;
		bool checkIsThereContact = false;

		public QuestionsListAdapter(Activity context, List<DBQuestion> items)
			: base()
		{
			_context = context;
			_items = items;
		}

		public QuestionsListAdapter (Activity context, List<DBQuestion> items, List<DBQuestion> _items_false)
            : base()
        {
            _context = context;
            _items = items;
			items_false = _items_false;
        }

		public QuestionsListAdapter (Activity context, List<DBQuestion> items, List<DBOverviewQuestionAnswer> _list_answers)
			: base()
		{
			_context = context;
			_items = items;
			list_answers = _list_answers;
		}
		public QuestionsListAdapter (Activity context, List<DBQuestion> items, DBlocalContact _Contact, List<DBOverviewQuestionAnswer> _list_answers)
			: base ()
		{
			_context = context;
			_items = items;
			contact = _Contact;
			checkIsThere = true;
			list_answers = _list_answers;
		}
		public QuestionsListAdapter (Activity context, List<DBQuestion> items, DBlocalContact _Contact, List<DBOverviewQuestionAnswer> _list_answers,
		                            DBQuestionFontSize questionFontSize,DBAnswerFontSize answerFontSize,
		                            DBColor questinoFontColor,DBColor questinoBackgroundColor,
		                            DBColor answerFontColor, DBColor answerBackgroundColor)
			: base()
		{
			_context = context;
			_items = items;
			contact = _Contact;
			checkIsThere = true;
			list_answers = _list_answers;
			_questionFontSize = questionFontSize;
			_answerFontSize = answerFontSize;
			_questinoFontColor = questinoFontColor;
			_questinoBackgroundColor = questinoBackgroundColor;
			_answerFontColor = answerFontColor;
			_answerBackgroundColor = answerBackgroundColor;
		}

		public object VerticalOptions { get; private set; }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
			var item = _items [position];
//			LinearLayout questionLinear = parent.FindViewById<LinearLayout>(Resource.Id.QuestionLinear);
//			TextView previous = parent.FindViewById<TextView> (Resource.Id.textCancel);
//			TextView finish = parent.FindViewById<TextView> (Resource.Id.textNext);
//			ScrollView scroll = parent.FindViewById<ScrollView> (Resource.Id.scrollView1);


			if (item.maxLength == 0 || !string.IsNullOrEmpty(item.options)) {

				// what happens if there is nothing in this position?
				pendingView = LayoutInflater.From (_context).Inflate (Resource.Layout.ListItemClassify, parent, false);

				TextView text = pendingView.FindViewById<TextView> (Resource.Id.textViewDisplayName);
				Spinner spinnerFrom = pendingView.FindViewById<Spinner> (Resource.Id.spinnerQuestions);

				text.SetTypeface (font, TypefaceStyle.Normal);

				if (_questionFontSize == null) 
				{
					text.TextSize = 18;
				}
				else 
				{
					text.TextSize = _questionFontSize.size;
				}

				if (_questinoFontColor == null) {
					text.SetTextColor(Color.Rgb( 0x71,0x71,0x71));
					text.SetBackgroundColor (Color.Rgb (0x00, 0x00, 0x00));
				} else {
					var tempForColors = Color.Rgb ((Int32)_questinoFontColor.red, (Int32)_questinoFontColor.green, (Int32)_questinoFontColor.blue);
					text.SetTextColor (tempForColors);
				}

				//if (_answerFontColor == null) {
				//	colorPickerAnswerFontColor.Color = Color.Black;
				//} else {
				//	var tempForColors = Color.Rgb ((Int32)answerFontColor.red, (Int32)answerFontColor.green, (Int32)answerFontColor.blue);
				//	colorPickerAnswerFontColor.SetBackgroundColor (tempForColors);
				//}



				if (_questinoBackgroundColor == null) {
					//text.SetBackgroundColor (Color.White);
				} else {
					var tempForColors = Color.Rgb ((Int32)_questinoBackgroundColor.red, (Int32)_questinoBackgroundColor.green, (Int32)_questinoBackgroundColor.blue);
					//	colorPickerQuestionBackgroundColor.Color = tempForColors;

					text.SetBackgroundColor (tempForColors);
				}


					text.Text = item.question;
				if (_questionFontSize == null) 
				{
					text.TextSize = 18;
				} else 
				{ 
				text.TextSize = _questionFontSize.size;
				}

				if (items_false != null && items_false [position] != null) {
					spinnerFrom.SetBackgroundResource (Resource.Drawable.borderItemRed);
				}


				string[] From = item.options.Split ("_,_".ToCharArray ());
				List<string> fr = new List<string> ();
				fr.Add ("select a value");
				for (int i = 0; i < From.Length; i++) {
					if (From [i] != "") {
						fr.Add (From [i]);
					}
				}

				if (contact != null)
				{
//					var answer = list_answers.Find(s => s.question == item.question);
					checkIfThere(text.Text);
					spinnerFrom.SetSelection(ReturnValue(fr, name));
					name = "";
					if (_answerBackgroundColor == null) {
						spinnerFrom.SetBackgroundColor (Color.Rgb (0x1B, 0x1B, 0x1B));
					} else {
						var tempForColors = Color.Rgb ((Int32)_answerBackgroundColor.red, (Int32)_answerBackgroundColor.green, (Int32)_answerBackgroundColor.blue);
						//	colorPickerAnswerBackgroundColor.Color = tempForColors;
						spinnerFrom.SetBackgroundColor (tempForColors);
					}
					SpinnerListAdapter _adapterFrom = new SpinnerListAdapter (_context, Resource.Layout.item_text_select_kiosk,
					                                                          fr,
					                                                         _answerFontColor, _answerBackgroundColor,
					                                                         _answerFontSize);

					_adapterFrom.SetDropDownViewResource (Resource.Layout.item_text_select_kiosk);

					spinnerFrom.Adapter = _adapterFrom;
					spinnerFrom.ItemSelected += (sender, e) =>
					{
							//answer.answer = (string)spinnerFrom.GetItemAtPosition(e.Position);

							checkIfThereContact(item.question);
							if(checkIsThereContact)
							{
								checkIfThereContactAdd(item.question,(string)spinnerFrom.GetItemAtPosition(e.Position));
							}
							else
							{
								SaveAndLoad.tempAnswer[position].question = item.question;
								SaveAndLoad.tempAnswer[position].required = item.required;
								SaveAndLoad.tempAnswer[position].name_question = item.name;
								SaveAndLoad.tempAnswer[position].answer =(string)spinnerFrom.GetItemAtPosition(e.Position);
								SaveAndLoad.tempAnswer[position].type_question = item.type;
								SaveAndLoad.tempAnswer[position].datetime = DateTime.Now;
								//SaveAndLoad.tempAnswer[position].status = "update";
								SaveAndLoad.tempAnswer[position].session = -1;	
							}	
					};
				}
				else
				{
					if (checkIsThere)
					{
						SpinnerListAdapter _adapterFrom = new SpinnerListAdapter (_context, Resource.Layout.item_text_select_kiosk,
						                                                          fr,
						                                                         _answerFontColor, _answerBackgroundColor,
						                                                         _answerFontSize);
						if (_answerBackgroundColor == null) {
							//spinnerFrom.SetBackgroundColor (Color.Rgb (0x1b, 0x1b, 0x1b));
						} else {
							var tempForColors = Color.Rgb ((Int32)_answerBackgroundColor.red, (Int32)_answerBackgroundColor.green, (Int32)_answerBackgroundColor.blue);
							//	colorPickerAnswerBackgroundColor.Color = tempForColors;
							spinnerFrom.SetBackgroundColor (tempForColors);
						}
						_adapterFrom.SetDropDownViewResource(Resource.Layout.item_text_select_kiosk);
						spinnerFrom.Adapter = _adapterFrom;
					}
					else
					{
						SpinnerListAdapter _adapterFrom = new SpinnerListAdapter (_context, Resource.Layout.item_text_select, fr,
						                                                         _answerFontColor, _answerBackgroundColor,
						                                                         _answerFontSize);

						spinnerFrom.SetBackgroundResource (Resource.Drawable.borderItem);
						spinnerFrom.SetSelection (ReturnValue(fr, SaveAndLoad.GetInstance().GetTextPosition(position)));
                        _adapterFrom.SetDropDownViewResource (Resource.Layout.item_text_select);

						spinnerFrom.Adapter = _adapterFrom;
				    }
				}

				//spinnerFrom


				spinnerFrom.ItemSelected += (sender, e) => _context.RunOnUiThread(() =>{
					Console.WriteLine((string)spinnerFrom.GetItemAtPosition(e.Position));
					SaveAndLoad.GetInstance().SaveText((string)spinnerFrom.GetItemAtPosition(e.Position), position);
				});


				spinnerFrom.SetSelection (ReturnValue(fr, SaveAndLoad.GetInstance().GetTextPosition(position)));

				if (list_answers != null)
				{
					var answer = list_answers.Find(s => s.question == item.question);
					if (answer != null)
					{
						spinnerFrom.SetSelection(ReturnValue(fr, answer.answer));

						spinnerFrom.ItemSelected += (sender, e) => _context.RunOnUiThread(() =>
							{
								answer.answer = (string)spinnerFrom.GetItemAtPosition(e.Position);

								SaveAndLoad.tempAnswer[position].question = item.question;
								SaveAndLoad.tempAnswer[position].required = item.required;
								SaveAndLoad.tempAnswer[position].name_question = item.name;
								SaveAndLoad.tempAnswer[position].answer = answer.answer;
								SaveAndLoad.tempAnswer[position].type_question = item.type;
								SaveAndLoad.tempAnswer[position].datetime = list_answers[0].datetime;
								//SaveAndLoad.tempAnswer[position].status = "update";
								SaveAndLoad.tempAnswer[position].session = list_answers[0].session;
								//DBLocalDataStore.GetInstance().UpdateOverwievAnswer(answer);
							});
					}
					else
					{

						list_answers.Add(new DBOverviewQuestionAnswer
							{
								question = item.question,
								required = item.required,
								name_question = item.name,
								answer = fr[0],
								type_question = item.type,
								datetime = list_answers[0].datetime,
								status = "new",
								session = list_answers[0].session
							});
						this.GetView(position, null, parent);
					}

				}
				else
				{
					spinnerFrom.SetSelection(0);
				}

				


			}
			else if(item.type =="string"|| item.type =="integer")
			{
                pendingView = LayoutInflater.From (_context).Inflate (Resource.Layout.ListItemClassifyText, parent, false);


				TextView text = pendingView.FindViewById<TextView> (Resource.Id.textViewDisplayName);
				answer = pendingView.FindViewById<EditText> (Resource.Id.editAnswer);
				text.SetTypeface (font, TypefaceStyle.Normal);

				if (item.type == "integer") 
				{
					answer.SetRawInputType (InputTypes.ClassNumber);
				}
				if (_questionFontSize == null) 
				{
					text.TextSize = 18;
				} 
				else 
				{
					text.TextSize = _questionFontSize.size;
				}

				if (_answerFontSize == null) 
				{
					answer.TextSize = 18;
				}
				else 
				{
					answer.TextSize = _answerFontSize.size;
				}
				if (_questinoFontColor == null) {
					text.SetTextColor (Color.Rgb (0x71, 0x71, 0x71));
				} else {
					var tempForColors = Color.Rgb ((Int32)_questinoFontColor.red, (Int32)_questinoFontColor.green, (Int32)_questinoFontColor.blue);
					text.SetTextColor (tempForColors);
				}
				answer.Click += (sender, e) => {
					((ScrollView)((LinearLayout)parent).Parent).SmoothScrollingEnabled = true;

                    ((ScrollView)((LinearLayout)parent).Parent).ScrollTo(10, 0);

				};
				if (_questinoBackgroundColor == null) {
					//text.SetBackgroundColor (Color.White);
				} else {
					var tempForColors = Color.Rgb ((Int32)_questinoBackgroundColor.red, (Int32)_questinoBackgroundColor.green, (Int32)_questinoBackgroundColor.blue);
					//	colorPickerQuestionBackgroundColor.Color = tempForColors;

					text.SetBackgroundColor (tempForColors);
				}
				text.Text = item.question;
				if (contact != null)
				{
//					var _answer = list_answers.Find(s => s.question == item.question);
					checkIfThere(text.Text);
					answer.Text = name;
					text.Text = item.question;

					name = "";
					if (_answerBackgroundColor == null) {

						answer.SetBackgroundColor (Color.Gray);
					} else {
						var tempForColors = Color.Rgb ((Int32)_answerBackgroundColor.red, (Int32)_answerBackgroundColor.green, (Int32)_answerBackgroundColor.blue);
						//	colorPickerAnswerBackgroundColor.Color = tempForColors;
						answer.SetBackgroundColor (tempForColors);
					}
					if (_answerFontColor == null) {
							answer.SetTextColor (Color.Rgb (0xED, 0xCD, 0x00));
						} else {
							var tempForColors = Color.Rgb ((Int32)_answerFontColor.red, (Int32)_answerFontColor.green, (Int32)_answerFontColor.blue);
							answer.SetTextColor (tempForColors);
						}
						
					answer.TextChanged += (sender, e) =>
					{
//							_answer.answer = e.Text.ToString ();
							Console.WriteLine(e.Text.ToString ());
							checkIfThereContact(item.question);
							if(checkIsThereContact)
							{
								checkIfThereContactAdd(item.question,answer.Text);
							}
							else
							{
							SaveAndLoad.tempAnswer[position].question = item.question;
							SaveAndLoad.tempAnswer[position].required = item.required;
							SaveAndLoad.tempAnswer[position].name_question = item.name;
							SaveAndLoad.tempAnswer[position].answer =e.Text.ToString ();
							SaveAndLoad.tempAnswer[position].type_question = item.type;
							SaveAndLoad.tempAnswer[position].datetime = DateTime.Now;
							//SaveAndLoad.tempAnswer[position].status = "update";
							SaveAndLoad.tempAnswer[position].session = -1;	
							}
					};
				}
				else
				{
					if (checkIsThere)
					{
						answer.SetBackgroundColor(Color.Gray);
						answer.SetTextColor(Color.Rgb(0xED,0xCD,0x00));
					}
					else
					{
						answer.SetBackgroundResource(Resource.Drawable.borderItemText);
						answer.Text = SaveAndLoad.GetInstance().LoadTextEdit(position);
					}
				}
				if (items_false != null && items_false [position] != null) {
					answer.SetBackgroundResource (Resource.Drawable.borderItemRedText);
				}

				//Spinner spinnerFrom = pendingView.FindViewById<Spinner> (Resource.Id.spinnerQuestions);
				//spinnerFrom.Visibility = ViewStates.Gone;

				answer.SetTypeface (font, TypefaceStyle.Normal);
				//answer.SetSelectAllOnFocus (true);

				//answer.Focusable = true;
				answer.SetFilters (new IInputFilter[1]{new InputFilterLengthFilter(int.Parse(item.maxLength.ToString())) });

				if (list_answers != null)
				{
					var _answer = list_answers.Find(s => s.question == item.question);
					if (_answer != null)
					{
						answer.Text = _answer.answer;
						answer.TextChanged += (sender, e) =>
						{
							//list_answers[position].answer =answer.Text;
							_answer.answer = e.Text.ToString ();
							Console.WriteLine(_answer.answer);
								SaveAndLoad.tempAnswer[position].question = item.question;
								SaveAndLoad.tempAnswer[position].required = item.required;
								SaveAndLoad.tempAnswer[position].name_question = item.name;
								SaveAndLoad.tempAnswer[position].answer = _answer.answer;
								SaveAndLoad.tempAnswer[position].type_question = item.type;
								SaveAndLoad.tempAnswer[position].datetime = list_answers[0].datetime;
								//SaveAndLoad.tempAnswer[position].status = "update";
								SaveAndLoad.tempAnswer[position].session = list_answers[0].session;


							//DBLocalDataStore.GetInstance().UpdateOverwievAnswer(_answer);
						};
					}
					else
					{
						
						list_answers.Add (new DBOverviewQuestionAnswer{
							question = item.question,
							required = item.required,
							name_question = item.name,
							answer = answer.Text,
							type_question = item.type,
							datetime = list_answers[0].datetime,
							status = "new",
							session = list_answers[0].session
						});
						this.GetView(position, null, parent);
					}
				}
				else
				{
					

					answer.AddTextChangedListener(new EditTextListen() 
					{ _editText = answer, _context = _context, _pos = position, maxlenght = int.Parse(item.maxLength.ToString()) });
				}

				//				answer.KeyPress += (object sender, View.KeyEventArgs e) => {
				//					Toast.MakeText (_context, "answer.KeyPress", ToastLength.Short).Show ();
				//					e.Handled = false;
				//					if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter) {
				//						Toast.MakeText (_context, answer.Text, ToastLength.Short).Show ();
				//						e.Handled = true;
				//					}
				//				};
				//


				//				answer.AfterTextChanged += (object sender, AfterTextChangedEventArgs e) => {
				//					Toast.MakeText(_context, answer.Text, ToastLength.Long).Show();
				//				};

				//answer.AfterTextChanged += (object sender, AfterTextChangedEventArgs e) => {
				//ITextWatcher tw = new ITextWatcher;

				//};


                //answer.SetRawInputType(InputTypes.Null);
                //answer.Click += delegate {
                //    if (item.type == "integer")
                //    {
                //        answer.SetRawInputType(InputTypes.ClassNumber);
                //    }
                //    else {
                //        answer.SetRawInputType(InputTypes.ClassText);
                //    }
                //    answer.ImeOptions = Android.Views.InputMethods.ImeAction.Next;
                //    ShowKeyboard(pendingView);
                //};

			}
			else if(item.type =="datetime"|| item.type =="date")
			{
				pendingView = LayoutInflater.From (_context).Inflate (Resource.Layout.ListItemClassifyDateTime, parent, false);
				TextView text = pendingView.FindViewById<TextView> (Resource.Id.textView1);
				TextView buttonDate = pendingView.FindViewById<TextView> (Resource.Id.buttonDateTime);
				TextView textDate = pendingView.FindViewById<TextView> (Resource.Id.textView2);
				DatePicker date = pendingView.FindViewById<DatePicker> (Resource.Id.datePicker1);
				TimePicker timePicker = pendingView.FindViewById<TimePicker> (Resource.Id.timePicker1);
                //RelativeLayout relativeLayout = pendingView.FindViewById<RelativeLayout> (Resource.Id.relativeLayout1);
                textDate.Click+= delegate {
                    
                };


				timePicker.Visibility = ViewStates.Gone;
				date.Visibility = ViewStates.Gone;
				buttonDate.Visibility = ViewStates.Gone;
				date.CalendarView.Visibility = ViewStates.Gone;

				text.SetTypeface (font, TypefaceStyle.Normal);
				textDate.SetTypeface (font, TypefaceStyle.Normal);
				buttonDate.SetTypeface (font, TypefaceStyle.Normal);

				buttonDate.SetTextColor (Color.Rgb (0xff, 0xff, 0xff));
				if (_questionFontSize == null) 
				{
					text.TextSize = 18;
				}
				else 
				{
					text.TextSize = _questionFontSize.size;
				}

				if (_answerFontSize == null)
				{
					textDate.TextSize = 18;
				} 
				else 
				{
					textDate.TextSize = _answerFontSize.size;
				}
				if (_questinoFontColor == null) {
					text.SetTextColor (Color.Rgb (0x71, 0x71, 0x71));
				} else {
					var tempForColors = Color.Rgb ((Int32)_questinoFontColor.red, (Int32)_questinoFontColor.green, (Int32)_questinoFontColor.blue);
					text.SetTextColor (tempForColors);
				}
				//if (_questinoBackgroundColor == null) {
				//	text.SetBackgroundColor (Color.White);
				//} else {
				//	var tempForColors = Color.Rgb ((Int32)_questinoBackgroundColor.red, (Int32)_questinoBackgroundColor.green, (Int32)_questinoBackgroundColor.blue);
				//	//	colorPickerQuestionBackgroundColor.Color = tempForColors;

				//	text.SetBackgroundColor (tempForColors);
				//}
				text.Text = item.question;
				if (contact != null)
				{
					checkIfThere(text.Text);
					textDate.Text = name;
					name = "";


					if (_answerBackgroundColor == null) {

						textDate.SetBackgroundColor (Color.Rgb (0x71, 0x71, 0x71));
					} else {
						var tempForColors = Color.Rgb ((Int32)_answerBackgroundColor.red, (Int32)_answerBackgroundColor.green, (Int32)_answerBackgroundColor.blue);
						//	colorPickerAnswerBackgroundColor.Color = tempForColors;
						textDate.SetBackgroundColor (tempForColors);
					}
					if (_answerFontColor == null) {
						textDate.SetTextColor (Color.Rgb (0xff, 0xff, 0xff));
					} else {
						var tempForColors = Color.Rgb ((Int32)_answerFontColor.red, (Int32)_answerFontColor.green, (Int32)_answerFontColor.blue);
						textDate.SetBackgroundColor (tempForColors);
					}
					textDate.TextChanged += (sender, e) =>
						{
						//layoutparams.addRule (RelativeLayout.CENTER_HORIZONTAL);
						//	mTextView.setLayoutParams (layoutParams);
							Console.WriteLine(e.Text.ToString ());
							checkIfThereContact(item.question);
							if(checkIsThereContact)
							{
								checkIfThereContactAdd(item.question,textDate.Text);
							}
							else
							{
								SaveAndLoad.tempAnswer[position].question = item.question;
								SaveAndLoad.tempAnswer[position].required = item.required;
								SaveAndLoad.tempAnswer[position].name_question = item.name;
								SaveAndLoad.tempAnswer[position].answer =e.Text.ToString ();
								SaveAndLoad.tempAnswer[position].type_question = item.type;
								SaveAndLoad.tempAnswer[position].datetime = DateTime.Now;
								//SaveAndLoad.tempAnswer[position].status = "update";
								SaveAndLoad.tempAnswer[position].session = -1;	
							}
						};
				}
				else
				{
					if (checkIsThere)
					{
						textDate.SetBackgroundColor(Color.Gray);
						textDate.SetTextColor(Color.Rgb(0xff,0xff,0xff));
					}
					else
					{
						textDate.SetBackgroundResource (Resource.Drawable.borderItemText);
						textDate.Text = SaveAndLoad.GetInstance().LoadTextEdit(position);
					}
				}
				textDate.Click += (sender, e) => 
				{
                    HideKeuboard(pendingView);
					if (item.type == "datetime"){
									   timePicker.Visibility = ViewStates.Visible;
							buttonDate.Visibility = ViewStates.Visible;
							date.Visibility = ViewStates.Visible;
							date.CalendarView.Date.ToString("dd/MM");
							SaveAndLoad.GetInstance().SaveTextEdit(textDate.Text, position);

						}
						else if (item.type == "date")
						{
							timePicker.Visibility = ViewStates.Gone;
							buttonDate.Visibility = ViewStates.Visible;
							date.Visibility = ViewStates.Visible;
							date.CalendarView.Date.ToString("dd/MM/yyyy");
							SaveAndLoad.GetInstance().SaveTextEdit(textDate.Text, position);
						}

					
					};

				buttonDate.Click += (object sender, EventArgs e) => 
					{
						((ScrollView)((LinearLayout)parent).Parent).SmoothScrollingEnabled = true;
						((ScrollView)((LinearLayout)parent).Parent).ScrollTo(10, 0);
						if (item.type == "datetime")
						{
							string fotmatDateTime = "";
							if((int)timePicker.CurrentHour > 12)
							{
								fotmatDateTime = "PM";
							}
							else
							{
								fotmatDateTime = "AM";
							}
							textDate.Text = Convert.ToString(date.DateTime.Date.ToString("d/ddd/MMM")+ timePicker.CurrentHour+ ":"+timePicker.CurrentMinute + fotmatDateTime);
							SaveAndLoad.GetInstance().SaveTextEdit(textDate.Text, position);

						}
						else if (item.type == "date")
						{
							textDate.Text = Convert.ToString(date.DateTime.Date.ToString("dd/MM/yyyy"));
							SaveAndLoad.GetInstance().SaveTextEdit(textDate.Text, position);

						}
						timePicker.Visibility = ViewStates.Gone;
						buttonDate.Visibility = ViewStates.Gone;
						date.Visibility = ViewStates.Gone;
					};
				
				textDate.Text = SaveAndLoad.GetInstance ().LoadTextEdit(position);
				textDate.SetFilters (new IInputFilter[1]{new InputFilterLengthFilter(int.Parse(item.maxLength.ToString())) });

				if (list_answers != null)
				{
					var _answer = list_answers.Find(s => s.question == item.question);
					if (_answer != null)
					{
						textDate.Text = _answer.answer;
						textDate.TextChanged += (sender, e) =>
						{
							_answer.answer = textDate.Text;
							Console.WriteLine(_answer.answer);
								SaveAndLoad.tempAnswer[position].question = item.question;
								SaveAndLoad.tempAnswer[position].required = item.required;
								SaveAndLoad.tempAnswer[position].name_question = item.name;
								SaveAndLoad.tempAnswer[position].answer = _answer.answer;
								SaveAndLoad.tempAnswer[position].type_question = item.type;
								SaveAndLoad.tempAnswer[position].datetime = list_answers[0].datetime;
								//SaveAndLoad.tempAnswer[position].status = "update";
								SaveAndLoad.tempAnswer[position].session = list_answers[0].session;
							//DBLocalDataStore.GetInstance().UpdateOverwievAnswer(_answer);
						};
					}
				else
					{
						
						list_answers.Add (new DBOverviewQuestionAnswer{
							question = item.question,
							required = item.required,
							name_question = item.name,
							answer = textDate.Text,
							type_question = item.type,
							datetime = list_answers[0].datetime,
							status = "new",
							session = list_answers[0].session
						});
						this.GetView(position, null, parent);
					}
				}
				else
				{
					textDate.Text = "";
//                    textDate.AddTextChangedListener(new EditTextListen() 
//						{ _editText = textDate, _context = _context, _pos = position, maxlenght = int.Parse(item.maxLength.ToString()) });
				}
			
			}
            return pendingView;
        }
        EditText answer;
        public static void ShowKeyboard(View pView)
        {
            pView.RequestFocus();

            InputMethodManager inputMethodManager = Application.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.ShowSoftInput(pView, ShowFlags.Forced);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
        }

        private void HideKeuboard(View view)
        {
            InputMethodManager imm = (InputMethodManager)_context.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(view.WindowToken, 0);
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return _items.Count; }
        }

		public override DBQuestion this[int position]
        {
            get { return _items[position]; }
        }

		int ReturnValue(List<string> _list, string _value)
		{
			var _pos = _list.IndexOf (_value);
			return _pos;
		}

		public override void NotifyDataSetChanged ()
		{
			base.NotifyDataSetChanged ();
		}

		public override void NotifyDataSetInvalidated ()
		{
			base.NotifyDataSetInvalidated ();
		}
		public void checkIfThere(string textEdite)
		{
            ///////////////////////////////////////////WARNING BADGE/////////////////////////////////////////////////////////////////       
            switch(textEdite.ToLower())
			{
				case "first name":
                    name=contact.firstname;
                    SaveAndLoad.tempContact.firstname=name;
					break;
				case "last name":
                    name=contact.lastname;
                    SaveAndLoad.tempContact.lastname=name;
					break;
				case "email":
					name=contact.email;
					SaveAndLoad.tempContact.email=name;
					break;
				case "company":
					name=contact.company;
					SaveAndLoad.tempContact.company=name;
					break;
				case "phone":
					name=contact.phone;
					SaveAndLoad.tempContact.phone=name;
                    break;
				case "barcode":
                    name=contact.uid;
                    SaveAndLoad.tempContact.uid=name;
					break;
				case "jobTitle":
                    name=contact.jobtitle;
                    SaveAndLoad.tempContact.jobtitle=name;
					break;
				case "externalReference":
					name=contact.externalReference;
					SaveAndLoad.tempContact.externalReference=name;
					break;
				case "externalCompanyReference":
					name=contact.externalCompanyReference;
					SaveAndLoad.tempContact.externalCompanyReference=name;
					break;
				case "prefix":
					name=contact.prefix;
					SaveAndLoad.tempContact.prefix=name;
					break;
				case "mobile":
					name=contact.mobile;
					SaveAndLoad.tempContact.mobile=name;
					break;
				case "fax":
					name=contact.fax;
					SaveAndLoad.tempContact.fax=name;
					break;
				case "vat":
					name=contact.vat;
					SaveAndLoad.tempContact.vat=name;
					break;
				case "function":
					name=contact.function;
					SaveAndLoad.tempContact.function=name;
					break;
				case "level":
					name=contact.level;
					SaveAndLoad.tempContact.level=name;
					break;
				case "department":
					name=contact.department;
					SaveAndLoad.tempContact.department=name;
					break;
				case "street":
					name=contact.street;
					SaveAndLoad.tempContact.street=name;
					break;
				case "city":
					name=contact.city;
					SaveAndLoad.tempContact.city=name;
					break;
				case "zip":
					name=contact.zip;
					SaveAndLoad.tempContact.zip=name;
					break;
				case "country":
					name=contact.country;
					SaveAndLoad.tempContact.country=name;
					break;
				
			default:
				Console.WriteLine("Default case");
			break;
		    }
		
		}
        ///////////////////////////////////////////WARNING BADGE/////////////////////////////////////////////////////////////////       
        public void checkIfThereContactAdd(string question,string answer)
		{
			switch(question.ToLower())
			{
				case "first name":
                    SaveAndLoad.tempContact.firstname=answer;
					break;
				case "last name":
                    SaveAndLoad.tempContact.lastname=answer;
					break;
				case "email":
					SaveAndLoad.tempContact.email=answer;
					break;
				case "company":
					SaveAndLoad.tempContact.company=answer;
					break;
				case "phone":
					SaveAndLoad.tempContact.phone=answer;
					break;
				case "barcode":
                    SaveAndLoad.tempContact.uid=answer;
					break;
				case "jobTitle":
                    SaveAndLoad.tempContact.jobtitle=answer;
					break;
				case "externalReference":
					SaveAndLoad.tempContact.externalReference=answer;
					break;
				case "externalCompanyReference":
					SaveAndLoad.tempContact.externalCompanyReference=answer;
					break;
				case "prefix":
					SaveAndLoad.tempContact.prefix=answer;
					break;
				case "mobile":
					SaveAndLoad.tempContact.mobile=answer;
					break;
				case "fax":
					SaveAndLoad.tempContact.fax=answer;
					break;
				case "vat":
					SaveAndLoad.tempContact.vat=answer;
					break;
				case "function":
					SaveAndLoad.tempContact.function=answer;
					break;
				case "level":
					SaveAndLoad.tempContact.level=answer;
					break;
				case "department":
					SaveAndLoad.tempContact.department=answer;
					break;
				case "street":
					SaveAndLoad.tempContact.street=answer;
					break;
				case "city":
					SaveAndLoad.tempContact.city=answer;
					break;
				case "zip":
					SaveAndLoad.tempContact.zip=answer;
					break;
				case "country":
					SaveAndLoad.tempContact.country=answer;
					break;

				default:
					Console.WriteLine("Default case");
					break;
			}

		}
		public void checkIfThereContact(string question)
		{
			checkIsThereContact = false;
			switch(question.ToLower())
			{
				case "first name":
					checkIsThereContact = true;
					break;
				case "last name":
					checkIsThereContact = true;
					break;
				case "email":
					checkIsThereContact = true;
					break;
				case "company":
					checkIsThereContact = true;
					break;
				case "phone":
					checkIsThereContact = true;
					break;
				case "source":
					checkIsThereContact = true;
					break;
				case "barcode":
					checkIsThereContact = true;
					break;
				case "jobTitle":
					checkIsThereContact = true;
					break;
				case "externalReference":
					checkIsThereContact = true;
					break;
				case "externalCompanyReference":
					checkIsThereContact = true;
					break;
				case "internalContactName":
					checkIsThereContact = true;
					break;
				case "internalContactEmail":
					checkIsThereContact = true;
					break;
				case "prefix":
					checkIsThereContact = true;
					break;
				case "mobile":
					checkIsThereContact = true;
					break;
				case "fax":
					checkIsThereContact = true;
					break;
				case "vat":
					checkIsThereContact = true;
					break;
				case "function":
					checkIsThereContact = true;
					break;
				case "level":
					checkIsThereContact = true;
					break;
				case "department":
					checkIsThereContact = true;
					break;
				case "street":
					checkIsThereContact = true;
					break;
				case "city":
					checkIsThereContact = true;
					break;
				case "zip":
					checkIsThereContact = true;
					break;
				case "country":
					checkIsThereContact = true;
					break;

				default:
					Console.WriteLine("Default case");
					break;
			}

		}
	}

	public class EditTextListen : Java.Lang.Object, ITextWatcher
	{
		public EditText _editText { get; set; }
		public Activity _context { get; set; }
		public int _pos { get; set; }
		public int maxlenght { get; set; }

		#region ITextWatcher implementation

		public void AfterTextChanged (IEditable s)
		{
			String account = s.ToString ();
			Console.WriteLine ("Account variable = " + account);
			//AccountStorage.SetAccount (Activity, account);
		}

		public void BeforeTextChanged (Java.Lang.ICharSequence s, int start, int count, int after)
		{
			//Not implemented
			Console.WriteLine ("Before Change - Lenght char = " + start + "/" + maxlenght);
		}

		public void OnTextChanged (Java.Lang.ICharSequence s, int start, int before, int count)
		{
			//Toast.MakeText(_context, _editText.Text, ToastLength.Short).Show();
//			_editText.Focusable = true;
//			_editText.FocusableInTouchMode = true;
//			_editText.CallOnClick ();

			Console.WriteLine ("Lenght char = " + start + "/" + maxlenght);
			if (start >= maxlenght - 1) {
				_editText.SetError ("You can not enter more than " + maxlenght + " characters", null);
				//_editText.Error = "You can not enter more than " + maxlenght + " characters";
				return;
			}

			SaveAndLoad.GetInstance().SaveTextEdit(_editText.Text, _pos);


				
			//Not implemented
		}



		#endregion


	}

	public class SpinnerListAdapter : ArrayAdapter<string>
	{
		Activity context;
		int resId;
		List<string> answers;
		 DBColor _answerFontColor;
		 DBColor _answerBackgroundColor;
		DBAnswerFontSize _answerFontSize;

		public SpinnerListAdapter(Activity _context, int _resId, List<string> _answers,
		                          DBColor answerFontColor, DBColor answerBackgroundColor,DBAnswerFontSize answerFontSize) : base(_context, _resId, _answers)
		{
			context = _context;
			resId = _resId;
			answers = _answers;
			_answerFontColor = answerFontColor;
			_answerBackgroundColor = answerBackgroundColor;
			_answerFontSize = answerFontSize;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			TextView _view = (TextView)LayoutInflater.From (context).Inflate (resId, parent, false);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
			_view.SetTypeface (font, TypefaceStyle.Normal);
			_view.Text = answers[position];
			//if (_answerBackgroundColor == null) {
				_view.SetBackgroundColor (Color.Rgb (0x1B, 0x1B, 0x1B));
			//} else {
			//	var tempForColors = Color.Rgb ((Int32)_answerBackgroundColor.red, (Int32)_answerBackgroundColor.green, (Int32)_answerBackgroundColor.blue);
			//	//	colorPickerAnswerBackgroundColor.Color = tempForColors;
			//	_view.SetBackgroundColor (tempForColors);
			//}
			//if (_answerFontColor == null) {
				_view.SetTextColor (Color.Rgb (0xff, 0xff, 0xff));
			//} else {
			//	var tempForColors = Color.Rgb ((Int32)_answerFontColor.red, (Int32)_answerFontColor.green, (Int32)_answerFontColor.blue);
			//	_view.SetTextColor (tempForColors);
			//}
			//if (_answerFontSize == null) {
				_view.TextSize = 18;
			//} else {
			//	_view.TextSize = _answerFontSize.size;
			//}
			return _view;
		}

		public override View GetDropDownView (int position, View convertView, ViewGroup parent)
		{
			TextView _view = (TextView)LayoutInflater.From (context).Inflate (resId, parent, false);
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, "HelveticaNeueLTStd-Lt.otf");
			_view.SetTypeface (font, TypefaceStyle.Normal);
			_view.Text = answers[position];
			_view.SetBackgroundColor (Color.Rgb (0x1B, 0x1B, 0x1B));
			return _view;
		}

//		public override long GetItemId(int position)
//		{
//			return position;
//		}

//		public override int Count
//		{
//			get { return answers.Count; }
//		}

//		public override string this[int position]
//		{
//			get { return answers[position]; }
//		}

	}

}