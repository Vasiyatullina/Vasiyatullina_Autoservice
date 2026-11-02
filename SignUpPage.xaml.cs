using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vasiyatullina_Autoservice
{
    public partial class SignUpPage : Page
    {
      
        private Service _currentService = new Service();
        private ClientService _currentClientService = new ClientService();

        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();

            if (SelectedService != null)
                this._currentService = SelectedService;

            DataContext = _currentService;

         
            var _currentClient = Васиятуллина_АвтосервисEntities.GetContext().Client.ToList();
            ComboClient.ItemsSource = _currentClient;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string s = TBStart.Text;

                if (s.Length < 3 || !s.Contains(':'))
                    TBEnd.Text = "";
                else
                {
                    string[] start = s.Split(new char[] { ':' });
                    int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                    int startMin = Convert.ToInt32(start[1].ToString());

                    int sum = startHour + startMin + _currentService.Duration;

                    int EndHour = sum / 60;
                    int EndMin = sum % 60;
                    s = EndHour.ToString("00") + ":" + EndMin.ToString("00");
                    TBEnd.Text = s;
                }
            }
            catch
            {
                TBEnd.Text = "";
            }
        }
        /*
        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TimeSpan.TryParse(TBStart.Text, out TimeSpan startTime))
            {
                DateTime endTime = DateTime.Today + startTime.Add(TimeSpan.FromMinutes(_currentService.Duration));
                TBEnd.Text = endTime.ToString("HH:mm");
            }
        }
        */

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");

            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");

            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

        
            _currentClientService.ClientID = ComboClient.SelectedIndex + 1; 
            _currentClientService.ServiceID = _currentService.ID;
         
            string dateTimeStr = StartDate.Text + " " + TBStart.Text;

            
            if (DateTime.TryParse(dateTimeStr, out DateTime resultDate))
            {
                _currentClientService.StartTime = resultDate;
            }
            else
            {
                MessageBox.Show("Неверный формат даты или времени!\nВведите время в формате ЧЧ:ММ (например, 14:30)");
                return; 
            }

            //_currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);

            if (_currentClientService.ID == 0)
                Васиятуллина_АвтосервисEntities.GetContext().ClientService.Add(_currentClientService);


            try
            {
                Васиятуллина_АвтосервисEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
