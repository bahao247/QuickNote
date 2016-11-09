using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace QuickNote1
{
    public partial class HomePage : ContentPage
    {
        private int statusNoteSwitchPriority;
        private TimeZoneInfo localZone = TimeZoneInfo.Local;

        public HomePage()
        {
            InitializeComponent();

            dueDateNotePicker.Date = DateTime.Now;
            dueTimeNotePicker.Time = DateTime.Now.AddMinutes(30).TimeOfDay;

            statusNoteSwitchNone.Toggled += statusNoteSwitchNone_Toggled;
            statusNoteSwitchLow.Toggled += statusNoteSwitchLow_Toggled;
            statusNoteSwitchHigh.Toggled += statusNoteSwitchHigh_Toggled;
            statusNoteSwitchUrgen.Toggled += statusNoteSwitchUrgen_Toggled;
            addButton.Clicked += addButton_Clicked;
            filterNoteSwitch.Toggled += filterNoteSwitch_Toggled;

            noteListView.ItemTemplate = new DataTemplate(typeof(QuickNoteCell));
            noteListView.ItemSelected += noteListView_ItemSelected;

            using (var data = new DataAccess())
            {
                noteListView.ItemsSource = data.GetNotes();
            }
        }

        private void filterNoteSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                noteListView.ItemTemplate = new DataTemplate(typeof(QuickNoteCellFilterDay));
            }
            else
            {
                noteListView.ItemTemplate = new DataTemplate(typeof(QuickNoteCell));
            }
        }

        private void statusNoteSwitchUrgen_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
            {
                statusNoteSwitchPriority = 3;
                statusNoteSwitchNone.IsToggled = false;
                statusNoteSwitchLow.IsToggled = false;
                statusNoteSwitchHigh.IsToggled = false;
            }
        }

        private void statusNoteSwitchHigh_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
            {
                statusNoteSwitchPriority = 2;
                statusNoteSwitchNone.IsToggled = false;
                statusNoteSwitchLow.IsToggled = false;
                statusNoteSwitchUrgen.IsToggled = false;
            }
        }

        private void statusNoteSwitchLow_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
            {
                statusNoteSwitchPriority = 1;
                statusNoteSwitchNone.IsToggled = false;
                statusNoteSwitchHigh.IsToggled = false;
                statusNoteSwitchUrgen.IsToggled = false;
            }
        }

        private void statusNoteSwitchNone_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
            {
                statusNoteSwitchPriority = 0;
                statusNoteSwitchLow.IsToggled = false;
                statusNoteSwitchHigh.IsToggled = false;
                statusNoteSwitchUrgen.IsToggled = false;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (var data = new DataAccess())
            {
                noteListView.ItemsSource = data.GetNotes();
            }
        }

        private void noteListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new EditPage((QuickNote)e.SelectedItem));
        }

        private void addButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameNoteEntry.Text))
            {
                DisplayAlert("Error", "Name of notes must not be blank", "Accept");
                nameNoteEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(typeNoteEntry.Text))
            {
                DisplayAlert("Error", "Type of notes must not be blank", "Accept");
                nameNoteEntry.Focus();
                return;
            }

            if (statusNoteSwitch.IsToggled == true && (dueDateNotePicker.Date + dueTimeNotePicker.Time) <= DateTime.Now)
            {
                dueDateNotePicker.Focus();
                DisplayAlert("Error", "Date of notes must not be blank when note actived", "Accept");
                return;
            }

            QuickNote quicknote = new QuickNote
            {
                NameNote = nameNoteEntry.Text,
                StatusNote = statusNoteSwitch.IsToggled,
                DoneNote = doneNoteSwitch.IsToggled,
                TypeNote = typeNoteEntry.Text,
                DueTimeNote = dueTimeNotePicker.Time,
                DueDateNote = (dueDateNotePicker.Date + dueTimeNotePicker.Time).AddHours(localZone.BaseUtcOffset.Hours),
                PriorityNote = statusNoteSwitchPriority,
                TextNote = textNoteEntry.Text
            };
            
            System.Diagnostics.Debug.WriteLine("--Home Page Long string Data from DueDateNote {0} -  {1} - {2} - {3} -", dueDateNotePicker.Date, dueTimeNotePicker.Time, (dueDateNotePicker.Date + dueTimeNotePicker.Time), (dueDateNotePicker.Date + dueTimeNotePicker.Time).AddHours(7));

            using (var data = new DataAccess())
            {
                data.InsertNote(quicknote);
                noteListView.ItemsSource = data.GetNotes();
            }

            nameNoteEntry.Text = string.Empty;
            statusNoteSwitch.IsToggled = false;
            doneNoteSwitch.IsToggled = false;
            typeNoteEntry.Text = string.Empty;
            dueDateNotePicker.Date = DateTime.Now;
            dueTimeNotePicker.Time = DateTime.Now.AddMinutes(30).TimeOfDay;
            statusNoteSwitchNone.IsToggled = true;
            textNoteEntry.Text = string.Empty;
            statusNoteSwitchHigh.IsToggled = false;
            statusNoteSwitchLow.IsToggled = false;
            statusNoteSwitchUrgen.IsToggled = false;
        }
    }
}
