using CoupleWidgets.Model;
using CoupleWidgets.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CoupleWidgets.Widgets
{
    /// <summary>
    /// Interaction logic for CoupleWidget.xaml
    /// </summary>
    public partial class CoupleWidget : Window
    {

        private DBHelper helper;
        private CoupleData coupleData;

        public CoupleWidget()
        {
            InitializeComponent();
        }

        //Drag event
        private void DragMove(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                helper.updateWidgetPosition(Left, Top);
                DragMove();
            }
        }

        //From load event
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            helper = new DBHelper();
            coupleData = helper.coupleData;

            FirstName.Text = coupleData.firstCoupleName;
            SecondName.Text = coupleData.secondCoupleName;

            DateTime parsedDate = DateTime.Parse(coupleData.startDate);
            DateTime currentDate = DateTime.Now;

            DayCount.Text = currentDate.Subtract(parsedDate).Days.ToString() + " days";
            if (coupleData.firstCoupleImage != "")
            {
                loadCoupleImage(coupleData.firstCoupleImage, FirstImage);
            }
            if (coupleData.secondCoupleImage != "")
            {
                loadCoupleImage(coupleData.secondCoupleImage, SecondImage);
            }

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void loadCoupleImage(string path, System.Windows.Controls.Image ImageView)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(path);
                bitmap.EndInit();
                ImageView.Source = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        void OpenMainWindow(object sender, RoutedEventArgs e)
        {
            helper.updateVisivility(false);
            Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }



        void DateDiff(DateTime today, DateTime from)
        {
            DateTime currentTime = DateTime.Now;
            int months = today.Month - from.Month;
            int years = today.Year - from.Year;

            if (today.Day < from.Day) months--;
            if (months < 0)
            {
                years--;
                months += 12;
            }

            int days = (today - from.AddMonths((years * 12) + months)).Days;
            int hours = currentTime.Hour;
            int minutes = currentTime.Minute;
            int seconds = currentTime.Second;

            DayCount.Text = string.Format("{0}y {1}m {2}d\n{3}h {4}m {5}s", years, months, days, hours, minutes, seconds);
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            DateTime parsedDate = DateTime.Parse(coupleData.startDate);
            DateTime currentDate = DateTime.Today;
            DateDiff(currentDate, parsedDate);
        }
    }
}
