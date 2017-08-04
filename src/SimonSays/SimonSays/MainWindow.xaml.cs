using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimonSays
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Timer timer = new Timer();
		private Random rand = new Random();
		private int[] steps;
		private int tickCounter = 1;
		private int currentStep = 0;
		private int level = 1;
		private int buttonsClicked = 0;
		private int bestScore = 0;

		private List<Button> blueButtons;
		private List<Button> orangeButtons;

		public MainWindow()
		{
			InitializeComponent();

			steps = new int[1];
			steps[0] = rand.Next(0, 9);
			blueButtons = new List<Button>()
			{
				Button11,
				Button21,
				Button31,
				Button41,
				Button51,
				Button61,
				Button71,
				Button81,
				Button91
			};
			orangeButtons = new List<Button>()
			{
				Button12,
				Button22,
				Button32,
				Button42,
				Button52,
				Button62,
				Button72,
				Button82,
				Button92
			};
		}

		private void Button_Author_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("http://www.mohorko.info");
		}

		private void StartNewGame(object sender, RoutedEventArgs e)
		{
			Menu.Visibility = Visibility.Hidden;
			Game.Visibility = Visibility.Visible;

			timer.Elapsed += new ElapsedEventHandler(onTick);
			timer.Interval = 300;
			timer.AutoReset = false;
			timer.Start();
		}

		private void onTick(object sender, ElapsedEventArgs e)
		{
			if(++tickCounter % 2 == 0) {
				tickCounter = 0;
				timer.Start();
				ShowOrangeButton(steps[currentStep]);
			} else {
				ShowBlueButton(steps[currentStep++]);
				if(currentStep < steps.Length) {
					timer.Start();
				} else if(currentStep == steps.Length) {
					currentStep = 0;
					EnableButtons(true);
				}
			}
		}

		private void Click(object sender, RoutedEventArgs e)
		{
			Button btn = sender as Button;
			int number = int.Parse(btn.Tag as string);

			if(steps[buttonsClicked++] != number-1) {
				EnableButtons(false);
				if(level > bestScore) {
					bestScore = level;
					Best.Text = "Best: " + bestScore.ToString();
				}
				MessageBox.Show("Wrong!");
				MessageBox.Show("Try again from the beginning!");
				buttonsClicked = 0;
				steps = new int[1];
				steps[0] = rand.Next(0, 9);
				level = 1;
				Score.Text = level.ToString();
				timer.Start();
			} else if(buttonsClicked == level) {
				EnableButtons(false);
				buttonsClicked = 0;
				level++;
				Score.Text = level.ToString();
				int[] tmp = new int[steps.Length];
				for(int i = 0; i < steps.Length; i++) {
					tmp[i] = steps[i];
				}
				steps = new int[tmp.Length + 1];
				for(int i = 0; i < tmp.Length; i++) {
					steps[i] = tmp[i];
				}
				steps[steps.Length - 1] = rand.Next(0, 9);
				timer.Start();
			}
		}

		private void EnableButtons(bool value)
		{
			Dispatcher.Invoke(delegate
			{
				blueButtons.ForEach(b => b.IsHitTestVisible = value);
			});
		}

		private void ShowBlueButton(int i)
		{
			ShowButton(blueButtons, orangeButtons, i);
		}

		private void ShowOrangeButton(int i)
		{
			ShowButton(orangeButtons, blueButtons, i);
		}

		private void ShowButton(List<Button> show,List<Button> hide,int i)
		{
			Dispatcher.Invoke(delegate
			{
				hide[i].Visibility = Visibility.Hidden;
				show[i].Visibility = Visibility.Visible;
			});
		}
	}
}
